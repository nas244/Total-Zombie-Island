using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Interact_Prompt : MonoBehaviour
{
    [SerializeField]
    private GameObject _Mainchar;

    [SerializeField]
    private float _promptDist = 5.0f;

    private Vector3 _origin;

    [SerializeField]
    private Text _interactprompt;

    [SerializeField]
    private GameObject[] _interactPrefabs;

    private bool _show;
    private bool _showobj;

    private Player_movement _playerCtrl;

    [SerializeField]
    private GameObject[] _objectivePrefabs;
    [SerializeField]
    private GameObject[] _objectiveMarkers;



    // Start is called before the first frame update
    void Start()
    {
        _playerCtrl = _Mainchar.GetComponent<Player_movement>();

        for(int i = 0; i < 3; i++)
        {
            if(i == _playerCtrl._currentObjective)
            {
                _objectiveMarkers[i].SetActive(true);
            }
            else
            {
                _objectiveMarkers[i].SetActive(false);
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
                _show = true;
              
            }
            
        }

        

        _origin = _objectivePrefabs[_playerCtrl._currentObjective].transform.position;
        float objdist = Vector3.Distance(_origin, _Mainchar.transform.position);
        if (objdist <= _promptDist)
        {
            _showobj = true;
        }

        if ( !_playerCtrl._inVehicle)
        {
            if (_showobj)
            {
                _interactprompt.text = "Press 'f' to start minigame!";
                if (Input.GetKeyDown(KeyCode.F))
                {
                    _playerCtrl.Save_Data();
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
                    }
                    
                }
            }
            else if (_show)
            {
                _interactprompt.text = "Press 'e' to interact!";
            }
            else
            {
                _interactprompt.text = "";
            }
        }
        
        _show = false;
        _showobj = false;
    }
}
