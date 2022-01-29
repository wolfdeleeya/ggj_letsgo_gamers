using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageBubbleController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private float _minScale;
    [SerializeField] private float _maxScale;
    [SerializeField] private float _animationDuration;

    private RectTransform _transform;

    private void Start()
    {
        _transform = GetComponent<RectTransform>();
    }

    public void Initialize(string message)
    {
        _messageText.text = message;
        StartCoroutine(BumpAnimation());
    }

    private IEnumerator BumpAnimation()
    {
        float t = 0;
        Vector3 maxScale = Vector3.one * _maxScale;
        Vector3 minScale = Vector3.one * _minScale;
        while (t < _animationDuration)
        {
            yield return null;
            t += Time.deltaTime;
            _transform.localScale = Vector3.Lerp(minScale, maxScale, _animationCurve.Evaluate(t / _animationDuration));
        }
    }
}