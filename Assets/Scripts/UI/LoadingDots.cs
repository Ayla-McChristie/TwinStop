using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoadingDots : MonoBehaviour
{
    [SerializeField]
    Text text;

    float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer <= .5f)
            text.text = ".";
        else if (timer <= 1)
            text.text = ". .";
        else
            text.text = ". . .";

        if (timer >= 2f)
            timer = 0;
    }

}
