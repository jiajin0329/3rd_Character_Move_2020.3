using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Close_Game : MonoBehaviour
{
    private bool _switch = false;

    /// <summary>
    /// UI
    /// </summary>
    [SerializeField]
    [Header("UI")]
    private GameObject ui;

    public void _Close_Game()
    {
        Application.Quit();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _switch = !_switch;

            if(_switch)
            {
                //鎖住滑鼠並隱藏
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                //鎖住滑鼠並隱藏
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            ui.SetActive(_switch);
        }
    }
}
