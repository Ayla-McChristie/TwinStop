using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    AudioSource audio;
    public Animator anim;
    AudioClip clip;
    bool isPickedUp;
    public bool isUI;
    float clipLength;
    float clipTimer;

    PlayerStats PStatsScript;


    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        isPickedUp = false;
        clip = audio.clip;

        anim = GetComponent<Animator>();
        if (isUI)
            this.transform.localScale = new Vector3(4500f, 4500f, 4500f);
        else
            anim.enabled = false;

        PStatsScript = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
    }

    // Update is called once per frame
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
        if (other.transform.tag == "Player" && isPickedUp == false && PStatsScript.health < PStatsScript.numOfHearts)
        {
            AudioManager.Instance.PlaySound("HeartPickUp", this.transform.position, true);
            isPickedUp = true;
            clipLength = clip.length;
        }
    }
}
