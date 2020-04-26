using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // Start is called before the first frame update
    void Start()
    {

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

        if(_show)
        {
            _interactprompt.text = "Press 'e' to interact!";
        }
        else
        {
            _interactprompt.text = "";
        }
        _show = false;
    }
}
