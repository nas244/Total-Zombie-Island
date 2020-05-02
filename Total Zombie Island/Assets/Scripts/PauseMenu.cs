using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public bool canBeReloaded;
    bool isLocked;
    bool canPause = false;

    public GameObject pauseMenuUI;
    public GameObject weaponWheel;

    public Animator anim;

    void Start()
    {
        pauseMenuUI.SetActive(false);
        StartCoroutine(PauseDelay());
    }

    void Update()
    {
        if (canPause)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (isPaused)
                {
                    StartCoroutine(Resume());
                }

                else
                {
                    StartCoroutine(Pause());
                }
            }
        }      
    }

    public void OnResumeButton()
    {
        StartCoroutine(Resume());
    }

    IEnumerator Resume()
    {
        anim.SetTrigger("Close");

        while (AudioListener.volume != 1f)
        {
            AudioListener.volume += 0.25f;
            yield return new WaitForSecondsRealtime(0.25f);
        }

        pauseMenuUI.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;

        if (isLocked)
        {
            isLocked = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        /*if (SceneManager.GetActiveScene().name == "Overworld")
        {
            //if (weaponWheel.activeSelf)
            Cursor.lockState = CursorLockMode.Locked;
        }*/
    }

    public void Retry()
    {
        if (canBeReloaded)
        {
            Time.timeScale = 1f;
            AudioListener.volume = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void Quit ()
    {
        Application.Quit();
    }

    IEnumerator Pause()
    {
        if (Cursor.lockState == CursorLockMode.Locked) { isLocked = true; }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        pauseMenuUI.SetActive(true);

        while (AudioListener.volume != 0.25f)
        {
            AudioListener.volume -= 0.25f;
            yield return new WaitForSecondsRealtime(0.25f);
        }

        isPaused = true;
        Time.timeScale = 0f;
    }

    IEnumerator PauseDelay()
    {
        yield return new WaitForSeconds(2);
        canPause = true;
    }
}
