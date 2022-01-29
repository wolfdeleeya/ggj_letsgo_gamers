using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameplayScreen;
    [SerializeField] private GameObject _gameWonScreen;
    [SerializeField] private GameObject _gameOverScreen;

    [SerializeField] private Image _overlayImage;
    [SerializeField] private float _startFadeDuration;
    [SerializeField] private AnimationCurve _curve;

    private void Start()
    {
        StartCoroutine(FadeCRT());
        _gameplayScreen.SetActive(true);
        _gameWonScreen.SetActive(false);
        _gameOverScreen.SetActive(false);
    }

    public void ShowGameWon()
    {
        _gameplayScreen.SetActive(false);
        _gameWonScreen.SetActive(true);
    }

    public void ShowGameOver(bool isCrashed)
    {
        _gameplayScreen.SetActive(false);
        _gameOverScreen.SetActive(true);
    }

    private IEnumerator FadeCRT()
    {
        float t = 0;
        while (t < _startFadeDuration)
        {
            yield return null;
            t += Time.deltaTime;
            var c = _overlayImage.color;
            c.a = Mathf.Lerp(1, 0, _curve.Evaluate(t / _startFadeDuration));
            _overlayImage.color = c;
        }
        var c1 = _overlayImage.color;
        c1.a = 0;
        _overlayImage.color = c1;
    }
}