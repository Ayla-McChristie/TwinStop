using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessingController : MonoBehaviour
{
    [SerializeField]
    Volume TimeStopPostProcessingScript;
    public bool timeStopFadingIn, timeStopFadingOut;

    // Start is called before the first frame update
    void Start()
    {
        timeStopFadingIn = timeStopFadingOut = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeStopFadingIn)
        {
            FadeIn(TimeStopPostProcessingScript, timeStopFadingOut);
        }
        if (timeStopFadingOut)
        {
            FadeOut(TimeStopPostProcessingScript, timeStopFadingOut);
        }

    }

    /// <summary>
    /// Fades in the passed in post processing effect
    /// </summary>
    /// <param name="ppToFadeIn"></param>
    void FadeIn(Volume ppToFadeIn, bool fadeInBool)
    {
        if(ppToFadeIn.weight < 1)
        {
            //ppToFadeIn.weight += .01f;
        }
        else
        {
            fadeInBool = false;
        }
    }

    /// <summary>
    /// Fades out the passed in post processing effect
    /// </summary>
    /// <param name="ppToFadeIn"></param>
    void FadeOut(Volume ppToFadeOut, bool fadeOutBool)
    {
        if (ppToFadeOut.weight > 0)
        {
            //ppToFadeOut.weight -= .01f;
        }
        else
        {
            fadeOutBool = false;
        }
    }
}
