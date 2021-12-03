using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField]
    Vector3 rotationSpeed;
    [SerializeField]
    bool isTimeBased;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isTimeBased)
        {
            this.transform.Rotate(rotationSpeed * Time.timeScale);
        }
        else
        {
            this.transform.Rotate(rotationSpeed);
        }
    }
}
