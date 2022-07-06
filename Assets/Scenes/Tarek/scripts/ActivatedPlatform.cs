using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatedPlatform : MonoBehaviour
{
    [SerializeField] Transform startPoint, endPoint;
    [SerializeField] float animationDuration = 2f;

    bool _active = false;
    float _timeSinceActivation = 0;

    public void Activate() => _active = true;
    public void Decactivate() => _active = false;

    private void FixedUpdate()
    {
        if (_active)
        {
            _timeSinceActivation += Time.deltaTime;
        }
        else
        {
            _timeSinceActivation -= Time.deltaTime;
        }

        _timeSinceActivation = Mathf.Min(_timeSinceActivation, animationDuration);
        _timeSinceActivation = Mathf.Max(_timeSinceActivation, 0);

        transform.position =  Vector3.Lerp(startPoint.position, endPoint.position, _timeSinceActivation / animationDuration);
    }
}
