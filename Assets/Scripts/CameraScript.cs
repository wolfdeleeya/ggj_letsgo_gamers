using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Events;

public class CameraScript : MonoBehaviour
{
    public Animation camAnim;
    [SerializeField]
    int phoneMask;
    Camera _cam;
    Transform _transform;
    public PostProcessVolume volume;
    DepthOfField depthOfField;
    float hitDistance = 5;
    public Transform phone;
    public UnityEvent onPhoneFocus;
    public UnityEvent onPhoneDefocus;
    bool PhoneActive;
    // Start is called before the first frame update
    void Start()
    {
       _cam = this.GetComponent<Camera>();
        _transform = this.transform;
        volume.profile.TryGetSettings(out depthOfField);
        PhoneActive = false;
    }
    // Update is called once per frame

    private void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector3 worldPosition = _cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, _cam.nearClipPlane));
        Vector3 direction = (worldPosition - _cam.transform.position).normalized * 100;

        Vector3 smjer = (phone.position - _transform.position);
        Debug.DrawRay(_transform.position, direction.normalized * 10f, Color.red);

        

         if (PhoneActive)
        {
            depthOfField.active = true;
            
            hitDistance = smjer.magnitude;
            setFocus();
        }
        else
        {
            depthOfField.active = false;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(_transform.position, direction.normalized * 10f, out RaycastHit hit1))
            {
                if (hit1.collider.gameObject.name.Equals("Phone"))
                {
                    Debug.Log("hit");
                    if (PhoneActive)
                    {
                        onPhoneDefocus.Invoke();
                        camAnim.Play("CameraDefocus");


                    }
                    else
                    {
                        onPhoneFocus.Invoke();
                        camAnim.Play("CameraFocus");

                    }
                    PhoneActive = !PhoneActive;
                }
               
               
            }
           
        }
        
    }

    private void setFocus()
    {
        depthOfField.focusDistance.value = hitDistance;
        
    }



}
