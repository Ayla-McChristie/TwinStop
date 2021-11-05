using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody playerRigidbody;

    private Vector3 moveInput;
    private Vector3 moveVelocity;

    private  Vector3 currentMoveToTarget;

    private Camera mainCamera;
    AudioSource playerFootStep;
    private GunControl gunControlScript;

    //Turns true when special scenes happen like a door transition
    private bool freezeMovement;

    Animator anim;

    private void Start()
    {
        this.playerRigidbody = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        anim = GetComponent<Animator>();
        freezeMovement = false;
        playerFootStep = GetComponent<AudioSource>();
        gunControlScript = this.GetComponent<GunControl>();

        //currentMoveToTarget = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(!freezeMovement)
        {
            moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            moveVelocity = (moveInput.normalized * moveSpeed)/Time.timeScale;

            float velocityX = Vector3.Dot(moveInput.normalized, transform.right);
            float velocityZ = Vector3.Dot(moveInput.normalized, transform.forward);

            anim.SetFloat("VelocityX", velocityX, 0.1f, Time.deltaTime);
            anim.SetFloat("VelocityZ", velocityZ, 0.1f, Time.deltaTime);
        }
        else
        {
            MovePlayerToTarget();
        }
        
        
        //this.playerRigidbody.MovePosition(playerRigidbody.position + (moveVelocity * Time.unscaledDeltaTime));

        //Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        //Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        //float rayLength; // Length of line from Camera to nearest ground

        //if(groundPlane.Raycast(cameraRay, out rayLength))
        //{
        //    Vector3 pointToLook = cameraRay.GetPoint(rayLength);
        //    Debug.DrawLine(cameraRay.origin, pointToLook, Color.green);

        //    transform.LookAt(pointToLook);
        //}
    }

    void FixedUpdate()
    {
        playerRigidbody.velocity = moveVelocity;
    }

    /// <summary>
    /// Called by door triggers to move the player to the correct spot when they enter a door.
    /// </summary>
    /// <param name="moveTo"></param>
    public void StartDoorTransition(Vector3 moveTo)
    {
        if (!freezeMovement)
        {
            Freeze();
            currentMoveToTarget = moveTo;
        }
       
    }

    /// <summary>
    /// The method used for moving a player to a certain point (without player input)
    /// </summary>
    void MovePlayerToTarget()
    {
        if (this.transform.position != currentMoveToTarget)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, currentMoveToTarget, 9 * Time.deltaTime);
        }
        else
        {
            //currentMoveToTarget = this.transform.position;
            UnFreeze();
        }
    }

    /// <summary>
    ///  Freezes the player
    /// </summary>
    void Freeze()
    {
        gunControlScript.FrezeFire();
        freezeMovement = true;
    }

    /// <summary>
    ///  UnFreezes the player
    /// </summary>
    void UnFreeze()
    {
        gunControlScript.UnFrezeFire();
        freezeMovement = false;
    }

    public void Step()
    {
        playerFootStep.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("TransitionTrigger"))
        {
            Debug.Log("Duh?");
            if (freezeMovement)
            {
                currentMoveToTarget = this.transform.position;
                UnFreeze();

            }
        }
    }
}
