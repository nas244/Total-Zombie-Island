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

    public void DisableAll()
    {
        Theme2.SetActive(false);
        Theme3.SetActive(false);
        EscapeTheme.SetActive(false);
        Theme1.SetActive(false);
    }
}
