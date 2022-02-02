using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public enum Scene {MainMenu, Gameplay}
    public static SceneManager Instance { get; private set; }

    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeScene(Scene s)
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene((int) s);
    } 

    public void Quit() => Application.Quit();
}
