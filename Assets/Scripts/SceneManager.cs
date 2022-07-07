using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] GameObject blockade, bossCube, pistolGameObject;

#if !UNITY_IOS || !UNITY_ANDROID
    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
#endif
    
    public void BossDead()
    {
        PlayerPrefs.SetInt("Level", 3);
    }

    public void Start()
    {
        if(PlayerPrefs.GetInt("Level") != 2)
        {
            blockade.SetActive(false);
            bossCube.SetActive(false);
            pistolGameObject.SetActive(false);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Gunner>().enabled = false;
        }   
    }

#if !UNITY_IOS || !UNITY_ANDROID
    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
#endif
}
