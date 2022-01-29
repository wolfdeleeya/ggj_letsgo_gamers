using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField]
    LayerMask wheelMask;
    [SerializeField]
    AnimationCurve _animateServo;
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
    [SerializeField]
    private float _animationDuration;
    [SerializeField]
    private float _maxScale;
    [SerializeField]
    private float _minScale;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        trans = transform;
        acamera = this.GetComponentInChildren<Camera>();
        centerWheelScreen = acamera.WorldToScreenPoint(wheelTrans.position);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;
            Vector3 worldPosition = acamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, acamera.nearClipPlane));
            Vector3 smjer = (worldPosition - cam.transform.position).normalized * 100;
            //Debug.Log(worldPosition);
            if (Physics.Raycast(cam.transform.position, smjer, out RaycastHit hit1, wheelMask))
            {
                vecStart = (mousePos- centerWheelScreen).normalized;

            }
           
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Input.mousePosition;
            Vector3 worldPosition = acamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, acamera.nearClipPlane));
            Vector3 smjer = (worldPosition - cam.transform.position).normalized * 100;
            //Debug.Log(worldPosition);
            if (Physics.Raycast(cam.transform.position, smjer, out RaycastHit hit1, wheelMask))
            {
                Vector3 vec = (mousePos - centerWheelScreen).normalized;
                //Debug.Log(vec);
                float cross = Mathf.Sign(Vector3.Cross(vecStart, vec).z);
                float angle = Vector3.Angle(vecStart, vec);
                if (Mathf.Abs(realAngle) > maxAngle && Mathf.Abs(realAngle - cross) > Mathf.Abs(realAngle))
                {

                }
                else
                {
                    //Debug.Log(res);
                    wheelTrans.RotateAround(wheelTrans.position, wheelTrans.up, angle * -cross);
                    vecStart = vec;
                    //turn Angle

                    realAngle += angle * -cross;

                    Quaternion target = Quaternion.Euler(0, realAngle, 0);

                    // Dampen towards the target rotation
                    trans.rotation = Quaternion.Slerp(trans.rotation, target, Time.deltaTime * smoothTurn);
                }
            }


        }
        else
        {
            if (Mathf.Abs(realAngle) > 5)
            {

                wheelTrans.RotateAround(wheelTrans.position, wheelTrans.up,  servoSpeed * -Mathf.Sign(realAngle));
                realAngle += servoSpeed *- Mathf.Sign(realAngle);
                Quaternion target = Quaternion.Euler(0, realAngle, 0);

                // Dampen towards the target rotation
                trans.rotation = Quaternion.Slerp(trans.rotation, target, Time.deltaTime * smoothTurn);

            }
            else if(Mathf.Abs(realAngle) != 0)
            {
                wheelTrans.RotateAround(wheelTrans.position, wheelTrans.up, realAngle * -Mathf.Sign(realAngle));
                realAngle = 0;

                StartCoroutine(BumpAnimation());
                
            }
        }
    }

    private void FixedUpdate()
    {
        //forward

        rb.velocity = this.transform.forward.normalized * forceForward;


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
    }

}
