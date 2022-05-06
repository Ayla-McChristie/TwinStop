using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeTimeSlow : MonoBehaviour
{
    AudioSource themeSound;
    TimeManager tm;
    void Start()
    {
        //themeSound = GetComponent<AudioSource>();
        themeSound.pitch = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeManager.Instance != null)
        {
            if (TimeManager.Instance.isTimeStopped)
            {
                themeSound.pitch = .5f;
            }
            else
            {
                themeSound.pitch = 1;
            }
        }
    }
}
