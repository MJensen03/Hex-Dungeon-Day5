using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButtonScript : MonoBehaviour
{
    [SerializeField] GameObject panelObj;
    public void InitiateUI()
    {
        panelObj.SetActive(true);
    }

    public void RestartButtonPressed()
    {
        // Restart Scene or Respawn Player
    }
}
