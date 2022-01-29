using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneController : MonoBehaviour
{
    public Animation animPhone;

    public void PhoneActivate()
    {
        animPhone.Play("PhoneComeClose");
        
    }

    public void PhoneDeactivate()
    {
        animPhone.Play("PhoneGetBack");
    }
}
