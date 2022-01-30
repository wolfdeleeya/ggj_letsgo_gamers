using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class RadioChannel
{
    public List<AudioClip> ChannelClips;
}

public class RadioController : MonoBehaviour
{
    [SerializeField] private List<RadioChannel> _channels;
    private AudioSource _source;
    private int _currentChannel;
    
    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        if(_channels[_currentChannel].ChannelClips.Count > 0)
            _source.PlayOneShot(GetRandomAudio(_currentChannel));
    }

    private void Update()
    {
        if(_channels[_currentChannel].ChannelClips.Count > 0 && !_source.isPlaying)
            _source.PlayOneShot(GetRandomAudio(_currentChannel));
    }

    private AudioClip GetRandomAudio(int channelIndex)
    {
        var channel = _channels[channelIndex];
        return channel.ChannelClips[Random.Range(0, channel.ChannelClips.Count)];
    }

    public void SwitchChannel()
    {
        SFXManager.Instance.Play(SFXType.RadioClick);
        _currentChannel = (_currentChannel + 1) % _channels.Count;
        _source.Stop();
        if(_channels[_currentChannel].ChannelClips.Count > 0)
            _source.PlayOneShot(GetRandomAudio(_currentChannel));
    }
}
