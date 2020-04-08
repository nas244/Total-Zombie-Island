using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    // define some public vars editable in Unity
    [Tooltip("Specifies the master SpawnerController that controls this spawner.")]
    public SpawnerController controller;

    // Start is called before the first frame update
    void Start()
    {
        // grab SpawnerController script from controller and store back in controller
        controller = controller.GetComponent<SpawnerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // check if the player is near by
        if (DetectPlayer())
        {
            // check if spawnDelay has elapsed
            if (controller.CheckSpawnDelay())
            {
                // check if the spawnLimit has been reached
                if (controller.CheckSpawnLimit())
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
        if (Vector3.Distance(controller.targetPlayer.transform.position, this.transform.position) <= controller.spawnDistance)
        {
            return true;
        }
        
        return false;
    }

    // spanws a new instance of enemyObject
    void Spawn()
    {
        Debug.Log("Spawn called on spawner \""+ this.name + "\".");

        // TODO: add code to instantiate a zombie

        // update lastSpawnTime to now
        controller.lastSpawnTime = Time.time;

        // update spawnCount
        controller.spawnCount++;
    }
}
