using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneManagement : MonoBehaviour
{
    private static SceneManagement _instance;
    public static SceneManagement Instance { get { return _instance; } }

    public string SceneNameToLoad;
    public string sceneName;

    public Text text;
    public GameObject uiCanvas;
    public GameObject loadCanvas;
    public GameObject loadCam;

    float loadProgress;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            //Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        sceneName = SceneManager.GetActiveScene().name;
        FindLoadScreenItems();
    }

    void FindLoadScreenItems()
    {
        if (uiCanvas == null)
            uiCanvas = GameObject.Find("GameUI");
        if (loadCanvas == null)
            loadCanvas = GameObject.Find("LoadSceneCanvas");
        if (loadCanvas != null)
            loadCanvas.SetActive(false);
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
        if(loadCanvas != null)
            loadCanvas.SetActive(true);
        if (loadCam != null)
            loadCam.SetActive(true);
        if(uiCanvas != null)
            uiCanvas.SetActive(false);
        StartCoroutine(LoadAsync(sceneName));
    }

    //public void LoadCredits()
    //{
    //    SceneManager.LoadScene("Credits");
    //}

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadAsync(string sceneIndex)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneIndex);

        while (!op.isDone)
        {
            loadProgress = Mathf.Clamp01(op.progress / .9f);
            text.text = loadProgress * 100f + "%";
            yield return null;
        }
    }
}

