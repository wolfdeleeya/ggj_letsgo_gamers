using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraScript : MonoBehaviour
{
    Camera _cam;
    Transform _transform;
    public PostProcessVolume volume;
    DepthOfField depthOfField;
    float hitDistance = 5;
    public Transform phone;
    
    // Start is called before the first frame update
    void Start()
    {
       _cam = this.GetComponent<Camera>();
        _transform = this.transform;
        volume.profile.TryGetSettings(out depthOfField);
        
    }
    // Update is called once per frame

    private void Update()
    {
        /*if(Physics.Raycast(_transform.position,transform.forward,out RaycastHit hit, 100f))
        {
            hitDistance = hit.distance;
        }*/
        hitDistance = (phone.position - _cam.transform.position).magnitude;

        setFocus();
    }

    private void setFocus()
    {
        depthOfField.focusDistance.value = hitDistance;
    }

}
