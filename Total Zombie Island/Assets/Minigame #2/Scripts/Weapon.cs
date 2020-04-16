using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // define some vars editable in Unity
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private string weaponType;
    [SerializeField] private int ammo;

    // define some vars NOT editable in Unity
    private Renderer renderer;
    private Renderer[] children;
    private bool selected;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        // grab the rednerer for this gameobject
        renderer = gameObject.GetComponent<Renderer>();

        // find the player object
        player = GameObject.FindGameObjectWithTag("Player");

        // find all child renderer objects
        children = gameObject.GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // check if the player wants to pick it up with space bar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // check if the player is close enough to pick it up
            if (Vector3.Distance(player.transform.position, this.transform.position) <= 3.0f)
            {
                // if this object is selected, equip it
                if (selected)
                {
                    player.GetComponent<PlayerMovement>().EquipWeapon(weaponType, ammo);
                    selected = false;
                    Destroy(gameObject);
                }
            }
        }
    }

    // detect when the player hovers over the weapon object
    void OnMouseOver()
    {
        selected = true;
        renderer.material = outlineMaterial;
        foreach (Renderer rend in children) rend.material = outlineMaterial;
    }

    // detect when the player stops hovering over the weapon object
    void OnMouseExit()
    {
        selected = false;
        renderer.material = normalMaterial;
        foreach (Renderer rend in children) rend.material = normalMaterial;
    }
}
