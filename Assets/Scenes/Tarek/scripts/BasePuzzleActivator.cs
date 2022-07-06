using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// All activators (buttons, lazers, etc..) should inherit from this
public class BasePuzzleActivator : MonoBehaviour
{
    protected ActivatorAggregator _aggregator;

    public void SetAggregator(ActivatorAggregator agg) => _aggregator = agg;

    public void Activate() => _aggregator.ActivateOne(this);

    public void Deactivate() => _aggregator.DeactivateOne(this);
}
