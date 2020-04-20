using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // define som vars editable in Unity
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject mainUI;

    // define some vars NOT editable in Unity
    private GameObject gameManager;
    private bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        // grab the game manager from the scene
        gameManager = GameObject.FindGameObjectWithTag("GameController");
    }

    // Update is called once per frame
    void Update()
    {
        // check if the player has pressed the escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // if the game isn't already paused
            if (!paused)
            {
                // if the player can pause, pause the game
                if (gameManager.GetComponent<GameManager>().canPause) PauseGame();
            }

            // otherwise, the game is currently paused
            else
            {
                // if the player can pause, resume the game
                if (gameManager.GetComponent<GameManager>().canPause) ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        // stop time
        Time.timeScale = 0;

        // update paused
        paused = true;

        // display the pause screen
        mainUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void ResumeGame()
    {
        // reset time to normal
        Time.timeScale = 1;

        // update paused
        paused = false;

        // hide the pause menu
        pauseMenuUI.SetActive(false);
        mainUI.SetActive(true);
    }
}
