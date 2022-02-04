using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoadManager : MonoBehaviour
{
    public static string sceneInd;
    float loadProgress;
    public Text text;
    private void Start()
    {
        if(sceneInd == null)
        {
            sceneInd = "Tutorial";
        }
        StartCoroutine(LoadAsync(sceneInd));
        Debug.Log(sceneInd);
    }

    IEnumerator LoadAsync(string sceneIndex)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneIndex);

        while (!op.isDone)
        {
            loadProgress = Mathf.Clamp01(op.progress / .9f);
            text.text = loadProgress *100f + "%";
            yield return null;
        }
    }
}
