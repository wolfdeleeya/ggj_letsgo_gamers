using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    Camera cam;
    Vector3 MyPos;
    Vector3 smjer;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = this.GetComponent<Camera>();
        MyPos = cam.transform.position;
        smjer = cam.transform.forward * 10;
        
    }
    // Update is called once per frame

    private void FixedUpdate()
    {
        Debug.DrawRay(cam.transform.position, smjer, Color.red);
    }

    public void calcPoint(Vector3 worldPosition)
    {
        
    }
}
