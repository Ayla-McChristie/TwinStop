using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField]
    Vector3 rotationSpeed;

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.Rotate(rotationSpeed);
    }
}
