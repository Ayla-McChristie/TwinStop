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
    public int health = 3;
    // Total amount of health left
    public int Health
    {
        get => health;
        set => health = value;
    } 
    public int numOfHearts; // Max amount of hearts a player can have, should be 3

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
    public Image[] hearts; // all heart UI game objects go here
    public Sprite fullHeart; // sprite of full heart here
    public Sprite emptyHeart; // sprite of empty heart here

    /*
     * HitFlash Variables
     */
    public SkinnedMeshRenderer FlashRenderer { get; set; }
    public float flashIntensity;
    public float FlashIntensity
    {
        get => flashIntensity;
        set => flashIntensity = value;
    }
    public float flashDuration;
    public float FlashDuration 
    {
        get => flashDuration;
        set => flashDuration = value;
    }
    public float FlashTimer { get; set; }

    private void Start()
    {
        FlashRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }
    void Update()
    {
        UpdateHearts();
        TestHurt();
        FlashCoolDown();
        InvincibilityCoolDown();
        //Debug.Log(health);
    }
    void InvincibilityCoolDown()
    {
        if (isInvincible)
        {
            FlashTimer -= Time.unscaledTime;
            float lerp = Mathf.Clamp(invincibilityTimer, 0, invicibilityDuration);
            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
            }
        }
    }
    void FlashCoolDown()
    {
        FlashTimer -= Time.unscaledTime;
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
                isInvincible = true;
            }
        }

        // Coded this before I know one was already made for keys. Mine works and is tested -Steven
        // Code for Health Pick Up
        if(other.gameObject.tag == "HealthPickUp")
        {
            Health++;
            Destroy(other.gameObject);
        }

        // ! Code for Key pickup
        if(other.gameObject.tag == "KeyPickUp")
        {
            keys++;
            Destroy(other.gameObject);
            Debug.Log("Key is now 1");
        }
    }
    public void TakeDamage()
    {
        TakeDamage(1);
    }
    public void TakeDamage(int damageAmount)
    {
        if (!isInvincible)
        {
            Health -= damageAmount;
        }
        if (Health <= 0)
        {
            PlayerDead();
        }
        isInvincible = true;
        invincibilityTimer = invicibilityDuration;
        FlashTimer = FlashDuration;
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
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    void PlayerDead()
    {
        FindObjectOfType<SceneManagement>().LoadCurrentLevel();
    }
    void TestHurt()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            this.TakeDamage();
        }
    }
}
