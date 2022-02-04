using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    private static SceneManagement _instance;
    public static SceneManagement Instance { get { return _instance; } }

    public string SceneNameToLoad;
    public string sceneName;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        sceneName = SceneManager.GetActiveScene().name;
    }

    public void LoadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadCertainScene(string sceneName)
    {
        //SceneManager.LoadScene(sceneName);
        SceneManager.LoadScene(4);
        LoadManager.sceneInd = sceneName;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

