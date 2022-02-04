using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ScreenFade : MonoBehaviour
{
    Animator anim;
    AnimationClip[] clip;
    AnimationClip _clip;
    GameObject player;
    bool playerDead;
    float animTimer;
    void Start()
    {
        anim = this.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        clip = anim.runtimeAnimatorController.animationClips;
        _clip = clip[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (playerDead)
        {
            anim.SetBool("FadeOut", true);
            animTimer += Time.deltaTime;
            if (animTimer >= _clip.length)
                FindObjectOfType<SceneManagement>().LoadCertainScene(SceneManagement.Instance.sceneName);
        }
    }

    public void ChangeScene(bool isPlayerDead)
    {
        playerDead = isPlayerDead;
    }
}
