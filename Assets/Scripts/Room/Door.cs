using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Renderer))]
public class Door : MonoBehaviour
{
    [SerializeField]
    public bool isBossDoor = false;
    [SerializeField]
    public bool IsLocked = false;
    public bool IsOpen { get; set; }

    Collider doorCollider;
    Renderer renderer;

    [SerializeField]
    CinemachineVirtualCamera cam;

    [SerializeField]
    GameObject roomCenter;
    
    
    void Start()
    {
        this.doorCollider = this.gameObject.GetComponent<Collider>();
        this.renderer = this.gameObject.GetComponent<Renderer>();
        IsOpen = false;

        //cam = GameObject.Find("2Dcam");
    }
    /*
     * Used to open the door. turns off the renderer and collider but still allows it to update if need be
     */
    public void OpenDoor()
    {
        if (this.IsLocked == false)
        {
            this.IsOpen = true;
            this.renderer.enabled = false;
            this.doorCollider.enabled = false;
        }
    }
    /*
     * Closes the door. doesnt check if the door is locked because open doors are by definition unlocked
     */
    public void CloseDoor()
    {
        this.IsOpen = false;
        this.renderer.enabled = true;
        this.doorCollider.enabled = true;
    }
    /*
     * Unlocks the door.
     */
    public void UnlockDoor()
    {
        this.IsLocked = false;
    }
    /*
     * Locks the door. Locked doors can only be opened if the player has keys
     */
    public void LockDoor()
    {
        this.IsLocked = true;
    }

    /// <summary>
    /// Pans the camera to the next room by setting the 'focuson' and 'lookat' values
    /// to the RoomCenter game object of the corresponding room
    /// </summary>
    public void MoveCamera()
    {
        cam.Follow = roomCenter.transform;
        cam.LookAt = roomCenter.transform;
    }
}
