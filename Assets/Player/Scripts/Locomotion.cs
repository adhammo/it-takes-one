using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class Locomotion : MonoBehaviour
{
    [Header("Player")]
    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 4.0f;
    [Tooltip("Sprint speed of the character in m/s")]
    public float SprintSpeed = 6.0f;
    [Tooltip("Rotation speed of the character")]
    public float RotationSpeed = 1.0f;
    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;
    [Tooltip("Change speed based on input sensitivity")]
    public bool AnalogMovement = false;

    [Space(10)]
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.2f;
    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -9.81f;

    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.1f;
    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;

    [Header("Grounded")]
    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;
    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.5f;
    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;

    [Header("Camera")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;
    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 90.0f;
    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -90.0f;

    [Header("Animations")]
    [Tooltip("Player animator controller")]
    public Animator Anim;

    [HideInInspector()]
    public bool CanJump = true;
    public bool IsGrounded { get { return _grounded && !_jumped; } }

    // input
    private Vector2 _move;
    private Vector2 _look;
    private bool _jump;
    private bool _sprint;

    // cinemachine
    private float _cinemachineTargetPitch;

    // player
    private float _speed;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _stickingVelocity = -4.0f;

    // grounded
    [Header("Stats")]
    [Tooltip("If the character is grounded or not")]
    [SerializeField]
    private bool _grounded;
    private bool _jumped;

    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;

    // animator hashes
    private int _moveAnimHash = Animator.StringToHash("Move");
    private int _runAnimHash = Animator.StringToHash("Run");
    private int _fallAnimHash = Animator.StringToHash("Fall");
    private int _jumpAnimHash = Animator.StringToHash("Jump");
    private int _groundAnimHash = Animator.StringToHash("Ground");

    private PlayerInput _playerInput;
    private CharacterController _controller;

    private const float _threshold = 0.01f;


    // Keeps track of the last grounded position we had before jumping.
    public Vector3 _lastGroundedPosition;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();

        _grounded = false;
        _jumped = false;

        // reset our timeouts on start
        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;
    }

    private bool IsCurrentDeviceMouse => _playerInput.currentControlScheme == "KeyboardMouse";

    public void OnMove(InputValue value)
    {
        _move = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        _look = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        _jump = value.isPressed;
    }

    public void OnSprint(InputValue value)
    {
        _sprint = value.isPressed;
    }

    public void TeleportToLastGround()
    {
        _controller.enabled = false;
        transform.position = _lastGroundedPosition;
        _controller.enabled = true;
    }

    private void Update()
    {
        JumpAndGravity();
        Movement();
    }

    private void FixedUpdate()
    {
        GroundedCheck();
    }

    private void LateUpdate()
    {
        Rotation();
    }

    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = transform.position + Vector3.down * GroundedOffset;
        _grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

        // set animator ground
        Anim.SetBool(_groundAnimHash, _grounded);
    }

    private void JumpAndGravity()
    {
        // apply gravity over time
        _verticalVelocity += Gravity * Time.deltaTime;

        if (_grounded)
        {
            // reset the fall timeout timer
            _fallTimeoutDelta = FallTimeout;

            // stop our velocity dropping infinitely when grounded
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = _stickingVelocity;
            }

            // jump timeout
            if (_jumpTimeoutDelta > 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
            else if (CanJump && !_jumped && _jump)
            {
                // mark jumped
                _jumped = true;

                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                // set animator jump
                Anim.SetTrigger(_jumpAnimHash);
                
                _lastGroundedPosition = transform.position;
            }
        }
        else
        {
            // reset the jump timeout timer
            _jumped = false;
            _jumpTimeoutDelta = JumpTimeout;

            // fall timeout
            if (_fallTimeoutDelta > 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
        }

        // set animator fall
        Anim.SetBool(_fallAnimHash, _fallTimeoutDelta <= 0);
    }

    private void Movement()
    {
        // set target speed based on move speed, sprint speed and if sprint is pressed
        float targetSpeed = _sprint ? SprintSpeed : MoveSpeed;

        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (_move == Vector2.zero) targetSpeed = 0.0f;

        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = AnalogMovement ? _move.magnitude : 1f;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, SpeedChangeRate * Time.deltaTime);

            // round speed to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        // input direction
        Vector3 input = transform.right * _move.x + transform.forward * _move.y;

        // move the player
        Vector3 velocity = input.normalized * _speed + Vector3.up * (_verticalVelocity + 0.5f * Gravity * Time.deltaTime);
        _controller.Move(velocity * Time.deltaTime);

        // set animator move and run
        Anim.SetBool(_moveAnimHash, _speed > 0.01f);
        Anim.SetBool(_runAnimHash, _sprint);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void Rotation()
    {
        // if there is an input
        if (_look.sqrMagnitude >= _threshold)
        {
            // don't multiply mouse input by Time.deltaTime
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetPitch += _look.y * RotationSpeed * deltaTimeMultiplier;
            _rotationVelocity = _look.x * RotationSpeed * deltaTimeMultiplier;

            // clamp our pitch rotation
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // update Cinemachine camera target pitch
            CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

            // rotate the player left and right
            transform.Rotate(Vector3.up * _rotationVelocity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (_grounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Vector3 spherePosition = transform.position + Vector3.down * GroundedOffset;
        Gizmos.DrawSphere(spherePosition, GroundedRadius);
    }
}
