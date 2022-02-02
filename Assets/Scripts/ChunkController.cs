using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class ChunkController : MonoBehaviour
{
    [HideInInspector] public UnityEventChunkController OnPlayerPassed = new UnityEventChunkController();
    [SerializeField] private Transform _chunkEndPoint;
    [SerializeField] private List<GameObject> _animals;
    [SerializeField, Range(0, 1)] private float _chanceToSpawnAnimal;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    public void PlaceBehind(ChunkController c)
    {
        _transform.position = c._chunkEndPoint.position;
    }

    public void PlaceAt(Vector3 pos)
    {
        _transform.position = pos;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            OnPlayerPassed.Invoke(this);
    }

    private void OnEnable()
    {
        foreach (var a in _animals)
            a.SetActive(Random.Range(0, 1f) < _chanceToSpawnAnimal);
    }
}