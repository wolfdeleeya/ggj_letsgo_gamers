using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoicesUIController : MonoBehaviour
{
    [SerializeField] private List<ChoiceButtonController> _buttons;
    [SerializeField] private float _timeBetweenButtonFades;

    public void ShowChoices(MessageSO message)
    {
        StartCoroutine(ChoicesIterativeCRT(message));
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
