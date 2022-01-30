using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent OnGameWon;
    public UnityEventBool OnGameOver;

    private bool _isGameFinished;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Time.timeScale = 1;
    }

    public void GameWon()
    {
        if (!_isGameFinished)
        {
            _isGameFinished = true;
            OnGameWon.Invoke();
        }
    }

    public void GameOver(bool isCrashed)
    {
        if (!_isGameFinished)
        {
            _isGameFinished = true;
            OnGameOver.Invoke(isCrashed);
        }
    }

    public void Restart() => SceneManager.Instance.ChangeScene(SceneManager.Scene.Gameplay);

    public void GoToMenu() => SceneManager.Instance.ChangeScene(SceneManager.Scene.MainMenu);
}