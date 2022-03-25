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
    [SerializeField]
    private float TimeRecoveryRate = 1.5f;

    //REMOVE LATER - Just for showing off new Timestop
    //[SerializeField]
    //public Image timeStopReadyIndicator;
     
    //The timer variable for the time stop
    float timeValue;

    //The cooldown timer variable
    float coolDownValue;

    //Keeps track of when the timer times out, turns true when the timer runs out and false after
    //the cooldown
    public bool outtaTime;

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
    private AudioSource[] audio;
    AudioSource slowTimeEnter;
    AudioSource slowTimeExit;
    bool timeEnterIsPlayed;
    bool timeExitIsPlayed;

    PlayerStats pStats;
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
        pStats = GameObject.Find("Player_2.0").GetComponent<PlayerStats>();
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
        ReticleCursor._instance.SetMaxTime(MaxTimeValue);
        outtaTime = false;
        audio = GetComponents<AudioSource>();
        slowTimeEnter = audio[0];
        slowTimeExit = audio[1];
        timeEnterIsPlayed = true;
        timeExitIsPlayed = true;
        //Debug.Log("Has time cystal:" + hasTimeCrystal);
        //TimeStopPPOveraly = GameObject.FindWithTag("TimeStopPP");      


        //GameObject.Find("Player" = this.GetComponent<PostProcessingController>();    

    }

    // Update is called once per frame
    void Update()
    {
        if (PauseScript.Instance.isPaused && Input.GetKey(KeyCode.Escape) && !Input.GetKeyDown(KeyCode.LeftShift))
        {
            isTimeStopped = false;Debug.Log("HI");
            return;
        }
        if (PauseScript.Instance.isPaused && Input.GetKey(KeyCode.Escape) && Input.GetKey(KeyCode.LeftShift))
        {
            isTimeStopped = true;
            return;
        }
        if (PauseScript.Instance.isPaused)
            return;
        FreezeTime();

        Cooldown();

        EaseTimeToDefault();
        //Debug.Log(timeValue);
        Timer();
        AmIOuttaTime();
        if (outtaTime)
        {
            timeBar.TimeSet(coolDownValue);
        }
        else
        {
            timeBar.TimeSet(timeValue);
        }
        SetTimeSlow();
    }

    void SetTimeSlow()
    {
        if(isTimeStopped && hasTimeCrystal)
            this.GetComponentInParent<GunControl>().GetTimeSlow(true);
        else if (!isTimeStopped)
            this.GetComponentInParent<GunControl>().GetTimeSlow(false);
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
            
            PlaySlowTimeExit();
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

    public void HardTimeReset()
    {
        //hasTimeCrystal = false;
        Time.timeScale = 1f;
    }

    public void ReturnToNormalAfterTransition()
    {
        //hasTimeCrystal = true;
        isTimeStopped = false;
    }

    /*
     * this is the method used for the time stop mechanic -A
     */
    void TimeStop()
    {
        //Debug.Log("Time has been stopped");
        if (!outtaTime && Time.timeScale > timeStopTimeScale && isTimeStopped && hasTimeCrystal)
        {           
            Time.timeScale = Mathf.Clamp01(Time.timeScale - ((1f / timeStopLength) * Time.unscaledDeltaTime) * timeSlowDownRate);
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
            coolDownValue += Time.unscaledDeltaTime/(TimeRecoveryRate/2f);
        }
    }

    void FreezeTime()
    {
        if (isTimeStopped && hasTimeCrystal)
        {
            ppController.timeStopOn = true;
            TimeStop();
            PlaySlowTimeEnter();
        }
        else 
        {
            //TimeStop();
            PlaySlowTimeExit();
            ppController.timeStopOn = false;
            if (timeValue < MaxTimeValue)
            {
                this.timeValue += Time.unscaledDeltaTime/TimeRecoveryRate;
            }
        }
    }
    public void OnTimeStop(InputAction.CallbackContext context)
    {
        if (pStats.isDead)
            return;

        if (PauseScript.Instance.isPaused)
            return;
        if (hasTimeCrystal)
        {
            if (context.performed)
            {
                isTimeStopped = true;
            }
            if (context.canceled && hasTimeCrystal)
            {
                timeEnterIsPlayed = false;
                isTimeStopped = false;
                timeExitIsPlayed = false;
            }
        }
    }

    void PlaySlowTimeEnter()
    {
        if(slowTimeEnter != null)
        {
            if (!slowTimeEnter.isPlaying && !timeEnterIsPlayed)
            {
                AudioManager.Instance.PlaySound("TimeStop", this.transform.position, true);
                timeEnterIsPlayed = true;
            }
        }
    }

    void PlaySlowTimeExit()
    {
        if(slowTimeExit != null)
        {
            if (!slowTimeExit.isPlaying && !timeExitIsPlayed)
            {
                AudioManager.Instance.PlaySound("ReverseTimeStop", this.transform.position, true);
                timeExitIsPlayed = true;
            }
        }
    }
}
