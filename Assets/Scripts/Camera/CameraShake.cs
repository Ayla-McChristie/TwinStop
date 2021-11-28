using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    CinemachineVirtualCamera vcam;
    float shakeTimer;
    float shakeTimerTotal;
    float startingIntesity;
    private void Awake()
    {
        Instance = this;
        vcam = GetComponent<CinemachineVirtualCamera>();     
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTimer>0)
        {
            shakeTimer -= Time.deltaTime;
            CinemachineBasicMultiChannelPerlin perlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (shakeTimer <= 0)
            {
                perlin.m_AmplitudeGain = 0f;
            }

            //perlin.m_AmplitudeGain = Mathf.Lerp(startingIntesity, 0f, (1 -(shakeTimer / shakeTimerTotal)));
        }
    }
    public void ShakeCam(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin perlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        perlin.m_AmplitudeGain = intensity;
        startingIntesity = intensity;

        shakeTimer = time;
        shakeTimerTotal = time;
    }
}
