using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TimeManager : MonoBehaviour
{
    [SerializeField]
    public float timeSlowDownRate = .5f;
    [SerializeField]
    public float timeSpeedUpRate = .5f;
    [SerializeField]
    public float timeStopLength = 2;
    private float defaultTimeScale;
    private float defaultFixedDeltaTime;

    /*
     * singleton to ensure we only have 1 time manager
     */
    private static TimeManager _instance;
    public static TimeManager Instance { get { return _instance; } }

    private void Awake()
    {
        /*
         * deletes this game object of one instance of time manager exists already
         */
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        defaultTimeScale = Time.timeScale;
        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale < 1f)
        {
            Time.timeScale += (1f / timeStopLength) * Time.unscaledDeltaTime;
            Time.fixedDeltaTime = Time.timeScale * .02f;
        }

        //Debug.Log(Time.timeScale.ToString());

        if (Input.GetKey(KeyCode.L))
        {
            TimeStop();
        }
    }

    /*
     * this is the method used for the time stop mechanic
     */
    void TimeStop()
    {
        //Debug.Log("Time has been stopped");
        Time.timeScale = .1f;
        
    }
}
