using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChronoLordAttackPattern : MonoBehaviour
{
    Animator MyAnimator;

    [SerializeField]
    GameObject[] Chargers, TestEnemies;

    GameObject[] waveToCheck;

    bool IsVulnerable;

    int waveIndex;

    string TriggerToSet;

    public Renderer FlashRenderer { get; set; }
    public Material hurtMat;
    public Material HurtMat { get => hurtMat; }
    protected Material defaultMat;
    public float flashDuration;

    [SerializeField]
    ChronoLordStatus MyChronoLordStatus;
    public float FlashDuration
    {
        get => flashDuration;
        set => flashDuration = value;
    }
    public float FlashTimer { get; set; }
    Renderer[] FlashRenderers;

    public bool hasChildrenRender;
    // Start is called before the first frame update
    void Start()
    {
        MyAnimator = GetComponent<Animator>();
        IsVulnerable = false;
        waveIndex = 1;
        waveToCheck = Chargers;

        FlashRenderer = GetComponent<Renderer>();
        if (hasChildrenRender)
        {
            FlashRenderers = this.transform.GetComponentsInChildren<Renderer>();
        }
        if (FlashRenderer == null || FlashRenderer.enabled == false)
        {
            FlashRenderer = this.GetComponentInChildren<Renderer>();
        }
        defaultMat = FlashRenderer.material;

        MyChronoLordStatus.AmFiring();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsWaveFinished(waveToCheck) && !IsVulnerable)
        {
            IncrementWave();
            MyAnimator.SetTrigger(TriggerToSet);
        }
    }
    void FlashCoolDown()
    {
        FlashTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(FlashTimer / FlashDuration);

        if (hasChildrenRender)
        {
            if (FlashTimer >= 0 && FlashRenderer.material != hurtMat)
            {
                foreach (Renderer r in FlashRenderers)
                    r.material = HurtMat;
            }
            else if (FlashTimer <= 0 && FlashRenderer.material != defaultMat)
            {
                foreach (Renderer r in FlashRenderers)
                    r.material = defaultMat;
            }
            return;
        }

        if (FlashTimer >= 0 && FlashRenderer.material != hurtMat)
        {
            FlashRenderer.material = HurtMat;
        }
        else if (FlashTimer <= 0 && FlashRenderer.material != defaultMat)
        {
            FlashRenderer.material = defaultMat;
        }
    }

    bool IsWaveFinished(GameObject[] enemiesInWave)
    {
        foreach (GameObject enemy in enemiesInWave)
        {
            if (enemy.activeInHierarchy)
            {
                return false;
            }
        }
        return true;
    }

    void MakeInvulnerable()
    {
        MyAnimator.SetBool("Vulnerable", false);
        IsVulnerable = false;
        MyChronoLordStatus.NotVulnerable();
        MyChronoLordStatus.AmFiring();
    }

    void MakeVulnerable()
    {
        MyAnimator.SetBool("Vulnerable", true);
        IsVulnerable = true;
        MyChronoLordStatus.AmVulnerable();
        MyChronoLordStatus.NotFiring();
    }

    void IncrementWave()
    {
        switch (waveIndex)
        {
            case 1: 
                TriggerToSet = "ChargerWaveEnd";
                waveToCheck = TestEnemies;
                break;
            case 2:
                waveToCheck = TestEnemies;
                TriggerToSet = "TestEnemyWaveEnd";
                break;
        }
        waveIndex++;
        MakeVulnerable();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            FlashTimer = FlashDuration;
        }
    }

}
