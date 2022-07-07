using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("Fighter")]
    [Tooltip("Fighter to attack")]
    public Fighter PlayerFighter;

    public void AttackEnemy()
    {
        PlayerFighter.AttackEnemy();
    }
}
