using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchSurvey : MonoBehaviour
{
    void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.tag == "Player")
        {
            Application.OpenURL("https://forms.gle/kBV7V9E4rwAWQdBq9");
            Debug.Log("this worked");
            other.gameObject.GetComponent<PlayerStats>().TakeDamage(3);

            SceneManagement sceneScript = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManagement>();
            sceneScript.LoadCertainScene(0);
        }
    }

    void OnTriggerEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            Application.OpenURL("https://forms.gle/kBV7V9E4rwAWQdBq9");
            Debug.Log("this worked");
            other.gameObject.GetComponent<PlayerStats>().TakeDamage(3);

            SceneManagement sceneScript = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManagement>();
            sceneScript.LoadCertainScene(0);
        }
    }
}
