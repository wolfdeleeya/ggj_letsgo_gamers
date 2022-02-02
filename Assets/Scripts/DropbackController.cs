using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropbackController : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _zOffsetFromCamera;
    
    private Vector3 _startPosition;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
        _startPosition = _transform.position;
        _startPosition.z = 0;
    }

    private void LateUpdate()
    {
        _transform.position = _startPosition + Vector3.forward * (_cameraTransform.position.z + _zOffsetFromCamera);
    }
}
