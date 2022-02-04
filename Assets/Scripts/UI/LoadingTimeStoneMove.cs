using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingTimeStoneMove : MonoBehaviour
{
    public float yMoveDist = 0.1f;
    public float speed = 1f;

    // Position Storage Variables
    Vector3 posOG = new Vector3();
    Vector3 tempPos = new Vector3();

    private void Start()
    {
        posOG = this.transform.position;
    }
    void Update()
    {
        transform.RotateAround(transform.position, transform.up, Time.deltaTime * 90f);

        tempPos = posOG;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * speed) * yMoveDist;

        transform.position = tempPos;
    }
}
