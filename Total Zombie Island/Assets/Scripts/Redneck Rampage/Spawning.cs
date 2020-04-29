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
    private GameManager gameManager;
    private int spawnCount;
    private int zombiesReamaining;
    private float lastSpawnTime;
    private int wave;
    private bool stopUpdating = false;

    // Start is called before the first frame update
    void Start()
    {
        // grab the gameManager
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        // grab all active spawnpoints in scene and save their scripts
        spawnpoints = GameObject.FindGameObjectsWithTag("Spawner");

        // init some vars
        zombiesReamaining = spawnLimit;
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopUpdating)
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

            // if no more zombies remain, move to next round or game over
            if (zombiesReamaining <= 0)
            {
                switch (wave)
                {
                    case 1:
                        gameManager.Wave2();
                        break;
                    case 2:
                        gameManager.Wave3();
                        break;
                    case 3:
                        stopUpdating = true;
                        StartCoroutine(gameManager.GameOver(true));
                        break;
                }
            }
        }

    }

    // sets this instance of spawning to the specified values and resets all counters
    public void Setup(int damage, int health, int limit, float delay, int wave)
    {
        // setup specified values
        zombieDamage = damage;
        zombieHealth = health;
        spawnLimit = 1;//limit;
        spawnDelay = delay;

        // reset counters
        spawnCount = 0;
        zombiesReamaining = limit;
        lastSpawnTime = Time.time;
        this.wave = 3;//wave;

        Debug.Log("Spawning reset.");
    }

    // updates the UI on the screen
    void UpdateUI()
    {
        // update remaining zombies counter
        zombiesUI.GetComponent<TextMesh>().text = zombiesReamaining.ToString();

        // update wave counter
        wavesUI.GetComponent<TextMesh>().text = wave.ToString() + "/3";
    }

    void Spawn()
    {
        // instantiate a new zombie at a random spawn point
        GameObject spawnpoint = spawnpoints[Random.Range(0, spawnpoints.Length)];
        GameObject zombie = Instantiate(zombiePrefab, spawnpoint.transform);
        zombie.transform.position = spawnpoint.transform.position;

        // setup zombie attributes
        zombie.GetComponent<Zombie>().parentSpawner = this.gameObject;
        zombie.GetComponent<Zombie>().health = zombieHealth;
        zombie.GetComponent<Zombie>().attackDamage = zombieDamage;

        // reset the last spawn time and update spawn count
        lastSpawnTime = Time.time;
        spawnCount++;

        Debug.Log("Zombie spawned at spawnpoint: \"" + spawnpoint.name + "\".");
    }

    public void UpdateCount()
    {
        // decrement the zombies remaining counter
        zombiesReamaining--;
    }
}
