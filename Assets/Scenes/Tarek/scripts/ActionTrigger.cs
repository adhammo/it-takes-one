using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class ActionTrigger : BasePuzzleActivator
{
    // Addition actions in case you need actions independent of an aggregator
    [SerializeField] UnityEvent onActivate;
    [SerializeField] UnityEvent onDeactivate;

    [SerializeField] string[] tagsToConsider;

    Collider _colliderInside;

    private void OnTriggerEnter(Collider other)
    {
        if (tagsToConsider.Contains(other.tag))
        {
            _colliderInside = other;

            if (_aggregator)
                Activate();

            if (onActivate != null)
                onActivate.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (tagsToConsider.Contains(other.tag) && other == _colliderInside)
        {
            _colliderInside = null;

            if (_aggregator)
                Deactivate();

            if (onDeactivate != null)
                onDeactivate.Invoke();
        }
    }

    public void TestFn()
    {
        Debug.Log("Button pressed");
    }
}

