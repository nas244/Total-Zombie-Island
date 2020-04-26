using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warning_Prompt : MonoBehaviour
{
    [SerializeField]
    private GameObject _mainChar;

    [SerializeField]
    private Text _warningPrompt;

    [SerializeField]
    private float _warningDist = 190;

    [SerializeField]
    private float _blowupDist = 200;
    private bool _boom = false;

    [SerializeField]
    private GameObject _explosion;

    private Player_movement _playerCtrl;
    // Start is called before the first frame update
    void Start()
    {
        _playerCtrl = _mainChar.GetComponent<Player_movement>();
        _warningPrompt.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_mainChar.transform.position.x > _warningDist)
        {
            _warningPrompt.enabled = true;
        }
        else
        {
            _warningPrompt.enabled = false;
        }
        if(_mainChar.transform.position.x > _blowupDist && !_boom)
        {
            GameObject effect;

            effect = Instantiate(_explosion, _mainChar.transform);
            StartCoroutine(_playerCtrl.Damage(100));
            _boom = true;
            Destroy(effect, 1.0f);
        }
    }
}
