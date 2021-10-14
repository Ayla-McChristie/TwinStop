using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool IsOpen { get; set; }
    public bool IsLocked { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        IsOpen = false;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public void OpenDoor()
    {
        this.IsOpen = true;
    }

    public void CloseDoor()
    {
        this.IsOpen = false;
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
