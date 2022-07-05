using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerAttack : MonoBehaviour
{
    [Header("Attack")]
    [Tooltip("Attack time in seconds")]
    public float AttackTimeout = 0.4f;

    [Header("Animations")]
    [Tooltip("Player animator controller")]
    public Animator Anim;

    // attack timeout deltatime
    private float _attackTimeoutDelta;

    // animator hashes
    private int _attackAnimHash = Animator.StringToHash("Attack");

    // input
    private bool _attack;

    public void OnAttack(InputValue value)
    {
        _attack = value.isPressed;
    }

    private void Start()
    {
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
        }
        else if (_attack)
        {
            // reset the attack timeout timer
            _attackTimeoutDelta = AttackTimeout;

            // set animator attack
            Anim.SetTrigger(_attackAnimHash);
        }
    }
}
