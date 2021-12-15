using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerWeakpoint : MonoBehaviour
{
    AudioSource damage;
    private void Start()
    {
        damage = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (TimeManager.Instance.isTimeStopped)
            damage.pitch = .5f;
        damage.pitch = 1;
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.gameObject.transform.tag == "PlayerBullet")
    //    {
    //        this.gameObject.GetComponentInParent<Charger>().DamageTaken(1);
    //        damage.Play();
    //    }
    //}
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "PlayerBullet")
        {
            this.gameObject.GetComponentInParent<Charger>().DamageTaken(1);
            damage.PlayOneShot(damage.clip);
        }
    }
}
