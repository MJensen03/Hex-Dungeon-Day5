using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletsUIScript : MonoBehaviour
{

    [SerializeField] int currentBullets;
    int maxBullets = 6;

    [SerializeField] Image[] bullet_images;
    [SerializeField] Sprite emptyBullet;
    [SerializeField] Sprite fullBullet;


    void Update()
    {
        if (currentBullets > maxBullets)
        {
            currentBullets = maxBullets;
        }

        for (int i = 0; i < bullet_images.Length; i++)
        {
            if (i < currentBullets)
            {
                bullet_images[i].sprite = fullBullet;
            }
            else
            {
                bullet_images[i].sprite = emptyBullet;
            }
        }
    }

    public void SetBullets(int bullets)
    {
        currentBullets = bullets;
    }
}
