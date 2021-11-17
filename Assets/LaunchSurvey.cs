using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchSurvey : MonoBehaviour
{
    [SerializeField] private string sceneName;

    void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.tag == "Player")
        {
            Application.OpenURL("https://forms.gle/Svz3TX6UAY3enQBs5");
            Debug.Log("this worked");
            other.gameObject.GetComponent<PlayerStats>().TakeDamage(3);

            SceneManagement sceneScript = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManagement>();
            sceneScript.LoadCertainScene(sceneName);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Application.OpenURL("https://forms.gle/Svz3TX6UAY3enQBs5");
            Debug.Log("this worked");
            other.gameObject.GetComponent<PlayerStats>().TakeDamage(3);

            SceneManagement sceneScript = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManagement>();
            sceneScript.LoadCertainScene(sceneName);
        }
    }
}
