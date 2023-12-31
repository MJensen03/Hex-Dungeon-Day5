using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButtonScript : MonoBehaviour
{
    [SerializeField] GameObject panelObj;
    public void InitiateUI()
    {
        panelObj.SetActive(true);
    }

    public void RestartButtonPressed()
    {
        PlayerPrefs.SetInt("level", 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        PlayerPrefs.SetInt("level", 0);
        PlayerPrefs.SetInt("maxRoomCount", 5);
        PlayerPrefs.SetInt("dungeonWidth", 120);
        PlayerPrefs.SetInt("dungeonHeight", 80);

    }
}
