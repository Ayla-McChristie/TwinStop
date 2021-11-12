using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRingFollow : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    RectTransform rTransform;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = rTransform.anchoredPosition;

        position.x = player.transform.position.x;
        position.y = player.transform.position.z;

        rTransform.anchoredPosition = position;

    }
}
