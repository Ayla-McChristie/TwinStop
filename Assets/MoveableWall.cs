using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableWall : MonoBehaviour
{
    public Animator SetAnim;
    public GameObject Time;
    public bool TimeCheck;

    // Start is called before the first frame update
    void Start()
    {
        Time = GameObject.Find("TimeManager");
        SetAnim = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        TimeCheck = Time.GetComponent<TimeManager>().hasTimeCrystal;
        if(TimeCheck == true)
        {
            SetAnim.enabled = true;
        }
    }
}
