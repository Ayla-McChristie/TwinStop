using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShield : MonoBehaviour
{
    Renderer renderer;
    [SerializeField]
    AnimationCurve displacementCurve;
    [SerializeField]
    float displacementMagnitude;
    [SerializeField]
    float lerpSpeed;
    [SerializeField]
    float dissolveSpeed;
    [SerializeField]
    bool isTurtle;
    [SerializeField]
    bool isGargoyle;
    [SerializeField]
    bool isChronoLord;

    float dissolveVal;
    float timeSlowTarget;
    float timeNormTarget;
    bool shieldOn;
    Collider collider;
    Coroutine disolveCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        collider = GetComponent<Collider>();Debug.Log(renderer.name);
        if (isTurtle)
        {
            timeNormTarget = 1f;
            timeSlowTarget = -3f;
            collider.enabled = false;
            renderer.material.SetFloat("Dissolve", 1f);
        }
        if (isGargoyle)
        {
            timeNormTarget = -3f;
            timeSlowTarget = 1f;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (TimeManager.Instance.isTimeStopped)
        {
            if (isTurtle)
                collider.enabled = true;
            if (isGargoyle)
                collider.enabled = false;
            DissolveShield(timeSlowTarget);
            shieldOn = false;
            return;
        }
        if (!shieldOn)
        {
            if (isTurtle)
                collider.enabled = false;
            if (isGargoyle)
                collider.enabled = true;
            DissolveShield(timeNormTarget);
            shieldOn = true;
        }

    }

    void TurtleSetUp()
    {
        if (TimeManager.Instance.isTimeStopped)
        {
            if (isTurtle)
                collider.enabled = true;
            if (isGargoyle)
                collider.enabled = false;
            DissolveShield(-3f);
            shieldOn = false;
            return;
        }
        if (!shieldOn)
        {
            if (isTurtle)
                collider.enabled = false;
            if (isGargoyle)
                collider.enabled = true;
            DissolveShield(-3f);
            shieldOn = true;
        }
    }

    void GargoyleSetUp()
    {
        if (TimeManager.Instance.isTimeStopped)
        {
            DissolveShield(1f);
            shieldOn = false;
            return;
        }
        if (!shieldOn)
        {
            DissolveShield(-3f);
            shieldOn = true;
        }
    }

    void DissolveShield(float target)
    {
        if (disolveCoroutine != null)
            StopCoroutine(disolveCoroutine);
        disolveCoroutine = StartCoroutine(Coroutine_DissolveShield(target));
    }


    public void HitShield(Vector3 hitPos)
    {
        this.renderer.material.SetVector("HitPos",hitPos);
        StopAllCoroutines();
        StartCoroutine(Coroutine_HitDisplacement());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "PlayerBullet")
            HitShield(collision.transform.position);
    }

    IEnumerator Coroutine_HitDisplacement()
    {
        float lerp = 0;
        while(lerp < 1)
        {
            this.renderer.material.SetFloat("DisplacementStr", displacementCurve.Evaluate(lerp) * displacementMagnitude);
            lerp += Time.deltaTime * lerpSpeed;
            yield return null;
        }
    }

    IEnumerator Coroutine_DissolveShield(float target)
    {
        dissolveVal = 0;
        while (dissolveVal < 1)
        {
            renderer.material.SetFloat("Dissolve", Mathf.Lerp(renderer.material.GetFloat("Dissolve"), target, dissolveVal));
            dissolveVal += Time.unscaledDeltaTime * .5f;
            yield return null;
        }
    }
}
