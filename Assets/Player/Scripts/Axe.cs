using UnityEngine;

public class Axe : MonoBehaviour
{
    [Header("Axe")]
    [Tooltip("Axe speed in m/s")]
    public float AxeSpeed = 20.0f;
    [Tooltip("Axe offset from target")]
    public float AxeOffset = 5.0f;

    [HideInInspector()]
    public float ThrowDamage = 80.0f;
    [HideInInspector()]
    public float TravelDistanceSqr = 2500.0f;

    private Vector3 _startPos;

    private void Start()
    {
        _startPos = transform.position;
    }

    private void Update()
    {
        transform.position += transform.forward * AxeSpeed * Time.deltaTime;

        float distanceSqr = (transform.position - _startPos).sqrMagnitude;
        if (distanceSqr >= TravelDistanceSqr + AxeOffset)
        {
            DestroyAxe();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Throw damage");
        //ThrowDamage
    }

    private void DestroyAxe()
    {
        Destroy(gameObject);
    }
}
