using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRingFollow : MonoBehaviour
{
    GameObject player;
    RectTransform rTransform;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position.x = 1; 

    }
}
