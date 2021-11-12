using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessingController : MonoBehaviour
{
    [SerializeField]
    Volume TimeStopPostProcessingScript;
    public bool timeStopOn;

    // Start is called before the first frame update
    void Start()
    {
        timeStopOn = false;
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
}
