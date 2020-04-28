using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    // define some variables editable in Unity
    [Tooltip("Specifies the max amount of zombies that can be alive from this spawner. Set to -1 to ignore.")]
    public int localSpawnLimit;
    [Tooltip("Specifies the amount of time to wait between spawns on an enemy.")]
    public float spawnDelay;
    [Tooltip("Enables the debugging messages on this spawn area.")]
    public bool debugging;

    // define some variables NOT editabl in Unity
    [System.NonSerialized]
    public ZombieSpawner[] spawners;
    [System.NonSerialized]
    public SpawnerController controller;
    [System.NonSerialized]
    public GameObject targetPlayer;
    [System.NonSerialized]
    public float lastSpawnTime;
    [System.NonSerialized]
    public int localSpawnCount;
    [System.NonSerialized]
    public bool playerDetected;

    // Start is called before the first frame update
    void Start()
    {
        // grab all of the children spawners to this SpawnArea
        spawners = GetComponentsInChildren<ZombieSpawner>();
        if (spawners.Length <= 0) Debug.LogError("No spawners found in \"" + this.name + "\". Spawning won't work!");

        // try to find the target player
        targetPlayer = GameObject.FindGameObjectWithTag("Player");
        if (!targetPlayer) Debug.LogError("Failed to find a tagged \"Player\" object! Spawning won't work!");

        // try to find a SpawnerController if one exists
        GameObject controllerObject = GameObject.FindGameObjectWithTag("SpawnerController");
        if (controllerObject) controller = controllerObject.GetComponent<SpawnerController>();
        else if (debugging) Debug.LogWarning("Did not find SpawnerController to attach to on \"" + this.name + "\".");

        // set the last spawn time so that a zombie will instantly spawn upon trigger
        lastSpawnTime = Time.time - spawnDelay;
    }

    // Update is called once per frame
    void Update()
    {
        // check if the player is in the spawn area
        //if (playerDetected)
        //{
            // check if the spawnDelay has passed
            if (CheckSpawnDelay())
            {
                // check if the local spawn limit has been reached
                if (CheckSpawnLimit())
                {
                    // spawn a zombie
                    Spawn();
                }
            }
        //}
    }

    // function called when something sets off the trigger
    private void OnTriggerEnter(Collider other)
    {
        // check if this was the player
        if (other.CompareTag("Player"))
        {
            playerDetected = true;
            if (debugging) Debug.Log("Player entered trigger \"" + this.name + "\".");
        }
        
    }

    // fucntion called when something leaves the trigger area
    private void OnTriggerExit(Collider other)
    {
        // check if the player left
        if (other.CompareTag("Player"))
        {
            playerDetected = false;
            if (debugging) Debug.Log("Player left trigger \"" + this.name + "\".");
        }
    }

    // checks if spawnDelay time has passed since lastSpawnTime
    bool CheckSpawnDelay()
    {
        if (Time.time - lastSpawnTime >= spawnDelay)
        {
            return true;
        }

        return false;
    }

    // checks if the local/global spawn limits have been reached
    // returns true if limit has NOT been reached; false otherwise
    bool CheckSpawnLimit()
    {
        // check if -1 to ignore and return true as default
        if (localSpawnLimit < 0 || localSpawnCount < localSpawnLimit)
        {
            // check if the global spawn limit has been reached
            if ((controller && controller.CheckGlobalSpawnLimit()) || !controller)
            {
                return true;
            }
        }

        return false;
    }

    // spawns a zombie on the child spawnpoint that is farthest from the player
    public void Spawn()
    {
        // don't spawn if there aren't any spawners
        if (spawners.Length <= 0) return;

        // find the farthest spawn point from the player
        ZombieSpawner spawnPoint = spawners[0];
        foreach (ZombieSpawner spawner in spawners)
        {
            if (Vector3.Distance(targetPlayer.transform.position, spawner.transform.position) > Vector3.Distance(targetPlayer.transform.position, spawnPoint.transform.position))
            {
                spawnPoint = spawner;
            }
        }

        // spawn a zombie on the spawnPoint
        spawnPoint.Spawn();

        // update the last spawn time
        lastSpawnTime = Time.time;

        // update the global and local spawn counts
        localSpawnCount++;
        if (controller) controller.globalSpawnCount++;

        // log to the console if the spawn limits have been reached
        if (debugging && localSpawnCount >= localSpawnLimit) Debug.Log("Local spawn limit reached at \"" + this.name + "\": " + localSpawnLimit.ToString());
        if (controller && !controller.CheckGlobalSpawnLimit()) Debug.Log("Global spawn limit reached.");
    }
}
