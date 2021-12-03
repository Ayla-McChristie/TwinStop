using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryTurret_Hand : MonoBehaviour
{
    public GameObject cannon;
    bool detectPlayer;
    // Start is called before the first frame update
    void Start()
    {
        detectPlayer = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (detectPlayer && cannon.active)
            this.transform.Rotate(0, 200 *Time.deltaTime,0);
    }
}
