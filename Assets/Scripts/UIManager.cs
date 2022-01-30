using System;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameplayScreen;

    [SerializeField] private Image _overlayImage;
    [SerializeField] private float _startFadeDuration;
    [SerializeField] private AnimationCurve _fadeCurve;

    [SerializeField] private ParticleSystem _gameWonParticles;
    [SerializeField] private float _timeBeforeGameWon;
    [SerializeField] private float _gameWonScreenFadeDuration;
    [SerializeField] private AnimationCurve _gameWonScreenFadeCurve;
    [SerializeField] private CanvasGroup _gameWonCanvasGroup;

    [SerializeField] private CanvasGroup _gameOverCanvas;
    [SerializeField] private float _gameOverAnimationDuration;
    [SerializeField] private AnimationCurve _gameOverCurve;
    [SerializeField] private CameraDecolorizeEffectController _decolorizeEffect;
    [SerializeField, Range(0, 1)] private float _minTimeScale;
    [SerializeField] private TextMeshProUGUI _gameOverDescription;
    [SerializeField] private string _crashedString;
    [SerializeField] private string _breakupString;

    private void Start()
    {
        StartCoroutine(FadeCRT());
        _gameplayScreen.SetActive(true);
        ChangeCanvasGroup(_gameWonCanvasGroup, false);
        ChangeCanvasGroup(_gameOverCanvas, false);
    }

    public void ShowGameWon()
    {
        _gameplayScreen.SetActive(false);
        _gameWonParticles.Play();
        StartCoroutine(GameWonCRT());
    }

    public void ShowGameOver(bool isCrashed)
    {
        _gameplayScreen.SetActive(false);
        StartCoroutine(GameOverCRT(isCrashed));
    }

    private void ChangeCanvasGroup(CanvasGroup c, bool isActive)
    {
        c.interactable = isActive;
        c.blocksRaycasts = isActive;
        c.alpha = isActive ? 1 : 0;
    }

    private IEnumerator FadeCRT()
    {
        float t = 0;
        while (t < _startFadeDuration)
        {
            yield return null;
            t += Time.deltaTime;
            var c = _overlayImage.color;
            c.a = Mathf.Lerp(1, 0, _fadeCurve.Evaluate(t / _startFadeDuration));
            _overlayImage.color = c;
        }
        var c1 = _overlayImage.color;
        c1.a = 0;
        _overlayImage.color = c1;
    }

    private IEnumerator GameWonCRT()
    {
        yield return new WaitForSeconds(_timeBeforeGameWon);
        float t = 0;
        while (t < _gameWonScreenFadeDuration)
        {
            yield return null;
            t += Time.deltaTime;
            _gameWonCanvasGroup.alpha =
                Mathf.Lerp(0, 1, _gameWonScreenFadeCurve.Evaluate(t / _gameWonScreenFadeDuration));
        }
        ChangeCanvasGroup(_gameWonCanvasGroup, true);
    }

    private IEnumerator GameOverCRT(bool isCrashed)
    {
        _decolorizeEffect.enabled = true;
        _gameOverDescription.text = isCrashed ? _crashedString : _breakupString;
        float t = 0;
        while (t < _gameOverAnimationDuration)
        {
            yield return null;
            t += Time.deltaTime;
            float val = _gameOverCurve.Evaluate(t / _gameOverAnimationDuration);
            _gameOverCanvas.alpha = Mathf.Lerp(0, 1, val);
            _decolorizeEffect.Intensity = Mathf.Lerp(0, 1, val);
            Time.timeScale = Mathf.Lerp(1, _minTimeScale, val);
        }
        ChangeCanvasGroup(_gameOverCanvas, true);
    }
}