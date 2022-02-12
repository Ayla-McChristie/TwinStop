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
        loadCanvas.SetActive(true);
        loadCam.SetActive(true);
        if (uiCanvas != null)
            uiCanvas.SetActive(false);

        //SceneManager.LoadScene(sceneName);
        //SceneManager.LoadScene(4);
        //LoadManager.sceneInd = sceneName;
        StartCoroutine(LoadAsync(sceneName));
    }

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

