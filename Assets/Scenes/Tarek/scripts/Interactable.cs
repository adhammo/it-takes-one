using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{
    [SerializeField] UnityEvent onInteract;

    public void Interact() => onInteract?.Invoke();
}
