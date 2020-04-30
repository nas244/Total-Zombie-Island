using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public LevelLoader Loader;


    public void OnStartButton()
    {
        Loader.LoadLevel("Hector");
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
