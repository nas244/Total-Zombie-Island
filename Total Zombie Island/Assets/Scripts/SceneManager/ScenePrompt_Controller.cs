using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenePrompt_Controller : MonoBehaviour
{
    [SerializeField]
    private Text _scenePrompt;
    // Start is called before the first frame update
    void Start()
    {
        _scenePrompt.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
