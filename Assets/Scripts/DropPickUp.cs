using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPickUp : MonoBehaviour
{
    [SerializeField] private GameObject GameObject;

    AudioSource crateBreak;
    AudioClip clip;
    bool isBroken;

    float clipLength;
    float clipTimer;

    void Start()
    {
        crateBreak = GetComponent<AudioSource>();
        clip = GetComponent<AudioSource>().clip;
        isBroken = false;
    }

    void Update()
    {
        if (isBroken)
        {
            
            clipLength = clip.length;
            if (clipTimer >= clipLength)
                Destroy(this.gameObject);
            else
                clipTimer += Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.tag == "PlayerBullet")
        {
            isBroken = true;
            crateBreak.Play();
            if (GameObject != null)
                Instantiate(GameObject, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
            this.gameObject.GetComponent<BoxCollider>().enabled = !this.gameObject.GetComponent<BoxCollider>().enabled;
        }
    }

}
