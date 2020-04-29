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
    [System.NonSerialized] public bool canPause;

    public LevelLoader Loader;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;

        // grab the spawner GameObject
        spawner = GameObject.FindGameObjectWithTag("SpawnerController");

        // grab the weaponSpawners in the scene
        weaponSpawners = GameObject.FindGameObjectsWithTag("WeaponSpawner");

        // start game with Wave 1
        StartCoroutine(HowToPlay());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator HowToPlay()
    {
        // make it so the player cannot pause
        canPause = false;

        // pause game time
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0;

        // disable the mainUI
        mainUI.SetActive(false);

        // show the how to play ui
        howtoplayUI.SetActive(true);
    }

    public void DismissHowToPlay()
    {
        // enable player to pause
        canPause = true;

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
            limit: 1,//10,
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
            limit: 1,//15,
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
            health: 120,
            limit: 1,//20,
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
    public IEnumerator GameOver(bool victory)
    {
        // disable player control
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().gameover = true;

        // make it so the player cannot pause
        canPause = false;

        // if the player won, log to console victory
        if (victory) StartCoroutine(Victory());
        else StartCoroutine(Loss());

        yield break;

        //yield return new WaitForSeconds(3);
        //ReturnToLevel();
    }

    IEnumerator Victory()
    {
        // wait for 3 seconds
        yield return new WaitForSeconds(3);

        // display victory screen
        mainUI.SetActive(false);
        winUI.SetActive(true);
        gameOverUI.SetActive(true);

        Debug.Log(State_Data.Instance._MG2Complete);
        //yield break;
        if (!State_Data.Instance._MG2Complete)
        {
            Debug.Log("Completed first time");
            State_Data.Instance._MG2Complete = true;
            State_Data.Instance._currentObjective += 1;
            State_Data.Instance._score += 1;
            yield break;
        }

        else if (State_Data.Instance._MG2Complete)
        {
            Debug.Log("Completed again");
            State_Data.Instance._score += 0.25f;
            yield break;
        }

    }

    IEnumerator Loss()
    {
        // wait for 3 seconds
        yield return new WaitForSeconds(3);

        Debug.Log(State_Data.Instance._MG2Complete);
        // display game over screen
        mainUI.SetActive(false);
        lossUI.SetActive(true);
        gameOverUI.SetActive(true);
        State_Data.Instance._score -= .5f;
    }

    public void ReturnToLevel()
    {
        // reset time scale if needed
        Time.timeScale = 1;

        Loader.LoadLevel("Overworld");
    }

    public void ReplayScene()
    {
        Loader.LoadLevel("Redneck Rampage");
    }
}
