using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class TimeManager : MonoBehaviour
{
    [SerializeField]
    private float timeSlowDownRate = .5f;
    [SerializeField]
    private float timeSpeedUpRate = .5f;
    [SerializeField]
    private float timeStopLength = 4;
    [SerializeField]
    private float timeStopTimeScale = .1f;
    [SerializeField]
    private float MaxTimeValue = 4f;

    //REMOVE LATER - Just for showing off new Timestop
    //[SerializeField]
    //public Image timeStopReadyIndicator;
     
    //The timer variable for the time stop
    float timeValue;

    //The cooldown timer variable
    float coolDownValue;

    //Keeps track of when the timer times out, turns true when the timer runs out and false after
    //the cooldown
    bool outtaTime;

    //Turned true when player gets time crystal
    public bool hasTimeCrystal;

    //The Timestop PostProcessing Overlay

    public PostProcessingController ppController;

    private PlayerActionControls playerActionControls;

    //public float timeTillLength = 0; // this is used in EaseTimeToDefault() to run the while statement until it reaches timeStopLength;
    private float defaultTimeScale;
    private float defaultFixedDeltaTime;
    [SerializeField] public bool isTimeStopped = false;

    public TimeBar timeBar; // need this to be able to move the time bar. -Steve
    private AudioSource audio;
    /*
     * singleton to ensure we only have 1 time manager -A
     */
    private static TimeManager _instance;
    public static TimeManager Instance { get { return _instance; } }

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
        playerActionControls = new PlayerActionControls();

    }
    private void OnEnable()
    {
        playerActionControls.Enable();
    }
    private void OnDisable()
    {
        playerActionControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        defaultTimeScale = Time.timeScale;
        defaultFixedDeltaTime = Time.fixedDeltaTime;
        timeValue = MaxTimeValue; //This will (hopefully) give the player 4 seconds total of meter
        coolDownValue = MaxTimeValue;
        timeBar.SetMaxTime(MaxTimeValue); // passes the current max time value to make sure the bar has the same max -Steve
        outtaTime = false;
        audio = GetComponent<AudioSource>();

        //Debug.Log("Has time cystal:" + hasTimeCrystal);
        //TimeStopPPOveraly = GameObject.FindWithTag("TimeStopPP");      


        //GameObject.Find("Player" = this.GetComponent<PostProcessingController>();    

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(timeValue);
        //Debug.Log(coolDownValue);
        //Debug.Log(isTimeStopped);
        //Debug.Log(Time.timeScale);
        //TimeLeft();
        FreezeTime();

        Cooldown();

        EaseTimeToDefault();
        
        Timer();
        AmIOuttaTime();
        //Debug.Log(Time.timeScale);
        //Debug.Log(isTimeStopped);

        //REMOVE LATER -just for demonstration purposes -Ryan
        //if(outtaTime)
        //{
        //    //timeStopReadyIndicator.gameObject.SetActive(false);
        //}
        //else
        //{
        //    //timeStopReadyIndicator.gameObject.SetActive(true);
        //}  
    }
    /// <summary>
    /// Increments the timer for the time stop, decreasing while time stop is active and
    /// refilling it when it's not
    /// </summary>
    private void Timer()
    {
        if (isTimeStopped && !outtaTime && hasTimeCrystal)
        {
            timeValue -= Time.unscaledDeltaTime;

        }
        //else
        //{
        //    if (timeValue <= 0 && timeValue < .4f)
        //    {
        //        timeValue += Time.deltaTime;
        //    }
        //}
    }
    /*
         * Method to gradually reset time to 1. may need to rename -A
         */
    private void EaseTimeToDefault()
    {
        //while (Time.timeScale < 1f)
        //{
        //    Debug.Log("Time has started to revert back");
        //    Time.timeScale += (1f / timeStopLength) * Time.unscaledDeltaTime;
        //    Time.fixedDeltaTime = Time.timeScale * .02f;
        //}

        // original code do not delete!
        if (Time.timeScale < 1f)
        {
            //Debug.Log("Time has started to revert back");
            Time.timeScale += ((1f / timeStopLength) * Time.unscaledDeltaTime) * timeSpeedUpRate;
            Time.fixedDeltaTime = Time.timeScale * .02f;
        }
    }

    /// <summary>
    /// Checks on update whether or not the player has time meter left
    /// </summary>
    void AmIOuttaTime()
    {
        if (timeValue <= 0)
        {
            if (!outtaTime)
            {
                coolDownValue = 0;
            }
            
            outtaTime = true;

        }
        if (coolDownValue >= MaxTimeValue)
        {
            if (outtaTime)
            {
                timeValue = MaxTimeValue;
            }
            outtaTime = false;
        }
    }

    /*
     * this is the method used for the time stop mechanic -A
     */
    void TimeStop()
    {
        //Debug.Log("Time has been stopped");
        if (!outtaTime && Time.timeScale > timeStopTimeScale && isTimeStopped && hasTimeCrystal)
        {

            Time.timeScale -= ((1f / timeStopLength) * Time.unscaledDeltaTime) * timeSlowDownRate;
            Time.fixedDeltaTime = Time.timeScale * .02f;
        }
        
    }

    /// <summary>
    /// Adds to the cool down
    /// </summary>
    void Cooldown()
    {
        if (coolDownValue < MaxTimeValue && outtaTime)
        {
            ppController.timeStopOn = false;
            coolDownValue += Time.unscaledDeltaTime;
        }
    }

    void FreezeTime()
    {
        if (isTimeStopped && hasTimeCrystal)
        {
            ppController.timeStopOn = true;
            TimeStop();
            timeBar.TimeSet(timeValue);
        }
        else
        {
            //TimeStop();
            ppController.timeStopOn = false;
            timeBar.TimeSet(timeValue);
            //}
            //if (!Input.GetKeyDown(KeyCode.LeftShift) && !outtaTime)
            //{
            //    isTimeStopped = false;
            //}

            //// code to undo the time pause if activated
            //if (Input.GetKeyDown(KeyCode.E) && isTimeStopped == true)
            //{
            //    isTimeStopped = false;
            //    EaseTimeToDefault();
            //}
        }
    }

    public void OnTimeStop(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isTimeStopped = true;
            audio.Play();
        }
        if (context.canceled)
            isTimeStopped = false;
    }
}
