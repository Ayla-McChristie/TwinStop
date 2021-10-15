using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Renderer))]
public class Door : MonoBehaviour
{
    public bool IsOpen { get; set; }
    public bool IsLocked { get; set; }

    Collider doorCollider;
    Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        this.doorCollider = this.gameObject.GetComponent<Collider>();
        this.renderer = this.gameObject.GetComponent<Renderer>();
        IsOpen = false;
    }

    // Update is called once per frame
    public virtual void Update()
    {
    }

    public void OpenDoor()
    {
        if (this.IsLocked == false)
        {
            this.IsOpen = true;
            this.renderer.enabled = false;
            this.doorCollider.enabled = false;
        }
    }

    public void CloseDoor()
    {
        this.IsOpen = false;
        this.renderer.enabled = true;
        this.doorCollider.enabled = true;
    }

    public void UnlockDoor()
    {
        this.IsLocked = false;
    }

    public void LockDoor()
    {
        this.IsLocked = true;
    }
}
