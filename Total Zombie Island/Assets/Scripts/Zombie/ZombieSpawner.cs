using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    // define some public vars editable in Unity
    [Tooltip("Prefab that will be used to instantiate zombies.")]
    public GameObject zombiePrefab;
    [Tooltip("Specifies the max amount of zombies that can be alive from this spawner. Set to -1 to ignore.")]
    public int localSpawnLimit;
    [Tooltip("Specifies the amount of time to wait between spawns on an enemy.")]
    public float spawnDelay;
    [Tooltip("Specifies how close the player must be before the spawner starts spawning enemies.")]
    public float spawnDistance;
    [Tooltip("Specifies the displacement of the spawn location relative to this ZombieSpawner game object. Leave all 0's to spawn directly on spawn point.")]
    public Vector3 spawnDisplacement;
    [Tooltip("Enables the debugging messages on this spawner.")]
    public bool debugging;

    // define some publics vars NOT editable in Unity
    [System.NonSerialized]
    public SpawnerController controller;
    [System.NonSerialized]
    public GameObject targetPlayer;
    [System.NonSerialized]
    public float lastSpawnTime;
    [System.NonSerialized]
    public int localSpawnCount;
    [System.NonSerialized]
    public Vector3 spawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        // set last spawn time so that the first zombie will spawn immediately
        lastSpawnTime = Time.time - spawnDelay;

        // try to find a SpawnerController if one exists
        GameObject controllerObject = GameObject.FindGameObjectWithTag("SpawnerController");
        if (controllerObject) controller = controllerObject.GetComponent<SpawnerController>();
        else if (debugging) Debug.LogWarning("Did not find SpawnerController to attach to on \"" + this.name + "\".");

        // add the spawn displacement to the spawn position
        spawnPosition = new Vector3(
            transform.position.x + spawnDisplacement.x,
            transform.position.y + spawnDisplacement.y,
            transform.position.z + spawnDisplacement.z
        );

        // try to find the target player
        targetPlayer = GameObject.FindGameObjectWithTag("Player");
        if (!targetPlayer) Debug.LogError("Failed to find a tagged \"Player\" object! Spawning won't work!");

        // check if a zombie prefab has been set
        if (!zombiePrefab) Debug.LogError("No Zombie Prefab specified for spawn point \"" + this.name + "\"! Spawning won't work!");
    }

    // Update is called once per frame
    void Update()
    {
        // check if the player is near by
        if (DetectPlayer())
        {
            // check if spawnDelay has elapsed
            if (CheckSpawnDelay())
            {
                // check if the local and global spawn limits have been reached
                if (CheckSpawnLimit())
                {
                    // spawn a Zombie
                    Spawn();
                }
            }
        }
    }

    // checks if the player is nearby within spawnDistance
    bool DetectPlayer()
    {
        // check if the player is within spawnDistance of this instance of ZombieSpawner
        if (targetPlayer && Vector3.Distance(targetPlayer.transform.position, this.transform.position) <= spawnDistance)
        {
            return true;
        }
        
        return false;
    }

    // checks if spawnDelay time has passed since lastSpawnTime
    public bool CheckSpawnDelay()
    {
        if (Time.time - lastSpawnTime >= spawnDelay)
        {
            return true;
        }

        return false;
    }

    // checks if the local/global spawn limits have been reached
    // returns true if limit has NOT been reached; false otherwise
    public bool CheckSpawnLimit()
    {
        // check if -1 to ignore and return true as default
        if (localSpawnLimit == -1 || localSpawnCount < localSpawnLimit)
        {
            // check if the global spawn limit has been reached
            if ((controller && controller.CheckGlobalSpawnLimit()) || !controller)
            {
                return true;
            }
        }

        return false;
    }

    // spanws a new instance of zombiePrefab
    void Spawn()
    {
        // check for zombiePrefab
        if (!zombiePrefab) return;

        // instantiate a new zombie in the 3D world at the location of spawnPosition
        GameObject newZombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
        newZombie.transform.parent = this.transform;
        if (debugging) Debug.Log("Zombie spawned at \"" + this.name + "\".");

        // update lastSpawnTime to now
        lastSpawnTime = Time.time;

        // update spawn counts
        localSpawnCount++;
        if (controller) controller.globalSpawnCount++;

        // log to the console if the spawn limits have been reached
        if (debugging && localSpawnCount >= localSpawnLimit) Debug.Log("Local spawn limit reached at \"" + this.name + "\": " + localSpawnLimit.ToString());
        if (controller && !controller.CheckGlobalSpawnLimit()) Debug.Log("Global spawn limit reached.");
    }
}
