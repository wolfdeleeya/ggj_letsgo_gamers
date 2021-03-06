using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarController : MonoBehaviour
{
    public UnityEventFloat OnAngleValueChanged;

    [SerializeField] bool isOverBarrier;
    float side;
    [SerializeField] LayerMask wheelMask;
    [SerializeField] AnimationCurve _animateServo;
    Rigidbody rb;
    public CameraScript cam;
    Camera acamera;
    public Transform wheelTrans;
    Transform trans;
    public float forceForward = 10;
    public float smoothTurn = 60.0f;
    Vector2 centerWheelScreen;
    Vector3 vecStart;
    float realAngle;
    public float servoSpeed;
    public float maxAngle;
    [SerializeField] private float turnScale;
    [SerializeField] private float _animationDuration;
    [SerializeField] private float _maxScale;
    [SerializeField] private float _minScale;
    bool isPhoneView;
    bool wheelHold;
    private bool isCollided;

    public static CarController Instance;

    public void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        trans = transform;
        acamera = this.GetComponentInChildren<Camera>();
        isPhoneView = false;
        wheelHold = false;
        isOverBarrier = false;
        side = 0;
    }

    private void Update()
    {
        //find out center of Wheel in screen width and length
        centerWheelScreen = acamera.WorldToScreenPoint(wheelTrans.position);

        //when click mouse
        if (Input.GetMouseButtonDown(0) && !isPhoneView)
        {
            Vector2 mousePos = Input.mousePosition;
            Vector3 worldPosition =
                acamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, acamera.nearClipPlane));
            Vector3 smjer = (worldPosition - cam.transform.position).normalized * 100;

            //if user clicked on steering wheel
            if (Physics.Raycast(cam.transform.position, smjer, out RaycastHit hit1))
            {
                if (hit1.collider.gameObject.name.Equals("steering_wheel"))
                {
                    vecStart = (mousePos - centerWheelScreen).normalized;
                }
                //remember vector position from center to the point the user clicked no steering wheel
            }
            else
            {
                vecStart = Vector3.zero;
            }
        }

        //if user holding the mouse button on steering wheel
        //realAngle: true angle from wheel origin position to current position of wheel
        if (Input.GetMouseButton(0) && vecStart.magnitude != 0 && !isPhoneView)
        {
            wheelHold = true;

            //calculate vector direction from the camera position to the mouse hold position
            Vector2 mousePos = Input.mousePosition;
            Vector3 worldPosition =
                acamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, acamera.nearClipPlane));
            Vector3 smjer = (worldPosition - cam.transform.position).normalized * 100;

            //if this direction is passing throgh the wheel box collider, user is holding the wheel 
            if (Physics.Raycast(cam.transform.position, smjer, out RaycastHit hit1))
            {
                if (hit1.collider.gameObject.name.Equals("steering_wheel"))
                {
                    Debug.Log("wheelCast" + hit1.collider.gameObject);
                    Vector3 vec = (mousePos - centerWheelScreen).normalized;

                    float cross = Mathf.Sign(Vector3.Cross(vecStart, vec).z);
                    float angle = Vector3.Angle(vecStart, vec);
                    if (Mathf.Abs(realAngle) > maxAngle && Mathf.Abs(realAngle - cross) > Mathf.Abs(realAngle))
                    {
                    }
                    else
                    {
                        wheelTrans.RotateAround(wheelTrans.position, wheelTrans.up, angle * -cross);
                        vecStart = vec;
                        //turn Angle

                        realAngle += angle * -cross;
                        OnAngleValueChanged.Invoke(Mathf.Abs(realAngle) / maxAngle);

                        Quaternion target = Quaternion.Euler(0, realAngle * turnScale, 0);


                        trans.rotation = Quaternion.Slerp(trans.rotation, target, Time.deltaTime * smoothTurn);
                    }
                }
            }
            else
            {
                wheelHold = false;
            }
        }
        else
        {
            ActivateServo();
        }
    }

    private void FixedUpdate()
    {
        if (isCollided)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            return;
        }

        //forward
        Vector3 vec = this.transform.forward.normalized * forceForward;
        Debug.Log(vec);
        if (isOverBarrier && vec.x * side > 1)
        {
            rb.velocity = new Vector3(0, vec.y, vec.z);
        }
        else
        {
            rb.velocity = vec;
        }
    }

    private IEnumerator BumpAnimation()
    {
        float t = 0;
        while (t < _animationDuration)
        {
            yield return null;
            float sign = Mathf.Sign(trans.rotation.y);
            t += Time.deltaTime;

            // Dampen towards the target rotation
            float angle = Mathf.Lerp(_minScale, _maxScale, _animateServo.Evaluate(t / _animationDuration));
            trans.rotation = Quaternion.Euler(0, angle * sign, 0);
        }

        trans.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void switchView()
    {
        isPhoneView = !isPhoneView;
    }

    private void ActivateServo()
    {
        if (Mathf.Abs(realAngle) > 5)
        {
            reverseRotateWheel();

            realAngle += servoSpeed * -Mathf.Sign(realAngle);
            OnAngleValueChanged.Invoke(Mathf.Abs(realAngle) / maxAngle);

            Quaternion target = Quaternion.Euler(0, realAngle * turnScale, 0);

            trans.rotation = Quaternion.Slerp(trans.rotation, target, Time.deltaTime * smoothTurn);
        }
        else if (Mathf.Abs(realAngle) != 0)
        {
            reverseRotateWheel();
            realAngle = 0;

            StartCoroutine(BumpAnimation());
        }
    }

    private void reverseRotateWheel()
    {
        wheelTrans.RotateAround(wheelTrans.position, wheelTrans.up, servoSpeed * -Mathf.Sign(realAngle));
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ziiid!!");
        isOverBarrier = true;
        if (other.CompareTag("WallLeft"))
        {
            side = -1;
            isOverBarrier = true;
        }
        else if (other.CompareTag("WallRight"))
        {
            side = 1;
            isOverBarrier = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.GetComponent<OtherCarController>())
        {
            GameManager.Instance.GameOver(true);
            isCollided = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        isOverBarrier = true;
        if (other.CompareTag("WallLeft"))
        {
            side = -1;
            isOverBarrier = true;
        }
        else if (other.CompareTag("WallRight"))
        {
            side = 1;
            isOverBarrier = true;
        }
    }

    public void OnGameFinished(bool dummyVar)
    {
        forceForward = 0;
        rb.detectCollisions = false;
    }

    private void OnTriggerExit(Collider other)
    {
        isOverBarrier = false;
    }
}