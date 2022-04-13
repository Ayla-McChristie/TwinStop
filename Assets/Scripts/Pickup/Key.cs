using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    enum IKeyType {RoomKey, BossKey}

    [SerializeField]
    IKeyType keyType;

    AudioSource audio;
    AudioClip clip;

    bool isPickedUp;

    float clipLength;
    float clipTimer;
    private void Start()
    {
        isPickedUp = false;
        audio = GetComponent<AudioSource>();
        clip = audio.clip;
    }

    void Update()
    {
        if (isPickedUp)
        {
            if (clipTimer >= clipLength)
                this.gameObject.SetActive(false);
            else
                clipTimer += Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Player" )
        {
            if(audio != null)
            {
                AudioManager.Instance.PlaySound("KeyPickUp", this.transform.position, true);
            }
            isPickedUp = true;
            clipLength = clip.length;

            /*
             * technical debt. this should use the object pool system but this will work for now -A
             */
            //Destroy(this.gameObject);
        }
    }
}
