using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hector_AI : MonoBehaviour
{
    [SerializeField]
    GameObject camObject;

    [SerializeField]
    string[] initialDialogue, obj1CompleteDialogue, obj2CompleteDialogue, obj3CompleteDialogue;

    int timesTriggered;

    Camera cam;

    public LevelLoader Loader;


    // Start is called before the first frame update
    void Start()
    {
        cam = camObject.GetComponent<Camera>();

        if (State_Data.Instance._setHector)
        {
            Debug.Log("tis true");
            State_Data.Instance._setHector = false;
            StartCoroutine(HectorDialogue());
        }
        else { camObject.SetActive(false); }
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(new Vector3(cam.transform.position.x, transform.position.y, cam.transform.position.z));
    }

    IEnumerator HectorDialogue()
    {
        State_Data.Instance._canBeHit = false;

        switch (timesTriggered)
        {
            case 0:
                // begginning of game dialogue
                break;

            case 1:
                // after you beat sniper minigame
                break;

            case 3:
                // after you beat redneck rampage
                break;

            case 4:
                // end sequence dialogue
                break;
        }
        //yield return new WaitForSeconds(5f);

        State_Data.Instance._canBeHit = true;

        Loader.LoadLevel("Overworld");
        yield return new WaitForSeconds(0.5f);
        camObject.SetActive(false);
    }
}
