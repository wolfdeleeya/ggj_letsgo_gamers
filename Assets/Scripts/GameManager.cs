
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent OnGameWon;
    public UnityEventBool OnGameOver;
    
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void GameWon() => OnGameWon.Invoke();

    public void GameOver(bool isCrashed) => OnGameOver.Invoke(isCrashed);
    
    public void Restart() => SceneManager.Instance.ChangeScene(SceneManager.Scene.Gameplay);

    public void GoToMenu() => SceneManager.Instance.ChangeScene(SceneManager.Scene.MainMenu);
}
