using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    
 
    void Awake()
    {
        FindObjectOfType<MapGenerator>().GenerateDungeon();
        Debug.Log("here");
    }

    
}
