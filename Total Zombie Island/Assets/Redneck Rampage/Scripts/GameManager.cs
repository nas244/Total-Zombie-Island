using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // define some vars editable in Unity
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject lossUI;
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject howtoplayUI;
    [SerializeField] private GameObject mainUI;

    // define some vars NOT editable in Unity
    private GameObject spawner;
    private GameObject[] weaponSpawners;

    // Start is called before the first frame update
    void Start()
    {
        // grab the spawner GameObject
        spawner = GameObject.FindGameObjectWithTag("SpawnerController");

        // grab the weaponSpawners in the scene
        weaponSpawners = GameObject.FindGameObjectsWithTag("WeaponSpawner");

        // start game with Wave 1
        HowToPlay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HowToPlay()
    {
        // pause game time
        Time.timeScale = 0;

        // disable the mainUI
        mainUI.SetActive(false);

        // show the how to play ui
        howtoplayUI.SetActive(true);
    }

    public void DismissHowToPlay()
    {
        // hide the how to play UI
        howtoplayUI.SetActive(false);

        // show the mainUI
        mainUI.SetActive(true);

        // reset time scale
        Time.timeScale = 1;

        // start Wave 1
        Wave1();
    }

    // sets up the game for Wave 1
    public void Wave1()
    {
        Debug.Log("Beginning of Wave 1");

        // setup the spawners
        spawner.GetComponent<Spawning>().Setup(
            damage: 10,
            health: 100,
            limit: 20,
            delay: 3.0f,
            wave: 1
        );

        // spawn a gun on each of the weapon spawners
        foreach (GameObject weaponSpawner in weaponSpawners)
        {
            weaponSpawner.GetComponent<WeaponSpawner>().Spawn();
        }
    }

    public void Wave2()
    {
        Debug.Log("Beginning of Wave 2");

        // setup the spawners
        spawner.GetComponent<Spawning>().Setup(
            damage: 15,
            health: 120,
            limit: 30,
            delay: 3.0f,
            wave: 2
        );

        // spawn a gun on each of the weapon spawners
        foreach (GameObject weaponSpawner in weaponSpawners)
        {
            weaponSpawner.GetComponent<WeaponSpawner>().Spawn();
        }
    }

    public void Wave3()
    {
        Debug.Log("Beginning of Wave 3");

        // setup the spawners
        spawner.GetComponent<Spawning>().Setup(
            damage: 20,
            health: 150,
            limit: 40,
            delay: 3.0f,
            wave: 3
        );

        // spawn a gun on each of the weapon spawners
        foreach (GameObject weaponSpawner in weaponSpawners)
        {
            weaponSpawner.GetComponent<WeaponSpawner>().Spawn();
        }
    }

    // function called when the game is over
    public void GameOver(bool victory)
    {
        // disable player control
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().gameover = true;

        // if the player won, log to console victory
        if (victory) StartCoroutine(Victory());
        else StartCoroutine(Loss());
    }

    IEnumerator Victory()
    {
        // wait for 3 seconds
        yield return new WaitForSeconds(3);

        // display victory screen
        mainUI.SetActive(false);
        winUI.SetActive(true);
        gameOverUI.SetActive(true);
    }

    IEnumerator Loss()
    {
        // wait for 3 seconds
        yield return new WaitForSeconds(3);

        // display game over screen
        mainUI.SetActive(false);
        lossUI.SetActive(true);
        gameOverUI.SetActive(true);
    }

    public void ReturnToLevel()
    {
        SceneManager.LoadScene("Overworld");
    }

    public void ReplayScene()
    {
        SceneManager.LoadScene("Redneck Rampage");
    }
}
