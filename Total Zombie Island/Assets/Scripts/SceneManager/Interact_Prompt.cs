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



    // Start is called before the first frame update
    void Start()
    {
        _playerCtrl = _Mainchar.GetComponent<Player_movement>();
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
        Debug.Log(objdist);
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
                    SceneManager.LoadScene("Sniping");
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
