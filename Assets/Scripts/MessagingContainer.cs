using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagingContainer : MonoBehaviour
{
    [SerializeField] private float _localYToDestroyMessage;
    [SerializeField] private RectTransform _messagesParentTransform;
    [SerializeField] private GameObject _playerMessagePrefab;
    [SerializeField] private GameObject _gfMessagePrefab;

    public void SpawnMessage(string message, bool isPlayer)
    {
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