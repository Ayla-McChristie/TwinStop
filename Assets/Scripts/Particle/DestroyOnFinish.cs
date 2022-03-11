using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnFinish : MonoBehaviour
{
    AudioSource audioSource;
    ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        //audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (ps.isEmitting == false)
        {
            Destroy(this.gameObject, ps.main.duration);
        }
    }
}
