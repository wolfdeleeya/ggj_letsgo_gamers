using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessagingContainer : MonoBehaviour
{
    [SerializeField] private float _localYToDestroyMessage;
    [SerializeField] private RectTransform _messagesParentTransform;
    [SerializeField] private GameObject _playerMessagePrefab;
    [SerializeField] private GameObject _gfMessagePrefab;
    [SerializeField] private GameObject _typingGameObject;

    private void Start()
    {
        _typingGameObject.SetActive(false);
    }

    public void StartTyping() => _typingGameObject.SetActive(true);
    
    public void SpawnMessage(string message, bool isPlayer)
    {
        if(!isPlayer)
            _typingGameObject.SetActive(false);
        Instantiate(isPlayer ? _playerMessagePrefab : _gfMessagePrefab, _messagesParentTransform)
            .GetComponent<MessageBubbleController>().Initialize(message);
        Cleanup();
    }

    private void Cleanup()
    {
        int numOfChildren = _messagesParentTransform.childCount;
        for (int i = 0; i < numOfChildren; ++i)
        {
            var child = _messagesParentTransform.GetChild(i);
            if (child.localPosition.y > _localYToDestroyMessage)
                Destroy(child.gameObject);
            else
                return;
        }
    }
}