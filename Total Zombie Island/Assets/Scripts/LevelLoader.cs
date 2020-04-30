using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public GameObject transitionObject;

    public Animator transition;

    void Update()
    {
     
    }

    public void LoadLevel(string scene)
    {
        Debug.Log("Load Level");
        StartCoroutine(LoadingScreen(scene));
    }

    IEnumerator LoadingScreen(string scene)
    {
        transition.SetTrigger("ExitLoad");

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        asyncLoad.allowSceneActivation = false;

        // Wait until the asynchronous scene fully loads
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        asyncLoad.allowSceneActivation = true;
    }
}
