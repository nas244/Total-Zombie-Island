using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScenePrompt_Controller : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _scenePrompt;
    [SerializeField]
    private GameObject _mainChar;

    private Player_movement _playerCtrl;
    // Start is called before the first frame update
    void Start()
    {
        _playerCtrl = _mainChar.GetComponent<Player_movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_playerCtrl._currentObjective == 0)
        {
            _scenePrompt.text = "Look for the old man near the hospital to get started!";
        }
        else if (_playerCtrl._currentObjective == 1)
        {
            _scenePrompt.text = "Look for the football player in the trailer park on the edge of the city and help thin the herds.";
        }
        else if(_playerCtrl._currentObjective == 2)
        {
            _scenePrompt.text = "You've handled zombies so well it's time for a new enemy. Look in the Police Headquarters.";
        }
        else
        {
            _scenePrompt.text = "Well Done! Pickup will be waiting at the Lighthouse!";
        }
    }
}
