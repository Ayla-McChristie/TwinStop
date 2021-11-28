using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessingController : MonoBehaviour
{
    [SerializeField]
    Volume TimeStopPostProcessingScript;
    [SerializeField]
    Volume hurtVignette;
    public bool timeStopOn;

    // Start is called before the first frame update
    void Start()
    {
        timeStopOn = false;
        if (hurtVignette != null)
        {
            hurtVignette.weight = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timeStopOn)
        {
            FadeIn(TimeStopPostProcessingScript);
        }
        if (!timeStopOn)
        {
            FadeOut(TimeStopPostProcessingScript);
        }

        if (hurtVignette.weight > 0)
        {
            hurtVignette.weight -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Fades in the passed in post processing effect
    /// </summary>
    /// <param name="ppToFadeIn"></param>
    void FadeIn(Volume ppToFadeIn)
    {
        if(ppToFadeIn.weight < 1)
        {
            ppToFadeIn.weight += .01f;
        }
    }

    /// <summary>
    /// Fades out the passed in post processing effect
    /// </summary>
    /// <param name="ppToFadeIn"></param>
    void FadeOut(Volume ppToFadeOut)
    {
        if (ppToFadeOut.weight > 0)
        {
            ppToFadeOut.weight -= .01f;
        }
    }

    /// <summary>
    /// Sets the weight of target post processing
    /// </summary>
    /// <param name="ppToChangeWeight"></param>
    /// <param name="targetWeight"></param>
    void SetWeight(Volume ppToChangeWeight, float targetWeight)
    {
        ppToChangeWeight.weight = targetWeight;
    }

    //technical debt
    public void ActivateHurtVignette()
    {
        SetWeight(hurtVignette, 1f);
    }
}
