using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] GameObject blockade, bossCube, pistolGameObject;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;

    public void BossDead()
    {
        PlayerPrefs.SetInt("Level", 3);
        UnityEngine.SceneManagement.SceneManager.LoadScene("ZozoScene");
    }

    public void Start()
    {
        if (PlayerPrefs.GetInt("Level") != 2)
        {
            blockade.SetActive(false);
            bossCube.SetActive(false);
            pistolGameObject.SetActive(false);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Gunner>().enabled = false;
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
