using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoDayCommandsController : MonoBehaviour
{
    void Update()
    {
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftCommand)) && Input.GetKey(KeyCode.R))
        {
            // Reload for Ctrl/Cmd+R
            GameManager.Instance.HandleReloadCurrScene();
        }
    }
}
