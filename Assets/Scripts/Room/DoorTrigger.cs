using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject Door;

    Door targetDoor;

    private void Awake()
    {
        targetDoor = Door.GetComponent<Door>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" && targetDoor)
        {
            targetDoor.OpenDoor();
            Debug.Log("Door Opened");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player" && targetDoor)
        {
            targetDoor.CloseDoor();
            Debug.Log("Door Closed");
        }
    }
}
