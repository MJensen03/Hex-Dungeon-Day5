using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathAnimationScript : MonoBehaviour
{

    [SerializeField] SpriteRenderer fadeImage;
    [SerializeField] float fadeDuration = 1f;

    private Color startColor;
    private Color targetColor;
    private float timer;
    private bool isFading;


    // Start is called before the first frame update
    void Start()
    {
        startColor = fadeImage.color;

        targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        fadeImage.color = Color.clear;

        StartFade();
    }

    public void StartFade()
    {
        isFading = true;
        timer = 0f;
    }

    private void Update()
    {
        if (isFading)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration;
            fadeImage.color = Color.Lerp(startColor, targetColor, progress);

            if (progress >= 1f)
            {
                isFading = false;
            }
        }
    }
}
