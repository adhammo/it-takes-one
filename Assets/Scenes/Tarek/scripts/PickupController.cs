using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

// https://youtu.be/6bFCQqabfzo
public class PickupController : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] Transform holdTransform;
    [SerializeField] string[] tagsToConsider;

    GameObject heldObject;
    Rigidbody heldObjectRb;

    [Header("Physics Parameters")]
    [SerializeField] float pickupRange = 5.0f;
    [SerializeField] float pickupForce = 150.0f;
    [SerializeField] float heightOffset = 2;
    [SerializeField] LayerMask environmentLayer, defaultLayer;


    private bool _pickingUp = false;
    private bool _environmentPickup = false;

    private void FixedUpdate()
    {
        if (heldObject)
        {
            MoveObject();
        }
    }

    public void OnInteract(InputValue value)
    {
        if (value.isPressed)
        {
            _pickingUp = !_pickingUp;

            if (_pickingUp && heldObject == null)
            {
                Vector3 rayOrigin = transform.position + heightOffset * Vector3.up;
                Vector3 rayDirection = (holdTransform.position - rayOrigin).normalized;


                Debug.DrawRay(rayOrigin, rayDirection.normalized * pickupRange, Color.red, 1);
                List<RaycastHit> hits = Physics.RaycastAll(rayOrigin, rayDirection, pickupRange).ToList();

                // Sort hits by distance
                hits.Sort((RaycastHit a, RaycastHit b) =>
                {
                    if (a.distance < b.distance) return -1;
                    else if (a.distance == b.distance) return 0;
                    else return 1;
                });

                // Find the closest hit that has a tag that we care about
                foreach (var hit in hits)
                {
                    if (tagsToConsider.Contains(hit.transform.tag))
                    {
                        PickupObject(hit.transform.gameObject);
                        break;
                    }
                }
            }
            else if (!_pickingUp && heldObject != null)
            {
                DropObject();
            }
        }
    }

    void PickupObject(GameObject pickedUp)
    {
        if (heldObjectRb = pickedUp.GetComponent<Rigidbody>())
        {
            heldObjectRb.useGravity = false;
            heldObjectRb.drag = 10;
            heldObjectRb.constraints = RigidbodyConstraints.FreezeRotation;

            heldObjectRb.transform.parent = holdTransform;
            heldObject = pickedUp;

            if (heldObject.gameObject.layer == environmentLayer)
            {
                heldObject.gameObject.layer = defaultLayer;
                _environmentPickup = true;
            }
        }
    }

    void DropObject()
    {
        heldObjectRb.useGravity = true;
        heldObjectRb.drag = 1;
        heldObjectRb.constraints = RigidbodyConstraints.None;

        if(_environmentPickup)
        {
            heldObject.layer = environmentLayer;
            _environmentPickup = false;
        }

        heldObject.transform.parent = null;
        heldObject = null;
    }

    void MoveObject()
    {
        if (Vector3.Distance(heldObject.transform.position, holdTransform.position) > 0.1f)
        {
            Vector3 moveDirection = (holdTransform.position - heldObject.transform.position);
            heldObjectRb.AddForce(moveDirection * pickupForce);
        }
    }
}
