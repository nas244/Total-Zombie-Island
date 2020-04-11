using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    // define some vars editable in Unity
    
    [Tooltip("Global limit on how many zombies can be alive at any given time. Set to -1 to ignore.")]
    public int globalSpawnLimit;
    
    // define some vars non-editable in Unity
    [System.NonSerialized]
    public int globalSpawnCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // checks if the global limit on zombie spawns has been reached
    // returns true if limit has NOT been reached; false otherwise
    public bool CheckGlobalSpawnLimit()
    {
        if (globalSpawnLimit == -1 || globalSpawnCount < globalSpawnLimit)
        {
            return true;
        }

        return false;
    }
}
