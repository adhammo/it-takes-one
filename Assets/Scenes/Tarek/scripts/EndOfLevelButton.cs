using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfLevelButton : MonoBehaviour
{
    public void ToNextLevel()
    {
        PlayerPrefs.SetInt("Level", 1);
        // Fade out effect
        UnityEngine.SceneManagement.SceneManager.LoadScene("ZozoScene");
    }
}
