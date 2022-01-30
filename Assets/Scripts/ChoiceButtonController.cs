using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChoiceButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private Image _buttonImage;
    [SerializeField] private float _animationDuration;
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private Button _buttonComponent;

    [SerializeField] private AnimationCurve _bumpUpAnimationCurve;
    [SerializeField] private float _bumpUpDuration;
    [SerializeField] private float _bumpDownDuration;
    [SerializeField] private float _maxScaleMultiplier;
    [SerializeField] private Color _hoverColor;

    private RectTransform _transform;
    private Vector3 _minScale;
    private Vector3 _maxScale;
    private Color _startColor;

    private Coroutine _bumpCoroutine;
    private bool _isShown;

    private void Start()
    {
        _transform = GetComponent<RectTransform>();
        _startColor = _buttonImage.color;
        _minScale = _transform.localScale;
        _maxScale = _maxScaleMultiplier * _minScale;

        _buttonComponent.enabled = false;
        SetAlpha(0);
    }

    public void Show(string message)
    {
        _buttonText.text = message;
        StartCoroutine(ShowAnimation());
    }

    public void Hide()
    {
        if (_isShown)
            StartCoroutine(HideAnimation());
    }

    private IEnumerator ShowAnimation()
    {
        float t = 0;

        while (t < _animationDuration)
        {
            yield return null;
            t += Time.deltaTime;
            float val = Mathf.Lerp(0, 1, _animationCurve.Evaluate(t / _animationDuration));
            SetAlpha(val);
        }

        SetAlpha(1);
        _buttonComponent.enabled = true;
        _isShown = true;
    }

    private IEnumerator HideAnimation()
    {
        float t = 0;
        _buttonComponent.enabled = false;

        while (t < _animationDuration)
        {
            yield return null;
            t += Time.deltaTime;
            float val = Mathf.Lerp(1, 0, _animationCurve.Evaluate(t / _animationDuration));
            SetAlpha(val);
        }

        SetAlpha(0);
        _isShown = false;
    }

    private IEnumerator BumpUp()
    {
        float t = 0;
        while (t < _bumpUpDuration)
        {
            yield return null;
            t += Time.deltaTime;
            float val = _bumpUpAnimationCurve.Evaluate(t / _bumpUpDuration);
            _transform.localScale =
                Vector3.Lerp(_minScale, _maxScale, val);
            SetButtonColor(Color.Lerp(_startColor, _hoverColor, val));
        }

        _transform.localScale = _maxScale;
    }

    private IEnumerator BumpDown()
    {
        float t = 0;
        while (t < _bumpDownDuration)
        {
            yield return null;
            t += Time.deltaTime;
            float val = t / _bumpDownDuration;
            _transform.localScale =
                Vector3.Lerp(_maxScale, _minScale, val);
            SetButtonColor(Color.Lerp(_hoverColor, _startColor, val));
        }

        _transform.localScale = _minScale;
    }

    private void SetAlpha(float val)
    {
        _buttonText.alpha = val;
        var c = _buttonImage.color;
        c.a = val;
        _buttonImage.color = c;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isShown)
        {
            SFXManager.Instance.Play(SFXType.ButtonHover);
            if (_bumpCoroutine != null)
                StopCoroutine(_bumpCoroutine);
            _bumpCoroutine = StartCoroutine(BumpUp());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isShown)
        {
            if (_bumpCoroutine != null)
                StopCoroutine(_bumpCoroutine);
            _bumpCoroutine = StartCoroutine(BumpDown());
        }
    }

    private void SetButtonColor(Color color)
    {
        color.a = _buttonImage.color.a;
        _buttonImage.color = color;
    }
}