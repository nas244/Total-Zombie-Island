using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeManager : MonoBehaviour
{
    [SerializeField]
    GameObject Theme1, Theme2, Theme3, EscapeTheme;

    // Start is called before the first frame update
    void Start()
    {
        //SetTheme();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            SetTheme();
            Theme1.SetActive(true);
        }

        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            SetTheme();
            Theme2.SetActive(true);
        }

        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            SetTheme();
            Theme3.SetActive(true);
        }

        else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            SetTheme();
            EscapeTheme.SetActive(true);
        }*/
    }

    public void SetTheme()
    {
        switch (State_Data.Instance._currentObjective)
        {
            case 0:
                Theme2.SetActive(false);
                Theme3.SetActive(false);
                EscapeTheme.SetActive(false);
                Theme1.SetActive(true);
                break;

            case 1:
                Theme1.SetActive(false);
                Theme3.SetActive(false);
                EscapeTheme.SetActive(false);
                Theme2.SetActive(true);
                break;

            case 2:
                Theme1.SetActive(false);
                Theme2.SetActive(false);
                EscapeTheme.SetActive(false);
                Theme3.SetActive(true);
                break;

            case 3:
                Theme1.SetActive(false);
                Theme2.SetActive(false);
                Theme3.SetActive(false);
                EscapeTheme.SetActive(true);
                break;
        }
    }
}
