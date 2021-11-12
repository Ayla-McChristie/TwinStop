using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRingFollow : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    [SerializeField]
    Camera gameCam;
    RectTransform rTransform;
    [SerializeField]
     RectTransform canvasRTransform;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rTransform = GetComponent<RectTransform>();
        canvasRTransform = GameObject.Find("GameUI").GetComponent<RectTransform>();
        //gameCam = GameObject.FindGameObjectWithTag("MainCam");
    }

    // Update is called once per frame
    void Update()
    {
        TrackPlayer();

    }

    void TrackPlayer()
    {
        //Vector2 position = rTransform.TransformPoint(rTransform.anchoredPosition);

        //position.x = player.transform.position.x;
        //position.y = player.transform.position.z;

        //position.x += 6.5f * Time.deltaTime;
        //position.y += 6.5f * Time.deltaTime;


        //rTransform.anchoredPosition = position;

        Vector2 ViewportPosition = gameCam.WorldToViewportPoint(player.transform.position);

        Vector2 WorldObject_ScreenPosition = new Vector2(
 ((ViewportPosition.x * canvasRTransform.sizeDelta.x) - (canvasRTransform.sizeDelta.x * 0.5f)),
 ((ViewportPosition.y * canvasRTransform.sizeDelta.y) - (canvasRTransform.sizeDelta.y * 0.5f)));

        rTransform.anchoredPosition = WorldObject_ScreenPosition;

    }
}
