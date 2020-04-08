using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    // define some vars editable in Unity
    [Tooltip("Specifies the amount of time to wait between spawns on an enemy.")]
    public float spawnDelay;
    [Tooltip("Specifies how close the player must be before the spawner starts spawning enemies.")]
    public float spawnDistance;
    [Tooltip("Specifies the target player that will trigger the spawn of enemies. Will be set as target for all spawned enemies.")]
    public GameObject targetPlayer;
    [Tooltip("Upper limit on how many zombies can be alive at any given time.")]
    public int spawnLimit;
    
    // define some vars non-editable in Unity
    [System.NonSerialized]
    public float lastSpawnTime;
    [System.NonSerialized]
    public int spawnCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // checks if spawnDelay time has passed since lastSpawnTime
    // returns true if spawnDelay time has passed; false otherwise
    public bool CheckSpawnDelay()
    {
        if (Time.time - lastSpawnTime >= spawnDelay)
        {
            return true;
        }

        return false;
    }

    // checks if the upper limit on zombie spawns has been reached
    // returns true if limit has NOT been reached; false otherwise
    public bool CheckSpawnLimit()
    {
        if (spawnCount < spawnLimit)
        {
            return true;
        }

        return false;
    }
}
