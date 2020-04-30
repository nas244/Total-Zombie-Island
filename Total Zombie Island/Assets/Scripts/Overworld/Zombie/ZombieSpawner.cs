using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    // define some public vars editable in Unity
    [Tooltip("Prefab that will be used to instantiate zombies.")]
    public GameObject[] zombiePrefab;

    // define some publics vars NOT editable in Unity
    [System.NonSerialized]
    public SpawnArea spawnArea;

    // Start is called before the first frame update
    void Start()
    {
        // check if we're in a spawn area
        if (transform.parent && transform.parent.TryGetComponent<SpawnArea>(out spawnArea))
        {
            if (spawnArea.debugging) Debug.Log("\"" + this.name + "\" is member of spawn area.");
        }
        else
        {
            Debug.LogError("No spawn area detected for \"" + this.name + "\". Spawning won't work!");
        }

        // check if a zombie prefab has been set
        //if (!zombiePrefab) Debug.LogError("No Zombie Prefab specified for spawn point \"" + this.name + "\"! Spawning won't work!");
    }

    // Update is called once per frame
    void Update()
    {

    }

    // spanws a new instance of zombiePrefab
    public void Spawn()
    {
        // if no zombiePrefab has been set, don't spawn
        //if (!zombiePrefab) return;


        int index = Random.Range(0, 11);
        // instantiate a new zombie in the 3D world at the location of spawnPosition
        GameObject newZombie = Instantiate(zombiePrefab[index], transform.position, Quaternion.identity);
        newZombie.transform.parent = this.transform;
        if (spawnArea && spawnArea.debugging) Debug.Log("Zombie spawned at \"" + this.name + "\".");
    }
}
