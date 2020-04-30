using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public GameObject WeaponWheelUI;

    public bool SlowDownGame = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (SlowDownGame)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Resume();

            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                SlowDown();
            }
        }
    }

    void Resume()
    {
        
        WeaponWheelUI.SetActive(false);
        Time.timeScale = 1f;
        SlowDownGame = false;

    }

    void SlowDown()
    {
        WeaponWheelUI.SetActive(true);
        Time.timeScale = 0.25f;
        SlowDownGame = true;
    }

}
