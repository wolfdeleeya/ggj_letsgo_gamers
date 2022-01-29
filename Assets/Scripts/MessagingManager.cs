using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MessagingManager : MonoBehaviour
{
    public UnityEventMessageSO OnMessageReceived;
    public UnityEvent OnMessageSent;
    public UnityEventFloat OnGFAngerChanged;
    
    [SerializeField] private MessageSO _beginningMessage;
    [SerializeField] private MessagingContainer _container;

    private MessageSO _currentMessage;
    private float _gfAnger;

    public float GFAnger
    {
        get
        {
            return _gfAnger;
        }
        
        set
        {
            _gfAnger = Mathf.Clamp(value, 0, 100);
            OnGFAngerChanged.Invoke(_gfAnger);
            Debug.Log(_gfAnger);
            if(_gfAnger >= 100)
                GameManager.Instance.GameOver();
        }
    }

    private void Start()
    {
        StartCoroutine(SendNewMessage(_beginningMessage));
    }

    private IEnumerator SendNewMessage(MessageSO message)
    {
        yield return new WaitForSeconds(message.TypingDuration);
        _currentMessage = message;
        _container.SpawnMessage(_currentMessage.MessageText, false);
        OnMessageReceived.Invoke(_currentMessage);
        if(_currentMessage.IsInstantGameOver)
            GameManager.Instance.GameOver();
        else if(_currentMessage.IsInstantGameWon)
            GameManager.Instance.GameWon();
    }

    public void Respond(int responseIndex)
    {
        var response = _currentMessage.Responses[responseIndex];
        _container.SpawnMessage(response.ResponseText, true);
        OnMessageSent.Invoke();
        GFAnger += response.AngerDelta;
        StartCoroutine(SendNewMessage(response.NextMessage));
    }

    private void Update()
    {
        if (_currentMessage)
            GFAnger += _currentMessage.AngerSpeedPerSecond * Time.deltaTime;
    }
}