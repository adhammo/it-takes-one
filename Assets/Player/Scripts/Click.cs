using UnityEngine;
using UnityEngine.InputSystem;

public class Click : MonoBehaviour
{
    public void OnClick(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("clicked");
        }
    }
}
