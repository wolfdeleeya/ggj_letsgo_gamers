using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Messages/Message", fileName = "New Message")]
public class MessageSO : ScriptableObject
{
    public string MessageText;
    public float TypingDuration;
    public List<Response> Responses;
    public bool IsInstantGameOver;
    public bool IsInstantGameWon;
}

[Serializable]
public class Response
{
    public string ResponseText;
    public MessageSO NextMessage;
    [Range(-100, 100)] public float AngerDelta;
}