using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Locomotion))]
public class Fighter : MonoBehaviour
{
    [Header("Attack")]
    [Tooltip("Attack timeout in seconds")]
    public float AttackTimeout = 0.4f;
    [Tooltip("Reattack window in seconds")]
    public float ReAttackTimeout = 0.1f;

    [Header("Throw")]
    [Tooltip("Throw timeout in seconds")]
    public float ThrowTimeout = 0.4f;
    [Tooltip("Throw cooldown in seconds")]
    public float ThrowCooldown = 1.0f;
    [Tooltip("Throw distance in meters")]
    public float ThrowDistance = 25.0f;
    [Tooltip("Layer to throw through")]
    public LayerMask ThrowLayer;

    [Header("Axe")]
    [Tooltip("Axe to hide")]
    public GameObject AxeGameObject;
    [Tooltip("Axe to throw")]
    public GameObject Axe;

    [Header("Hands")]
    [Tooltip("Hands transform")]
    public Transform Hands;
    [Tooltip("Power visual effect")]
    public ParticleSystem Power;

    [Header("Camera")]
    [Tooltip("The throw direction target")]
    public Transform ThrowTarget;

    [Header("Animations")]
    [Tooltip("Player animator controller")]
    public Animator Anim;

    // timeout deltatime
    private float _attackTimeoutDelta;
    private float _throwTimeoutDelta;
    private float _throwCooldown;

    // animator hashes
    private int _attackAnimHash = Animator.StringToHash("Attack");
    private int _throwAnimHash = Animator.StringToHash("Throw");

    // attacking
    private bool _attacked;
    private bool _attacking;

    // throwing
    private bool _throwing;

    // input
    private bool _attack;

    private Locomotion _playerMove;

    public void OnAttack(InputValue value)
    {
        _attack = value.isPressed;
    }

    public void OnThrow(InputValue value)
    {
        if (value.isPressed)
        {
            if (_throwCooldown <= 0.0f && !_attacking && _playerMove.IsGrounded)
            {
                // unregister attack
                _attacked = false;

                // mark throwing
                _throwing = true;

                // reset the throw timeout and cooldown timer
                _throwTimeoutDelta = ThrowTimeout;
                _throwCooldown = ThrowCooldown;

                // set animator throw
                Anim.SetTrigger(_throwAnimHash);
            }
        }
    }

    private void Start()
    {
        _playerMove = GetComponent<Locomotion>();

        _attacked = false;
        _attacking = false;

        _throwing = false;

        // reset our timeouts on start
        _attackTimeoutDelta = AttackTimeout;
        _throwTimeoutDelta = ThrowTimeout;
        _throwCooldown = ThrowCooldown;
    }

    private void Update()
    {
        Attack();
        Throw();

        if (!_playerMove.IsGrounded)
        {
            // reset attacking and throwing
            _attacking = false;
            _throwing = false;
        }

        _playerMove.CanJump = !_attacking && !_throwing;
    }

    private void Attack()
    {
        if (_attackTimeoutDelta > 0.0f)
        {
            _attackTimeoutDelta -= Time.deltaTime;

            if (_attackTimeoutDelta < ReAttackTimeout && !_throwing && _attack)
            {
                // register attack
                _attacked = true;
            }
        }
        else if (_playerMove.IsGrounded && !_throwing && (_attacked || _attack))
        {
            // unregister attack
            _attacked = false;

            // mark attacking
            _attacking = true;

            // reset the attack timeout timer
            _attackTimeoutDelta = AttackTimeout;

            // set animator attack
            Anim.SetTrigger(_attackAnimHash);
        }
        else
        {
            // unregister attack
            _attacked = false;

            // reset attacking
            _attacking = false;
        }
    }

    private void Throw()
    {
        if (_throwTimeoutDelta > 0.0f)
        {
            _throwTimeoutDelta -= Time.deltaTime;
        }
        else
        {
            // reset throwing
            _throwing = false;
            ResetThrowEffects();
        }

        if (_throwCooldown > 0.0f)
        {
            _throwCooldown -= Time.deltaTime;
        }
    }

    private void StartThrowEffects()
    {
        AxeGameObject.SetActive(false);
    }

    private void ResetThrowEffects()
    {
        AxeGameObject.SetActive(true);
    }

    public void ThrowAxe()
    {
        StartThrowEffects();

        Vector3 hitPoint = ThrowTarget.position + ThrowTarget.forward * ThrowDistance;
        if (Physics.Raycast(ThrowTarget.position, ThrowTarget.forward, out RaycastHit hit, ThrowDistance, ThrowLayer))
        {
            hitPoint = hit.point;
        }

        Vector3 direction = hitPoint - Hands.position;
        Quaternion look = Quaternion.LookRotation(direction.normalized, ThrowTarget.up);
        GameObject bullet = Instantiate(Axe, Hands.position, look);
        bullet.GetComponent<Axe>().TravelDistanceSqr = direction.sqrMagnitude;
        Debug.DrawLine(Hands.position, hitPoint);
    }
}
