using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TimeManager : MonoBehaviour
{
    [SerializeField]
    public float timeSlowDownRate { get; private set; }
    public float timeSpeedUpRate { get; private set; }
    public float timeStopLength { get; private set; }
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
        }
    }

    /*
     * this is the method used for the time stop mechanic
     */
    void TimeStop()
    {
        Time.timeScale = 0;
    }
}
