using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Locomotion))]
public class Fighter : MonoBehaviour
{
    [Header("Attack")]
    [Tooltip("Attack time in seconds")]
    public float AttackTimeout = 0.4f;
    [Tooltip("Reattack window in seconds")]
    public float ReAttackTimeout = 0.1f;

    [Header("Animations")]
    [Tooltip("Player animator controller")]
    public Animator Anim;

    // attack timeout deltatime
    private float _attackTimeoutDelta;

    // animator hashes
    private int _attackAnimHash = Animator.StringToHash("Attack");

    // attacking
    private bool _attacked;
    private bool _attacking;

    // input
    private bool _attack;

    private Locomotion _playerMove;

    public void OnAttack(InputValue value)
    {
        _attack = value.isPressed;
    }

    private void Start()
    {
        _playerMove = GetComponent<Locomotion>();

        _attacked = false;
        _attacking = false;

        // reset our timeouts on start
        _attackTimeoutDelta = AttackTimeout;
    }

    private void Update()
    {
        Attack();
    }

    private void Attack()
    {
        if (_attackTimeoutDelta > 0.0f)
        {
            _attackTimeoutDelta -= Time.deltaTime;

            if (_attackTimeoutDelta < ReAttackTimeout && _attack)
            {
                // register attack
                _attacked = true;
            }
        }
        else if (_playerMove.IsGrounded && (_attacked || _attack))
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

        if (!_playerMove.IsGrounded)
        {
            // reset attacking
            _attacking = false;
        }

        _playerMove.CanJump = !_attacking;
    }
}
