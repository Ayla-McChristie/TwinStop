using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField]
    Vector3 rotationSpeed;
    [SerializeField]
    bool isTimeBased;
    AudioSource wallMove;
    TimeManager time;
    private void Start()
    {
        wallMove = GetComponent<AudioSource>();
        if (wallMove != null && !wallMove.Equals(null))
        {
            wallMove.Play();
            wallMove.pitch = 2f;
        }
        time = GameObject.Find("Player_2.0").GetComponentInChildren<TimeManager>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (isTimeBased)
        {
            this.transform.Rotate(rotationSpeed * Time.timeScale, Space.Self);
        }
        else
        {
            this.transform.Rotate(rotationSpeed, Space.Self);
        }
        if (wallMove != null && !wallMove.Equals(null))
        {
            if (time.isTimeStopped)
                wallMove.pitch = 0.5f;
            else
                wallMove.pitch = 2f;
        }
    }
}
