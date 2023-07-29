using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    [SerializeField]
    private TextMeshProUGUI levelText;

    public int levelsCleared = 0;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                return null;

            return _instance;
        }
    }


    private void Awake()
    {
        _instance = this;
        if(levelText == null)
            levelText = GameObject.FindGameObjectWithTag("UI").GetComponent<TextMeshProUGUI>();
        levelText.text = ("Level: " + levelsCleared);
        DontDestroyOnLoad(gameObject);
    }


}
