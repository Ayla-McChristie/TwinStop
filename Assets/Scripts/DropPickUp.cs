using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPickUp : MonoBehaviour
{
    [SerializeField] private GameObject GameObject;

    void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.tag == "PlayerBullet")
        {
            Instantiate(GameObject, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

}
