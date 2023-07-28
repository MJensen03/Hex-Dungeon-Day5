using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPUIScript : MonoBehaviour
{
    [SerializeField] int currentHealth;
    int maxHealth = 6;

    [SerializeField] Image hp_image;
    [SerializeField] Sprite[] all_hp_images;


    void Update()
    {
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        } else if (currentHealth <= 0)
        {
            currentHealth = 1;
        }

        hp_image.sprite = all_hp_images[currentHealth-1];
    }

    public void SetHealth(int health)
    {
        currentHealth = health;
    }
}
