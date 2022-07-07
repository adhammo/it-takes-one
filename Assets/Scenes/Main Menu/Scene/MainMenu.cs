using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TarekScene"); 
    }
    
    public void Quit()
    {
        Application.Quit();

    }    
    
}
