using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Image _imageToAnimate;
    [SerializeField] private float _animationDuration;
    [SerializeField] private AnimationCurve _curve;

    public void Play() => StartCoroutine(Fade());

    public void Quit() => SceneManager.Instance.Quit();

    private IEnumerator Fade()
    {
        float t = 0;
        var c = _imageToAnimate.color;
        while (t < _animationDuration)
        {
            yield return null;
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, _curve.Evaluate(t / _animationDuration));
            _imageToAnimate.color = c;
        }
        SceneManager.Instance.ChangeScene(SceneManager.Scene.Gameplay);
    }
}
