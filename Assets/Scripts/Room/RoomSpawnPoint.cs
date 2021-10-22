using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawnPoint : MonoBehaviour
{
    [SerializeField]
    public List<WaveListWrapper> listOfWaves;

    //this is because unity doesnt serialized nested lists without it
    [System.Serializable]
    public class WaveListWrapper
    {
        public List<GameObject> WaveList;
    }

    private void Start()
    {
        //List<WaveListWrapper> listOfWaves = new List<WaveListWrapper>();
    }
}
