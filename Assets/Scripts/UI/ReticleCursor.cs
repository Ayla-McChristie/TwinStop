using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ReticleCursor : MonoBehaviour
{
    [SerializeField]
    Texture2D reticle;
    [SerializeField]
    Image newReticle;
    [SerializeField]
    Image timeMeter;
    [SerializeField]
    Canvas gameCan;
    float reticleAdjuster;
    float timeBarMax;
    float timeBarVal;
    bool isFiring;
    bool underScale;
    bool atScale;
    GunControl gun;
    Vector2 pos;

    static ReticleCursor instance;
    public static ReticleCursor _instance { get { return instance; } }

    private void Awake()
    {
        /*
         * deletes this game object of one instance of time manager exists already -A
         */
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

        // Start is called before the first frame update
        void Start()
    {
        //reticleTimeMeter.rectTransform.sizeDelta = new Vector3(reticleTimeMeter.sprite.rect.width * 2, reticleTimeMeter.sprite.rect.height * 2);
        Cursor.SetCursor(reticle, new Vector2(reticle.width/2, reticle.height/2), CursorMode.Auto);
        Cursor.visible = false;
        gun = GameObject.FindGameObjectWithTag("Player").GetComponent<GunControl>();
    }

    private void Update()
    {
        TimeMeterFill();
        DecreaseMeter();
        RegenMeter();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(gameCan.transform as RectTransform, Input.mousePosition, gameCan.worldCamera, out pos);
        newReticle.transform.position = gameCan.transform.TransformPoint(pos);
        timeMeter.transform.position = gameCan.transform.TransformPoint(pos);
        if (PauseScript.Instance.isPaused)
        {
            Cursor.visible = true;
            newReticle.enabled = false;
            timeMeter.enabled = false;
            return;
        }
        Cursor.visible = false;
        newReticle.enabled = true;
        if (!TimeManager.Instance.hasTimeCrystal)
        {
            timeMeter.enabled = false;
            return;
        }
        timeMeter.enabled = true;
    }
    public void GetSpreadModifier(float spreadMod, bool isfiring)
    {
        reticleAdjuster = spreadMod;
        isFiring = isfiring;
    }

    public void TimeCost(float cost)
    {
        timeMeter.fillAmount -= cost * 0.65f;
    }

    void TimeMeterFill()
    {
        timeMeter.fillAmount = Mathf.Lerp(timeMeter.fillAmount, timeBarVal / timeBarMax, 9f * Time.deltaTime);
    }

    public void SetMaxTime(float time)
    {
        timeBarMax = time;
        timeBarVal = time;
    }
    
    public void DecreaseMeter()
    {
        if (TimeManager.Instance.isTimeStopped && timeBarVal > 0 && !TimeManager.Instance.outtaTime)
            timeBarVal -= Time.unscaledDeltaTime * 1.75f;
    }

    public void RegenMeter()
    {
        if ((!TimeManager.Instance.isTimeStopped && timeBarVal < timeBarMax) || TimeManager.Instance.outtaTime && timeBarVal < timeBarMax)
            timeBarVal += Time.unscaledDeltaTime * 1.615f;
    }
}
