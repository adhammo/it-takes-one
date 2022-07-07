using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfLevelButton : MonoBehaviour
{
    public void ToNextLevel()
    {
        PlayerPrefs.SetInt("Level", 1);
        // Fade out effect
    }
}
