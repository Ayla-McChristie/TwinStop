using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    [RequireComponent(typeof(PlayerMovement))]
    class PlayerStats : MonoBehaviour
    {
        public int health; // Total amount of health left
        public int numOfHearts; // Max amount of hearts a player can have, should be 3

        public int numOfKeys;

        public Image[] hearts; // all heart UI game objects go here
        public Sprite fullHeart; // sprite of full heart here
        public Sprite emptyHeart; // sprite of empty heart here

        void Update()
        {
            // Health Lock
            if (health > numOfHearts)
            {
                health = numOfHearts; // this makes sure that players can never go over the set amount of hearts

            }

            // system for turning full hearts to empty hearts
            for (int i = 0; i < hearts.Length; i++)
            {
                if (i < health)
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

        // TODO add code for projectiles
        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "Enemy") // || other.gameObject.tag == "Projectiles"?
            {
                health--;
            }
        }
    }
