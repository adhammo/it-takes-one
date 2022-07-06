using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [Header("Bullet")]
    [Tooltip("Bullet speed in m/s")]
    public float BulletSpeed = 20.0f;

    // [Header("Explosion")]
    // [Tooltip("Explosion prefab")]
    // public GameObject Explosion;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rb.velocity = transform.forward * BulletSpeed * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        ContactPoint point = other.contacts[0];
        // Instantiate(Explosion, point.point, Quaternion.LookRotation(point.normal));
        DestroyBullet();
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
