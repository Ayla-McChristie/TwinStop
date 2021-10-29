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
    GameObject cameraFollowMe, player, putPlayerHere, thisGameObject;

    [SerializeField]
    FollowPlayer followPlayerScript;

    [SerializeField]
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

       thisGameObject = this.gameObject;

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
                targetDoor.OpenDoor();
                Debug.Log(putPlayerHere.transform.position);
                playerMovementScript.StartDoorTransition(putPlayerHere.transform.position);
                

                //Debug.Log("Door Opened");
            }
            else
            {
                PlayerStats ps = other.GetComponent<PlayerStats>();

                if (ps.keys >= 1)
                {
                    ps.keys--;
                    targetDoor.IsLocked = false;
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
        if (other.transform.tag == "Player")
        {
            targetDoor.CloseDoor();
            followPlayerScript.Move(roomCenter);
            if(amIALargeRoom)
            {
                followPlayerScript.doIFollow = !followPlayerScript.doIFollow;
            }
            //Debug.Log("Door Closed");

        }
    }
}
