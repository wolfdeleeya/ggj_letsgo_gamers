using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AngerBarVisualController : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    
    public void OnAngerChanged(float newAnger) =>_slider.value = newAnger / 100;
    }