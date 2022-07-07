using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider)), RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] public float damage = 5f;
    [SerializeField] string[] tagsToIgnore = { "Projectile" };

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerDeath = other.GetComponent<Death>();
            playerDeath?.TakeDamage(damage);
        }

        if (!tagsToIgnore.Contains(other.tag))
            Destroy(this.gameObject);
    }

}
