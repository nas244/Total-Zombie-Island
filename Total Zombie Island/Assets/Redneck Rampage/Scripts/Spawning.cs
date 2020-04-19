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

    // define some vars NOT editable in Unity
    private int spawnCount;
    private float lastSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        // grab all active spawnpoints in scene and save their scripts
        spawnpoints = GameObject.FindGameObjectsWithTag("Spawner");
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
    }

    void Spawn()
    {
        // instantiate a new zombie at a random spawn point
        GameObject spawnpoint = spawnpoints[Random.Range(0, spawnpoints.Length)];
        GameObject zombie = Instantiate(zombiePrefab, spawnpoint.transform);
        zombie.transform.position = spawnpoint.transform.position;
        zombie.SetActive(false);
        zombie.SetActive(true);
        Debug.Log("Zombie spawned at spawnpoint: \"" + spawnpoint.name + "\".");

        // reset the last spawn time and update spawn count
        lastSpawnTime = Time.time;
        spawnCount++;
    }
}
