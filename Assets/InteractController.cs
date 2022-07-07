using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractController : MonoBehaviour
{
    [SerializeField] Transform holder;
    [SerializeField] float interactionRange = 5f;

    public void OnInteract()
    {
        Vector3 direction = (transform.position - holder.position).normalized;
        Debug.DrawRay(transform.position, direction * interactionRange);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, interactionRange);

        foreach (var hit in hits)
        {
            Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();
            if (interactable)
            {
                interactable.Interact();
            }
        }
    }
}
