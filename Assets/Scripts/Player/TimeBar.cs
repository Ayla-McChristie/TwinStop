using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
    float timeBarMax;
    float timeBarValue;
    Material material;

    private void Start()
    {
        material = this.GetComponent<Renderer>().material;
    }

    public void SetMaxTime(float time)
    {
        timeBarMax = time;
        timeBarValue = time;
        //slider.maxValue = time; // sets the max value of the slider
        //slider.value = time; // current amount of time available
    }

    public void TimeSet(float time)
    {
        timeBarValue = time;
        float temp = Mathf.Lerp(-0.5f, 5.84f, Mathf.InverseLerp(0, timeBarMax, timeBarValue));
        if (material != null)
        {
            material.SetFloat("CuttOffHeight", temp);
        }
    }
}
