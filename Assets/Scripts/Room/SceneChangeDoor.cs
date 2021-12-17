using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeDoor : MonoBehaviour
{
    // Start is called before the first frame update

    //yes this is lazy i know sorry its late - Ryan
    public bool goToCredits;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            SceneManagement sceneScript = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManagement>();
            if (goToCredits)
            {
                sceneScript.LoadCertainScene("Credits");
            }
            else
            {
                
                sceneScript.LoadNextLevel();
            }
            
        }
    }
}
