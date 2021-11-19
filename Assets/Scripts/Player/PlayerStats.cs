using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        set => health = value;
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
    //public Image[] hearts; // all heart UI game objects go here
    //public Sprite fullHeart; // sprite of full heart here
    //public Sprite emptyHeart; // sprite of empty heart here

    /*
     * HitFlash Variables
     */
    public SkinnedMeshRenderer FlashRenderer { get; set; }
    public float flashIntensity = 50;
    public float FlashIntensity
    {
        get => flashIntensity;
        set => flashIntensity = value;
    }
    public float flashDuration = 1;
    public float FlashDuration
    {
        get => flashDuration;
        set => flashDuration = value;
    }
    public float FlashTimer { get; set; }

    public bool isDead = false;
    private void Start()
    {
        FlashRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }
    void Update()
    {
        UpdateHearts();
        FlashCoolDown();
        InvincibilityCoolDown();
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
        float intesity = (lerp * FlashIntensity) + 1.0f;
        FlashRenderer.material.color = Color.white * intesity;   
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
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyBullet")
        {
            if (!isInvincible)
            {
                TakeDamage();
            }
        }

        // Coded this before I know one was already made for keys. Mine works and is tested -Steven
        // Code for Health Pick Up
        if (other.gameObject.tag == "HealthPickUp")
        {
            Health++;
            //Destroy(other.gameObject);
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
        }
        if (Health <= 0)
        {
            PlayerDead();
        }
    }
    void UpdateHearts()
    {
        // Health Lock
        if (Health > numOfHearts)
        {
            Health = numOfHearts; // this makes sure that players can never go over the set amount of hearts

        }

        // system for turning full hearts to empty hearts
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < Health)
            {
                hearts[i].SetActive(true);
            }
            else
            {
                hearts[i].SetActive(false);
            }

            if (i < numOfHearts)
            {
                //hearts[i].SetActive(true);
            }
            else
            {
                hearts[i].SetActive(false);
            }

            //if (i < Health)
            //{
            //    hearts[i].sprite = fullHeart;
            //}
            //else
            //{
            //    hearts[i].sprite = emptyHeart;
            //}

            //if (i < numOfHearts)
            //{
            //    hearts[i].enabled = true;
            //}
            //else
            //{
            //    hearts[i].enabled = false;
            //}
        }
    }

    void PlayerDead()
    {
        isDead = true;
    }
}