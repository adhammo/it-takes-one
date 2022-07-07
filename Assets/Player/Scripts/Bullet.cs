using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet")]
    [Tooltip("Bullet speed in m/s")]
    public float BulletSpeed = 100.0f;
    [Tooltip("Bullet offset from target")]
    public float BulletOffset = 50.0f;

    [HideInInspector()]
    public float TravelDistanceSqr = 10000.0f;

    private Vector3 _startPos;

    private void Start()
    {
        _startPos = transform.position;
    }

    private void Update()
    {
        transform.position += transform.forward * BulletSpeed * Time.deltaTime;

        float distanceSqr = (transform.position - _startPos).sqrMagnitude;
        if (distanceSqr >= TravelDistanceSqr + BulletOffset)
        {
            DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
