using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DictionaryEntry
{
    public SFXType Key;
    public AudioClip Value;
}

[Serializable]
public enum SFXType {ButtonHover, ButtonClick, RadioClick, MessageNotification}

public class SFXManager : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private List<DictionaryEntry> _dictionaryEntries;

    private Dictionary<SFXType, AudioClip> _dictionary = new Dictionary<SFXType, AudioClip>();

    public static SFXManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        foreach (var e in _dictionaryEntries)
            _dictionary.Add(e.Key, e.Value);
    }

    public void Play(SFXType s) => _source.PlayOneShot(_dictionary[s]);
}
