using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WINNER : MonoBehaviour
{
    public float winnertimestart = 5;
    public Text winnercountdown;
    public GameObject Character;
    public GameObject music;
    public Animator Win;
    public Animator Wintext;
    public float Wait;

    public LevelLoader Loader;

    // Start is called before the first frame update
    void Start()
    {
        winnercountdown.text = winnertimestart.ToString();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {

        winnertimestart -= Time.deltaTime;
        winnercountdown.text = Mathf.Round(winnertimestart).ToString();


        if (winnertimestart <= 0)
        {
            winnertimestart = 0;
            music.gameObject.SetActive(true);
            Win.SetInteger("Animation_int", 4);
            Wintext.SetTrigger("begin");



        }
    }

    public void BackToStartScreen()
    {
        Loader.LoadLevel("Start");
    }
}
