using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonDoor : MonoBehaviour
{
    private int levelsCleared;
    private void Awake()
    {
        // instance = GameManager.Instance;
        levelsCleared = PlayerPrefs.GetInt("level");
        levelsCleared++;
        PlayerPrefs.SetInt("level", levelsCleared);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        if (collision.gameObject.tag != "Player") return;

       
        
        SceneManager.LoadScene(1);

    }
}
