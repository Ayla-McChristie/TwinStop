using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody playerRigidbody;

    //i think this is technical debt but im not sure at the moment
    private PlayerActionControls playerActionControls;

    private Vector3 moveInput;
    private Vector3 moveVelocity;

    private  Vector3 currentMoveToTarget;

    private Camera mainCamera;
    AudioSource playerFootStep;
    private GunControl gunControlScript;

    //Turns true when special scenes happen like a door transition
    public bool freezeMovement;

    Animator anim;

    private void Awake()
    {
        playerActionControls = new PlayerActionControls();
        
    }

    private void OnEnable()
    {
        playerActionControls.Enable();
    }

    private void OnDisable()
    {
        playerActionControls.Disable();
    }

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
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
        if(!freezeMovement && !GetComponent<PlayerStats>().isDead)
        {
            var moveInput = playerActionControls.Player.Move.ReadValue<Vector2>();
            Vector3 flattenedMoveInput = new Vector3(moveInput.x, 0, moveInput.y);
            moveVelocity = (flattenedMoveInput.normalized * moveSpeed)/Time.timeScale;

            float velocityX = Vector3.Dot(flattenedMoveInput.normalized, transform.right);
            float velocityZ = Vector3.Dot(flattenedMoveInput.normalized, transform.forward);

            anim.SetFloat("VelocityX", velocityX, 0.1f, Time.deltaTime);
            anim.SetFloat("VelocityZ", velocityZ, 0.1f, Time.deltaTime);
        }
        else if (GetComponent<PlayerStats>().isDead)
        {
            gunControlScript.FrezeFire();
            moveVelocity = Vector3.zero;
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
            if (freezeMovement)
            {
                currentMoveToTarget = this.transform.position;
                UnFreeze();
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputVelocity = context.ReadValue<Vector2>();
        inputVelocity.Normalize();
        moveVelocity = (new Vector3(inputVelocity.x, 0, inputVelocity.y) * moveSpeed) / Time.timeScale;
    }
}
