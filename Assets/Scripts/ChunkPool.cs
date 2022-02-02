using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChunkPool : MonoBehaviour
{
    [SerializeField] private List<GameObject> _chunkPrefabs;
    [SerializeField] private int _numOfEachPrefab;
    [SerializeField] private int _numOfChunksActive;

    private List<ChunkController> _spawnedChunks = new List<ChunkController>();

    private ChunkController _lastChunk;

    private void Start()
    {
        foreach (var c in _chunkPrefabs)
        {
            for (int i = 0; i < _numOfEachPrefab; ++i)
            {
                var spawned = Instantiate(c).GetComponent<ChunkController>();
                _spawnedChunks.Add(spawned);
                spawned.gameObject.SetActive(false);
                spawned.OnPlayerPassed.AddListener(ChunkExited);
            }
        }

        PlaceChunk(Vector3.zero);
        for (int i = 0; i < _numOfChunksActive - 1; ++i)
            AddChunk();
    }

    private ChunkController GetRandomChunk()
    {
        int numOfChunks = _spawnedChunks.Count;
        var chunk = _spawnedChunks[Random.Range(0, numOfChunks)];
        while (chunk.gameObject.activeSelf)
            chunk = _spawnedChunks[Random.Range(0, numOfChunks)];
        return chunk;
    }

    public void PlaceChunk(Vector3 pos)
    {
        if (_lastChunk)
            throw new Exception("Can't spawn at pos if there is last chunk present, use AddChunk() instead!");
        var c = GetRandomChunk();
        c.PlaceAt(pos);
        _lastChunk = c;
    }

    public void AddChunk()
    {
        if (!_lastChunk)
            throw new Exception("Can't spawn chunk on track without last chunk!");
        var c = GetRandomChunk();
        c.gameObject.SetActive(true);
        c.PlaceBehind(_lastChunk);
        _lastChunk = c;
    }

    public void ChunkExited(ChunkController c)
    {
        c.gameObject.SetActive(false);
        if (c == _lastChunk)
            _lastChunk = null;
        AddChunk();
    }
}