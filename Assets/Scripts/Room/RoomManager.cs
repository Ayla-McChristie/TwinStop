using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private static RoomManager _instance;
    public static RoomManager Instance { get { return _instance; } }

    public RoomTrigger CurrentRoom { get; private set; }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void SetCurrentRoom(RoomTrigger triggeredRoom)
    {
        this.CurrentRoom = triggeredRoom;
    }

    public static void CreateRoomManager()
    {
        GameObject roomManagerGO = new GameObject("RoomManager");
        roomManagerGO.AddComponent<RoomManager>();
        RoomManager._instance = roomManagerGO.GetComponent<RoomManager>();
    }
}
