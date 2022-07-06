using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleWireIndicator : MonoBehaviour
{
    [SerializeField] Material activatedMaterial, deactivatedMaterial;


    LineRenderer _lineRenderer;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        Deactivate();
    }

    public void Activate() =>
        _lineRenderer.material = activatedMaterial;

    public void Deactivate() =>
        _lineRenderer.material = deactivatedMaterial;
}
