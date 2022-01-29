using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrafficManager : MonoBehaviour
{
    public GameObject car;
    public List<GameObject> carList;
    public int numOfCars = 10;
    public int numOfLanes = 6;
    public int lowestSpeed = 20;
    public int highestSpeed = 50;

    public GameObject myCar;
    private Transform myCarPosition;

    [SerializeField] float spawnDistance = 1000f;
    private float[] spawnPointsX ;

    public float spawnDelay = 1.0f;

    public float roadWidth = 30f;

    // Start is called before the first frame update
    void Start()
    {
        spawnPointsX = new float[numOfLanes];
        calculateSpawningPoints();
        
        myCarPosition = myCar.GetComponent<Transform>();
        StartCoroutine(Cooldown(spawnDelay));
    }
    
    public IEnumerator Cooldown(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (carList.Count < numOfCars)
        {
            var auto = Instantiate(car, new Vector3(spawnPointsX[Random.Range(0, numOfLanes)], 0,
                                            myCarPosition.position.z + spawnDistance),
                                            Quaternion.identity);
            carList.Add(auto);
            auto.GetComponent<OtherCarController>().Speed = Random.Range(lowestSpeed, highestSpeed);
            StartCoroutine(Cooldown(delay));
        }
    }

    public void calculateSpawningPoints()
    {
        float d = roadWidth / (numOfLanes * 2);
        float startingX = -roadWidth / 2 + d;
        for (int i = 0; i < numOfLanes; i++)
        {
            spawnPointsX[i] = startingX + i * (2 * d);
        }
    }
}
