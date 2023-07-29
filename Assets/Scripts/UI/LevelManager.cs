using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private TextMeshProUGUI levelText;
    private void Awake()
    {
        levelText = GetComponent<TextMeshProUGUI>();
        levelText.text ="Levels: " + PlayerPrefs.GetInt("level" );
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("level", 0);
    }
}
