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

    [SerializeField]
    public EnemyManager enemyManager;
    /*
     * here is where the non singleton stuff starts -A
     */

    public List<Door> doors;

    private void Awake()
    {
        doors = new List<Door>();
    }
    /*
     * Singleton methods -A
     */
    public static void CreateDoorManager()
    {
        GameObject dmGameObject = new GameObject("DoorManager");
        dmGameObject.AddComponent<DoorManager>();
        DoorManager._instance = dmGameObject.GetComponent<DoorManager>();
        _instance.enemyManager = (EnemyManager)GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>();
    }
}
