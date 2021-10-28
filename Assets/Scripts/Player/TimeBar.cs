using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxTime(float time)
    {
        slider.maxValue = time; // sets the max value of the slider
        slider.value = time; // current amount of time available
    }

    public void TimeSet(float time)
    {
        slider.value = time;
    }
}
