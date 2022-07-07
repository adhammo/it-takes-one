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
    [Tooltip("Attack distance in meters")]
    public float AttackDistance = 25.0f;
    [Tooltip("Attack box width in meters")]
    public float AttackWidth = 1.0f;
    [Tooltip("Attack box height in meters")]
    public float AttackHeight = 2.0f;
    [Tooltip("Layer to attack in")]
    public LayerMask AttackLayer;

    [Header("Throw")]
    [Tooltip("Throw timeout in seconds")]
    public float ThrowTimeout = 0.4f;
    [Tooltip("Throw cooldown in seconds")]
    public float ThrowCooldown = 1.0f;
    [Tooltip("Throw distance in meters")]
    public float ThrowDistance = 5.0f;
    [Tooltip("Layer to throw through")]
    public LayerMask ThrowLayer;

    [Header("Stats")]
    [Tooltip("Attack damage")]
    public float AttackDamage = 40.0f;
    [Tooltip("Throw damage")]
    public float ThrowDamage = 80.0f;

    [Header("Axe")]
    [Tooltip("Axe to hide")]
    public GameObject AxeGameObject;
    [Tooltip("Axe to throw")]
    public GameObject Axe;

    [Header("Hands")]
    [Tooltip("Hands transform")]
    public Transform Hands;

    [Header("Camera")]
    [Tooltip("The throw direction target")]
    public Transform ThrowTarget;

    [Header("Animations")]
    [Tooltip("Player animator controller")]
    public Animator Anim;

    [Header("Audios")]
    [Tooltip("Attack hit sound")]
    public AudioClip AttackHit;

    public float CurrentThrowCooldown { get { return _throwCooldown; } }

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
    private AudioSource _audioSource;

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
        _audioSource = GetComponent<AudioSource>();

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
            AttackEnemy();

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

    public void AttackEnemy()
    {
        Collider[] hits = Physics.OverlapBox(transform.position + Vector3.up * (AttackHeight / 2f) + transform.forward * (AttackDistance / 2f - 0.5f), new Vector3(AttackWidth / 2f, AttackHeight / 2f, (AttackDistance / 2f) + 0.5f), Quaternion.LookRotation(transform.forward), AttackLayer);

        Collider minHit = null;
        float minSqrDist = float.PositiveInfinity;
        foreach (var hit in hits)
        {
            if (hit.tag != "Enemy") continue;

            float distanceSqr = (transform.position - hit.transform.position).sqrMagnitude;
            if (distanceSqr < minSqrDist)
            {
                minSqrDist = distanceSqr;
                minHit = hit;
            }
        }

        if (minHit != null)
        {
            Debug.Log("Attack damage");
            _audioSource.PlayOneShot(AttackHit);
            BotStatus bot = minHit.GetComponent<BotStatus>();
            bot.TakeDamage(AttackDamage);
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
        Axe axe = Instantiate(Axe, Hands.position, look).GetComponent<Axe>();
        axe.PlayerAudio = _audioSource;
        axe.ThrowDamage = ThrowDamage;
        axe.TravelDistanceSqr = direction.sqrMagnitude;
        Debug.DrawLine(Hands.position, hitPoint);
    }

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Gizmos.color = transparentGreen;

        Gizmos.matrix = Matrix4x4.TRS(transform.position + Vector3.up * (AttackHeight / 2f) + transform.forward * (AttackDistance / 2f - 0.5f), Quaternion.LookRotation(transform.forward), new Vector3(AttackWidth, AttackHeight, AttackDistance + 1f));

        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
}
