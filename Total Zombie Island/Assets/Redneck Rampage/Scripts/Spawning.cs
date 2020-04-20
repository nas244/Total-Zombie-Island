using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning : MonoBehaviour
{
    // define some vars editable in Unity
    [SerializeField] private GameObject[] spawnpoints;
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private float spawnDelay;
    [SerializeField] private int spawnLimit;
    [SerializeField] private int zombieHealth;
    [SerializeField] private int zombieDamage;
    [SerializeField] private GameObject zombiesUI;
    [SerializeField] private GameObject wavesUI;

    // define some vars NOT editable in Unity
    private GameObject gameManager;
    private int spawnCount;
    private int zombiesReamaining;
    private float lastSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        // grab the gameManager
        gameManager = GameObject.FindGameObjectWithTag("GameController");

        // grab all active spawnpoints in scene and save their scripts
        spawnpoints = GameObject.FindGameObjectsWithTag("Spawner");

        // init some vars
        zombiesReamaining = spawnLimit;
    }

    // Update is called once per frame
    void Update()
    {
        // if spawndelay time has passed
        if ((Time.time - lastSpawnTime) >= spawnDelay)
        {
            // make sure the spawn limit hasn't been reached
            if (spawnCount < spawnLimit)
            {
                Spawn();
            }
        }

        // update the UI
        UpdateUI();

        // if no more zombies remain, game over
        if (zombiesReamaining <= 0)
        {
            gameManager.GetComponent<GameManager>().GameOver(true);
        }
    }

    // updates the UI on the screen
    void UpdateUI()
    {
        // update remaining zombies counter
        zombiesUI.GetComponent<TextMesh>().text = zombiesReamaining.ToString();

        // update wave counter
        wavesUI.GetComponent<TextMesh>().text = "1/3";
    }

    void Spawn()
    {
        // instantiate a new zombie at a random spawn point
        GameObject spawnpoint = spawnpoints[Random.Range(0, spawnpoints.Length)];
        GameObject zombie = Instantiate(zombiePrefab, spawnpoint.transform);
        zombie.transform.position = spawnpoint.transform.position;
        zombie.GetComponent<Zombie>().parentSpawner = this.gameObject;
        Debug.Log("Zombie spawned at spawnpoint: \"" + spawnpoint.name + "\".");

        // reset the last spawn time and update spawn count
        lastSpawnTime = Time.time;
        spawnCount++;
    }

    public void UpdateCount()
    {
        // decrement the zombies remaining counter
        zombiesReamaining--;
    }
}
