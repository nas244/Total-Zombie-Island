using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    // define some vars editable in Unity
    [SerializeField] private float spawnDelay;
    [SerializeField] private GameObject shotgunPrefab;
    [SerializeField] private GameObject assaultPrefab;
    [SerializeField] private GameObject minigunPrefab;
    [SerializeField] private GameObject katanaPrefab;
    [SerializeField] private GameObject medkitPrefab;
    public float lastSpawnTime;
    public bool enable = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // check if spawnDelay time has passed
        if ((Time.time - lastSpawnTime) > spawnDelay)
        {
            // spawn a gun
            Spawn();
        }
    }

    // calculates chance and spawns resulting gun
    public void Spawn()
    {
        // check if this spawner is enabled
        if (!enable) return;

        // determine which gun to spawn
        GameObject weapon;
        int chance = Random.Range(1, 4);
        switch (chance)
        {
            case 1:
                weapon = Instantiate(shotgunPrefab, this.transform);
                break;
            case 2:
                weapon = Instantiate(minigunPrefab, this.transform);
                break;
            case 3:
                weapon = Instantiate(assaultPrefab, this.transform);
                break;
            default:
                weapon = Instantiate(katanaPrefab, this.transform);
                break;
        }
        weapon.GetComponent<Weapon>().parentSpawner = this.gameObject;

        // calculate chance of spawning additional health too
        chance = Random.Range(1, 5);
        if (chance == 1) Instantiate(medkitPrefab, this.transform);

        // disable spawner
        enable = false;
    }
}
