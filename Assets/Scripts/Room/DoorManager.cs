using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    /*
     * This is all for sigleton pattern. I only want 1 door manager and i want all the doors to check into said door manager -A
     */
    private static DoorManager _instance;
    public static DoorManager Instance { get { return _instance; } }

    /*
     * here is where the non singleton stuff starts -A
     */

    List<Door> doors;

    private void Awake()
    {
        doors = new List<Door>();
    }

    private void Update()
    {
        ShutAllDoorsInCombat();
    }

    void ShutAllDoorsInCombat()
    {
        if (enemymanager.isInCombat == true)
        {
            foreach (Door door in doors)
            {
                DoorTrigger dt = door.GetComponentInChildren<DoorTrigger>();
                dt.enabled = false;
            }
        }
    }




    /*
     * Singleton methods -A
     */
    public static void CreateDoorManager()
    {
        DoorManager dm = new DoorManager();
        _instance = dm;
    }
}
