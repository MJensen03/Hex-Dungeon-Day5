using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    private void Start()
    {

        // Has Secondary refresh in RestartButtonScript.cs
        PlayerPrefs.SetInt("level", 0);
        PlayerPrefs.SetInt("maxRoomCount", 5);
        PlayerPrefs.SetInt("dungeonWidth", 120);
        PlayerPrefs.SetInt("dungeonHeight", 80);
    }
}
