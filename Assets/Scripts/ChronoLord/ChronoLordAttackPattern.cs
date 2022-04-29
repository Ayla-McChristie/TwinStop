using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChronoLordAttackPattern : MonoBehaviour
{
    Animator MyAnimator;

    [SerializeField]
    GameObject[] Chargers, TestEnemies;

    GameObject[] waveToCheck;

    bool IsVulnerable, HasIncremented;

    int waveIndex;

    string BoolToSet;

    [SerializeField]
    float Health;

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
    public LayerMask mask;
    // Start is called before the first frame update
    void Start()
    {
        HasIncremented = false;
        MyAnimator = GetComponent<Animator>();
        IsVulnerable = false;
        waveIndex = 1;
        waveToCheck = Chargers;

        FlashTimer = 0;

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
        if (IsWaveFinished(waveToCheck) && !IsVulnerable && !HasIncremented)
        {
            MakeVulnerable();
            MyAnimator.SetBool(BoolToSet, true);
        }
        //Debug.Log("baba"+FlashTimer);
    }

    private void FixedUpdate()
    {
        FlashCoolDown();
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
        HasIncremented = true;
    }

    void MakeVulnerable()
    {
        HasIncremented = false;
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
                BoolToSet = "ChargerWaveEnd";
                waveToCheck = TestEnemies;
                break;
            case 2:
                waveToCheck = TestEnemies;
                BoolToSet = "TestEnemyWaveEnd";
                break;
        }
        waveIndex++;
        HasIncremented = true;
        Debug.Log("wave index" + waveIndex);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "PlayerBullet" && IsVulnerable)
        {
            Debug.Log("gaga");
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        Health--;
        FlashTimer = FlashDuration;

        if (CheckAmIDead())
        {
            Die();
        }
    }

    bool CheckAmIDead()
    {
        if (Health <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Die()
    {
        MyAnimator.SetBool("ImDead", true);
    }

    void Despawn()
    {
        this.gameObject.SetActive(false);
    }

}
