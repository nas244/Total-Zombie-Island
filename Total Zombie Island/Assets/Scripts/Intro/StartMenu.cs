using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public LevelLoader Loader;


    public void OnStartButton()
    {
        Loader.LoadLevel("Hector");
    }

    public void OnQuitButton()
    {
        // Quit Game
    }
}
