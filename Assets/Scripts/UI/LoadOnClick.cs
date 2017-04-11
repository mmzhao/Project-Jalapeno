using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOnClick : MonoBehaviour {

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex); 
    }

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Quit() {
        Application.Quit();
    }

}
