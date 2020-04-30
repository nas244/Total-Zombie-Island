using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Interact_Prompt : MonoBehaviour
{
    [SerializeField]
    private GameObject _Mainchar;

    [SerializeField]
    private float _promptDist = 5.0f;

    private Vector3 _origin;

    [SerializeField]
    private TextMeshProUGUI _interactprompt; //update this

    [SerializeField]
    string Objective1, Objective2, Objective3, Objective4;

    [SerializeField]
    private GameObject[] _interactPrefabs;

    private bool _show;
    private bool _showobj;

    private Player_movement _playerCtrl;

    [SerializeField]
    LevelLoader Loader;

    /*[SerializeField]
    private GameObject[] _objectivePrefabs;
    [SerializeField]
    private GameObject[] _objectiveMarkers;*/

    public GameObject Icon;
    //GameObject iconObject;

    // Start is called before the first frame update
    void Start()
    {
        _playerCtrl = _Mainchar.GetComponent<Player_movement>();

        // Add the interact icon to each interacable item
        foreach (GameObject interact in _interactPrefabs)
        {
            GameObject iconObject = new GameObject(interact.name + " icon");
            if (interact.gameObject.tag == "drivable")
            {
                iconObject = Instantiate(Icon, new Vector3(interact.transform.position.x, interact.transform.position.y + 5, interact.transform.position.z), Quaternion.identity);
            }
            else
            {
                iconObject = Instantiate(Icon, new Vector3(interact.transform.position.x, interact.transform.position.y + 3, interact.transform.position.z), Quaternion.identity);
            }
            iconObject.name = interact.name + " Icon";
            iconObject.transform.SetParent(interact.transform);
            iconObject.SetActive(false);
        }

            for (int i = 0; i < 3; i++)
        {
            if(i == _playerCtrl._currentObjective)
            {
                // Set next objective
                //_objectiveMarkers[i].SetActive(true);
            }
            else
            {
                //_objectiveMarkers[i].SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (GameObject interact in _interactPrefabs)
        {
            _origin = interact.transform.position;
            float dist = Vector3.Distance(_origin, _Mainchar.transform.position);
            if (dist <= _promptDist)
            {
                if (interact.gameObject.tag == "drivable")
                {
                    interact.transform.GetChild(10).gameObject.SetActive(true);
                    interact.transform.GetChild(10).LookAt(new Vector3(_Mainchar.transform.position.x, interact.transform.position.y, _Mainchar.transform.position.z));
                }
                else
                {
                    interact.transform.GetChild(0).gameObject.SetActive(true);
                    interact.transform.GetChild(0).LookAt(new Vector3(_Mainchar.transform.position.x, interact.transform.position.y, _Mainchar.transform.position.z));
                }
                _show = true;
              
            }

            else { interact.transform.GetChild(0).gameObject.SetActive(false); }
            
        }

        

        _origin = _interactPrefabs[10].transform.position;
        float objdist = Vector3.Distance(_origin, _Mainchar.transform.position);
        if (objdist <= _promptDist)
        {
            _showobj = true;
        }

        if ( !_playerCtrl._inVehicle)
        {
            if (_showobj)
            {
                //_interactprompt.text = "Press 'f' to start minigame!";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // win game
                    //Loader.LoadLevel("Winner");
                    
                    /*_playerCtrl.Save_Data();
                    if (_playerCtrl._currentObjective == 0)
                    {
                        SceneManager.LoadScene("Sniper Minigame");
                    }
                    else if (_playerCtrl._currentObjective == 1)
                    {
                        SceneManager.LoadScene("Redneck Rampage");
                    }
                    else if (_playerCtrl._currentObjective == 2)
                    {
                        SceneManager.LoadScene("Boss Battle Minigame");
                    }
                    else
                    {
                        //GameOver
                    }*/
                    
                }
            }
            else if (_show)
            {
                //_interactprompt.text = "Press 'e' to interact!";
            }
            else
            {
                //_interactprompt.text = "";
            }
        }
        
        _show = false;
        _showobj = false;
    }
}
