﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerMovement))]
class PlayerStats : MonoBehaviour, IDamageFlash
{
    /*
     * Kill Variables
     */
    private static int numOfKilledEnemies;
    public static int NumOfKilledEnemies { get; private set; }

    /*
     * Health Variables
     */
    public float health = 6;
    // Total amount of health left
    public float Health
    {
        get => health;
        private set => health = value;
    }
    public int numOfHearts; // Max amount of hearts a player can have, should be 6

    /*
     * Invinciblity Variables
     */
    private bool isInvincible;
    private float invincibilityTimer;
    public float invicibilityDuration;

    
    /*
     * Key Variables
     */
    public int keys; //number of keys the player has -A
    public int bossKeys; //number of bossKeys the player has -A

    /*
     * UI Variables
     */
    public GameObject[] hearts;
    public GameObject[] keysList;
    int activeHeartInd;
    Quaternion ogHeartRotation;
    Vector3 heartSizeSmall;
    bool isSizeDown;
    //public Image[] hearts; // all heart UI game objects go here
    //public Sprite fullHeart; // sprite of full heart here
    //public Sprite emptyHeart; // sprite of empty heart here
    [SerializeField]
    AudioSource playerHit;
    [SerializeField]
    AudioSource playerDead;
    /*
     * HitFlash Variables
     */
    PostProcessingController ppController;
    public Renderer FlashRenderer { get; set; }
    public Material defaultMat;
    public Material hurtMat;
    public Material HurtMat { get => hurtMat; }
    public float flashDuration = 1;
    public float FlashDuration
    {
        get => flashDuration;
        set => flashDuration = value;
    }
    public float FlashTimer { get; set; }
    public float cShakeIntensity;
    public float cShakeTime;

    public bool isDead = false;
    private void Start()
    {
        FlashRenderer = GetComponentInChildren<Renderer>();
        ppController = GetComponent<PostProcessingController>();
        defaultMat = FlashRenderer.material;
        UpdateHearts();
        UpdateKeys();
        heartSizeSmall = hearts[0].transform.localScale;
    }
    void Update()
    {
        //UpdateHearts();
        FlashCoolDown();
        InvincibilityCoolDown();
        UpdateHearts();
        UpdateKeys();
        GetActiveHeart();
        RotateActiveHeart();
        //BeatActiveHeart();
        //Debug.Log(health);
    }
    void InvincibilityCoolDown()
    {
        if (isInvincible)
        {
            if (invincibilityTimer <= Time.time)
            {
                isInvincible = false;
            }
        }
    }
    void FlashCoolDown()
    {
        FlashTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(FlashTimer / FlashDuration);

        if (FlashTimer >= 0 && FlashRenderer.material != hurtMat)
        {
            FlashRenderer.material = HurtMat;
        }
        else if (FlashTimer <= 0 && FlashRenderer.material != defaultMat)
        {
            FlashRenderer.material = defaultMat;
        }
    }
    public static void ResetKillCount()
    {
        NumOfKilledEnemies = 0;
    }
    public static void AddToKillCount()
    {
        NumOfKilledEnemies++;
        //Debug.Log(NumOfKilledEnemies);
    }
    void OnCollisionEnter(Collision other)
    {
        if ((other.gameObject.name == "Gargoyle" || other.gameObject.name == "Test_Gargoyle") && !TimeManager.Instance.isTimeStopped)
            return;
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyBullet")
        {
            if (!isInvincible)
            {
                TakeDamage();
                AudioManager.Instance.PlaySound("PlayerHit", this.transform.position, true);
            }
        }

        // Coded this before I know one was already made for keys. Mine works and is tested -Steven
        // Code for Health Pick Up
        if (other.gameObject.tag == "HealthPickUp")
        {
            Health++;
            //hearts[activeHeartInd].transform.rotation = ogHeartRotation;
            hearts[activeHeartInd].GetComponent<Heart>().anim.SetBool("startBeat", false);
            hearts[activeHeartInd].transform.localScale = heartSizeSmall;
        }

        // ! Code for Key pickup
        if (other.gameObject.tag == "KeyPickUp")
        {
            keys++;
            //Destroy(other.gameObject);
            //Debug.Log("Key is now 1");
        }
    }
    public void TakeDamage()
    {
        TakeDamage(1);
    }
    public void TakeDamage(float damageAmount)
    {
        if (!isInvincible)
        {
            Health -= damageAmount;
            isInvincible = true;
            invincibilityTimer = Time.time + invicibilityDuration;
            FlashTimer = FlashDuration;
            ppController.ActivateHurtVignette();
            UpdateHearts();
            if (CameraShake.Instance != null)
            {
                CameraShake.Instance.ShakeCam(cShakeIntensity, cShakeTime);
            }
        }
        if (Health <= 0)
        {
            PlayerDead();
        }
    }

    public void Instakill()
    {
        Health = 0;
        UpdateHearts();
        PlayerDead();
    }

    void UpdateHearts()
    {
        // Health Lock
        if (Health > numOfHearts)
        {
            Health = numOfHearts; // this makes sure that players can never go over the set amount of hearts

        }

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive((i < health));
        }

    }

    void GetActiveHeart()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (!hearts[i].activeSelf)
            {
                activeHeartInd = i - 1;
                return;
            }
            if (i >= hearts.Length - 1)
                activeHeartInd = i;
        }
    }

    void BeatActiveHeart()
    {
        if (hearts[activeHeartInd].transform.localScale.magnitude <= heartSizeSmall.magnitude)
            isSizeDown = true;
        else if (hearts[activeHeartInd].transform.localScale.magnitude >= (heartSizeSmall + new Vector3(1000, 1000, 1000)).magnitude)
            isSizeDown = false;

        if (isSizeDown)
            hearts[activeHeartInd].transform.localScale += new Vector3(750f, 750f, 750f) * Time.deltaTime * 2;
        else
            hearts[activeHeartInd].transform.localScale -= new Vector3(750f, 750f, 750f) * Time.deltaTime * 2;
    }

    void RotateActiveHeart()
    {
        //hearts[activeHeartInd].transform.Rotate(new Vector3(0, 0, 75f) * Time.deltaTime);
        hearts[activeHeartInd].GetComponent<Heart>().anim.SetBool("startBeat", true);
    }

    void UpdateKeys()
    {
        if (keys > keysList.Length)
        {
            keys = keysList.Length; // this makes sure that players can never go over the set amount of hearts
        }

        for (int i = 0; i < keysList.Length; i++)
        {
            keysList[i].SetActive((i < keys));

        }
    }
    void PlayerDead()
    {
        isDead = true;
        AudioManager.Instance.PlaySound("PlayerDeath", this.transform.position, true);
    }
}