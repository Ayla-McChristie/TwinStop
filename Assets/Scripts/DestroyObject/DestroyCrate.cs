using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCrate : MonoBehaviour
{
    AudioSource crateBreak;
    AudioClip clip;
    bool isBroken;

    float clipLength;
    float clipTimer;
    // Start is called before the first frame update
    void Start()
    {
        crateBreak = GetComponent<AudioSource>();
        clip = GetComponent<AudioSource>().clip;
        isBroken = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(isBroken);
        if (isBroken)
        {
            crateBreak.Play();
            clipLength = clip.length;
            //if (clipTimer >= clipLength)
            //    Destroy(this);
            //else
            //    clipTimer += Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
            isBroken = true;
    }
}
