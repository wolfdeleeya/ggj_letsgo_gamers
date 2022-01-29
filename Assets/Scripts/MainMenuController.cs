using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void Play() => SceneManager.Instance.ChangeScene(SceneManager.Scene.Gameplay);

    public void Quit() => SceneManager.Instance.Quit();
}
