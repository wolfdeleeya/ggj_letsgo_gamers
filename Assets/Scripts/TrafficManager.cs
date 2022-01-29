using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrafficManager : MonoBehaviour
{
    public static TrafficManager Instance;
    
    public List<GameObject> carPrefabs;
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
    
    int index = 0;

    private List<OtherCarController> carScripts;
    private List<Transform> carTransforms;
    private List<GameObject> createdCarPrefabs;
    
    private void Awake()
    {
        Instance = this;
        carScripts = new List<OtherCarController>();
        carTransforms = new List<Transform>();
        createdCarPrefabs = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var carPrefab in carPrefabs)
        {
            var car = Instantiate(carPrefab, Vector3.back, Quaternion.identity);
            car.SetActive(false);
            createdCarPrefabs.Add(car);
        }
        spawnPointsX = new float[numOfLanes];
        CalculateSpawningPoints();
        
        myCarPosition = myCar.GetComponent<Transform>();
        StartCoroutine(Cooldown(spawnDelay));
    }
    
    public IEnumerator Cooldown(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (carList.Count < numOfCars)
        {
            GameObject car = carPrefabs[Random.Range(0, carPrefabs.Count)];
            var auto = Instantiate(car, new Vector3(spawnPointsX[Random.Range(0, numOfLanes)], 0,
                                            myCarPosition.position.z + spawnDistance),
                                            Quaternion.identity);
            carList.Add(auto);
            var carScript = auto.GetComponent<OtherCarController>();
            carTransforms.Add(carScript.transform);
            carScripts.Add(carScript);
            carScript.Speed = Random.Range(lowestSpeed, highestSpeed);
            carScript.Id = index;
            index++;
            StartCoroutine(Cooldown(delay));
        }
    }

    public void CalculateSpawningPoints()
    {
        float d = roadWidth / (numOfLanes * 2);
        float startingX = -roadWidth / 2 + d;
        for (int i = 0; i < numOfLanes; i++)
        {
            spawnPointsX[i] = startingX + i * (2 * d);
        }
    }

    public void RespawnCar(int id)
    {
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < createdCarPrefabs.Count; i++)
        {
            if (!createdCarPrefabs[i].activeInHierarchy)
            {
                availableIndices.Add(i);
            }
        }

        int ind = Random.Range(0, availableIndices.Count);
        var newCar = createdCarPrefabs[availableIndices[ind]];
        
        carList[id].SetActive(false);
        newCar.SetActive(true);

        newCar.transform.position = new Vector3(spawnPointsX[Random.Range(0, numOfLanes)], 0,
            myCarPosition.position.z + spawnDistance);
        newCar.transform.rotation = Quaternion.identity;
        newCar.GetComponent<OtherCarController>().Speed = Random.Range(lowestSpeed, highestSpeed);
        newCar.GetComponent<OtherCarController>().Id = id;
        carList[id] = newCar;
        carList[id].SetActive(true);
    }

    private void Update()
    {
        for (int i = 0; i < carScripts.Count; i++)
        {
            if (carTransforms[i].position.y > 1)
            {
                carTransforms[i].position = new Vector3(carTransforms[i].position.x,0,carTransforms[i].position.z - 20);
                carScripts[i].Speed -= 5;
            }
        }
    }
}
