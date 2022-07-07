using UnityEngine;

public class Throw : MonoBehaviour
{
    [Header("Fighter")]
    [Tooltip("Fighter to throw axe")]
    public Fighter PlayerFighter;

    public void ThrowAxe()
    {
        PlayerFighter.ThrowAxe();
    }
}
