using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject Door;

    Door targetDoor;

    [SerializeField]
    bool amIALargeRoom;

    [SerializeField]
    GameObject roomCenter;

    [SerializeField]
    GameObject putPlayerHere;

    GameObject cameraFollowMe, player;

    FollowPlayer followPlayerScript;
    PlayerMovement playerMovementScript;

    private void Awake()
    {
        targetDoor = Door.GetComponent<Door>();       
    }

    private void Start()
    {
       cameraFollowMe = GameObject.FindWithTag("CamFollow");
       followPlayerScript = cameraFollowMe.GetComponent<FollowPlayer>();

       player = GameObject.FindWithTag("Player");
       playerMovementScript = player.GetComponent<PlayerMovement>();


        //List<GameObject> children = new List<GameObject>();

        //children.Add(thisGameObject.GetComponentsInChildren<GameObject>()[0]);

        //putPlayerHere = GameObject.Find("PutPlayerHere");
        //Debug.Log(thisGameObject.GetComponentInChildren<GameObject>().ToString());

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" && EnemyManager.Instance.isInCombat == false) //
        {
            
            if (targetDoor.IsLocked == false)
            {
                //targetDoor.OpenDoor();
                //followPlayerScript.Move(roomCenter);
                if (!followPlayerScript.safetyBuffer)
                {
                    followPlayerScript.safetyBuffer = true;
                }
                playerMovementScript.StartDoorTransition(putPlayerHere.transform.position);
                followPlayerScript.doIFollow = true;
                //Debug.Log("Door Opened");
            }
            else
            {
                PlayerStats ps = other.GetComponent<PlayerStats>();

                if (ps.keys >= 1)
                {
                    ps.keys--;
                    targetDoor.UnlockDoor();
                    /*
                     * play really cool animation here
                     */
                    this.OnTriggerEnter(other);
                }
            }
        }     
    }

    void OnTriggerExit(Collider other)
    {
        if (!followPlayerScript.safetyBuffer)
        {
            if (other.transform.tag == "Player")
            {
                //targetDoor.CloseDoor();
                //followPlayerScript.Move(roomCenter);
                if(amIALargeRoom)
                {
                    followPlayerScript.doIFollow = true;
                }
                else
                {
                    Debug.Log("No more following the camera followme object m oves to center");
                    followPlayerScript.doIFollow = false;

                    followPlayerScript.Move(roomCenter);
                }
                //Debug.Log("Door Closed");

            }

        }
        else
        {
            followPlayerScript.safetyBuffer = !followPlayerScript.safetyBuffer;
        }
    }
}
