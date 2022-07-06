using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Locomotion))]
public class Gunner : MonoBehaviour
{
    [Header("Fire")]
    [Tooltip("Fire time in seconds")]
    public float FireTimeout = 0.4f;
    [Tooltip("Refire window in seconds")]
    public float ReFireTimeout = 0.1f;

    [Header("Bullet")]
    [Tooltip("Bullet prefab")]
    public GameObject Bullet;

    [Header("Muzzle")]
    [Tooltip("Muzzle transform")]
    public Transform Muzzle;

    [Header("Animations")]
    [Tooltip("Player animator controller")]
    public Animator Anim;

    // fire timeout deltatime
    private float _fireTimeoutDelta;

    // animator hashes
    private int _fireAnimHash = Animator.StringToHash("Fire");

    // firing
    private bool _fired;
    private bool _firing;

    // input
    private bool _fire;

    private Locomotion _playerMove;

    public void OnAttack(InputValue value)
    {
        _fire = value.isPressed;
    }

    private void Start()
    {
        _playerMove = GetComponent<Locomotion>();

        _fired = false;
        _firing = false;

        // reset our timeouts on start
        _fireTimeoutDelta = FireTimeout;
    }

    private void Update()
    {
        fire();
    }

    private void fire()
    {
        if (_fireTimeoutDelta > 0.0f)
        {
            _fireTimeoutDelta -= Time.deltaTime;

            if (_fireTimeoutDelta < ReFireTimeout && _fire)
            {
                // register fire
                _fired = true;
            }
        }
        else if (_playerMove.IsGrounded && (_fired || _fire))
        {
            // unregister fire
            _fired = false;

            // mark firing
            _firing = true;

            FireBullet();

            // reset the fire timeout timer
            _fireTimeoutDelta = FireTimeout;

            // set animator fire
            Anim.SetTrigger(_fireAnimHash);
        }
        else
        {
            // unregister fire
            _fired = false;

            // reset firing
            _firing = false;
        }

        if (!_playerMove.IsGrounded)
        {
            // reset firing
            _firing = false;
        }

        _playerMove.CanJump = !_firing;
    }

    private void FireBullet()
    {
        Instantiate(Bullet, Muzzle.position, Muzzle.rotation);
    }
}
