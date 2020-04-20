using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
        Wave1();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        // if the player won, log to console victory
        if (victory) StartCoroutine(Victory());
        else StartCoroutine(Loss());
    }

    IEnumerator Victory()
    {
        // wait for 3 seconds
        yield return new WaitForSeconds(3);

        // display victory screen
        Debug.Log("You won!");
    }

    IEnumerator Loss()
    {
        // wait for 3 seconds
        yield return new WaitForSeconds(3);

        // display game over screen
        Debug.Log("Game over, you lost!");
    }
}
