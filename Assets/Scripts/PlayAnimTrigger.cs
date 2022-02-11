using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayAnimTrigger : MonoBehaviour
{
    [SerializeField]
    List<PlayableDirector> directors;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < directors.Count; i++)
        {
            directors[i].Play();
        }
    }
}
