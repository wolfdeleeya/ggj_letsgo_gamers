using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class OtherCarController : MonoBehaviour
{
    [field: SerializeField] public float Speed { get; set; }
    private Rigidbody rb;
    public float slowDownDistance;
    public LayerMask otherCarsLayer;

    public Transform shootingPoint;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        slowDownDistance = Random.Range(slowDownDistance - 2, slowDownDistance + 2);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(0, 0, Speed);
        
        RaycastHit hit;
        if (Physics.Raycast(shootingPoint.position, rb.velocity, out hit, slowDownDistance, otherCarsLayer))
        {
            float newSpeed = hit.transform.gameObject.GetComponent<OtherCarController>().Speed;
            Speed = newSpeed;
        }
    }
}
