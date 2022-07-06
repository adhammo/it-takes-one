using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Used in case you want multiple buttons (activators in general) to activate a door or something else
public class ActivatorAggregator : MonoBehaviour
{
    [SerializeField] BasePuzzleActivator[] dependencies;
    [SerializeField] UnityEvent onAllActivated;
    [SerializeField] UnityEvent onAnyDeactivated;

    Dictionary<BasePuzzleActivator, bool> _status = new Dictionary<BasePuzzleActivator, bool>();

    public void ActivateOne(BasePuzzleActivator activator)
    {
        if (_status.ContainsKey(activator))
            _status[activator] = true;

        if (_status.Values.ToList().TrueForAll((bool active) => { return active; } ))
        {
            Debug.Log("All requirements activated");
            onAllActivated.Invoke();
        }
    }
    public void DeactivateOne(BasePuzzleActivator activator)
    {
        if (_status.ContainsKey(activator))
            _status[activator] = false;

        onAnyDeactivated.Invoke();
    }


    private void Start()
    {
        foreach(var dep in dependencies)
        {
            dep.SetAggregator(this);
            _status.Add(dep, false);
        }
    }
}
