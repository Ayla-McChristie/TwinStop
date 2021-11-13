using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeDoor : MonoBehaviour
{
    // Start is called before the first frame update
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
            sceneScript.LoadCertainScene(2);
        }
    }
}
