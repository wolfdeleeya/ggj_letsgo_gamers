using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoicesUIController : MonoBehaviour
{
    [SerializeField] private List<ChoiceButtonController> _buttons;
    [SerializeField] private float _timeBetweenButtonFades;
    [SerializeField] private Image _notificationImage;
    [SerializeField] private float _notificationAnimationDuration;
    [SerializeField] private AnimationCurve _notificationAnimationCurve;

    private bool _isPhoneInFocus;
    private MessageSO _cachedMessage;
    private Coroutine _notificationCoroutine;

    private void Start()
    {
        var c = _notificationImage.color;
        c.a = 0;
        _notificationImage.color = c;
    }

    public void PhonePickedUp()
    {
        _isPhoneInFocus = true;
        if (_cachedMessage)
        {
            StartNotificationAnimation(false);
            ShowChoices(_cachedMessage);
        }
    }

    public void PhoneDropped()
    {
        _isPhoneInFocus = false;
        if (_cachedMessage)
        {
            StartNotificationAnimation(true);
            HideChoices();
        }
    }
    
    public void StartNotificationAnimation(bool willShow)
    {
        if (_notificationCoroutine != null)
            StopCoroutine(_notificationCoroutine);
        _notificationCoroutine = StartCoroutine(NotificationCoroutine(willShow));
    }

    private IEnumerator NotificationCoroutine(bool willAppear)
    {
        float t = 0;
        float min = willAppear ? 0 : 1;
        float max = 1 - min;
        var c = _notificationImage.color;
        while (t < _notificationAnimationDuration)
        {
            yield return null;
            t += Time.deltaTime;
            c.a = Mathf.Lerp(min, max, _notificationAnimationCurve.Evaluate(t / _notificationAnimationDuration));
            _notificationImage.color = c;
        }

        c.a = max;
        _notificationImage.color = c;
    }

    public void ShowChoices(MessageSO message)
    {
        StartCoroutine(ChoicesIterativeCRT(message));
    }

    public void ChoicePicked()
    {
        SFXManager.Instance.Play(SFXType.ButtonClick);
        HideChoices();
        _cachedMessage = null;
    }

    public void MessageReceived(MessageSO message)
    {
        if (_isPhoneInFocus)
        {
            ShowChoices(message);
            _cachedMessage = message;
        }
        else
        {
            StartNotificationAnimation(true);
            _cachedMessage = message;
        }
    }
    
    public void HideChoices()
    {
        foreach (var b in _buttons)
            b.Hide();
    }

    private IEnumerator ChoicesIterativeCRT(MessageSO message)
    {
        var responses = message.Responses;
        for (int i = 0; i < message.Responses.Count; ++i)
        {
            _buttons[i].Show(responses[i].ResponseText);
            yield return new WaitForSeconds(_timeBetweenButtonFades);
        }
    }
}