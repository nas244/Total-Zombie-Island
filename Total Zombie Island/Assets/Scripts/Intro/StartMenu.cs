using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public LevelLoader Loader;
    public GameObject Tutorial;

    private void Start()
    {
        Cursor.visible = true;
        Tutorial.SetActive(false);
    }

    public void OnStartButton()
    {
        Loader.LoadLevel("Hector");
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void OpenTutorial()
    {
        Tutorial.SetActive(true);
    }
}
