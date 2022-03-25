using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseScript : MonoBehaviour
{
    [SerializeField]
    GameObject PauseScreen;
    //[SerializeField]
    //SceneManagement sm;
    [SerializeField]
    private float timeSlowDownRate = .5f;
    [SerializeField]
    private float timeStopLength = 4;

    public bool isPaused;
    float fixedDeltaime;

    static PauseScript _instance;
    public static PauseScript Instance { get { return _instance; } }

    private void Awake()
    {
        /*
         * deletes this game object of one instance of time manager exists already -A
         */
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        if (PauseScreen == null)
            PauseScreen = GameObject.Find("PauseMenu");
        //if (sm == null)
        //    sm = GameObject.Find("SceneManagement").GetComponent<SceneManagement>();
    }

        // Start is called before the first frame update
    void Start()
    {
        PauseScreen.SetActive(false);
        isPaused = false;
        fixedDeltaime = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {

            //Time.timeScale = 0;
            isPaused = true;
            PauseScreen.SetActive(true);
            Debug.Log("hit");
            return;
        }
        if(Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            //Time.fixedDeltaTime = fixedDeltaime;
            isPaused = false;
            PauseScreen.SetActive(false);
        }
    }

    public void Resume()
    {
        isPaused = false;
        PauseScreen.SetActive(false);
    }

    public void LoadScene(string name)
    {
        SceneManagement.Instance.LoadCertainScene(name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
