using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEngineAudioController : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private float _minPitch;
    [SerializeField] private float _maxPitch;

    public void WheelTurned(float val) => _source.pitch = Mathf.Lerp(_minPitch, _maxPitch, val);
}
