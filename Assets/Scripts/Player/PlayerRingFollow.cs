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

    Image myImage;

    bool isChangingColors;



    // Start is called before the first frame update
    void Start()
    {
        isChangingColors = false;
        myImage = this.GetComponent<Image>();
        player = GameObject.FindGameObjectWithTag("Player");
        rTransform = GetComponent<RectTransform>();
        //gameCam = Camera.Main;
        canvasRTransform = GameObject.Find("GameUI").GetComponent<RectTransform>();
        //gameCam = GameObject.FindGameObjectWithTag("MainCam");
    }

    // Update is called once per frame
    void Update()
    {
        TrackPlayer();
        RotateWithPlayer();

    }

    /// <summary>
    /// Moves the ring's position to keep it on the player at all times
    /// </summary>
    void TrackPlayer()
    {
        //thank you this person https://gist.github.com/unitycoder/54f4be0324cccb649eff

        Vector2 ViewportPosition = gameCam.WorldToViewportPoint(player.transform.position);

        Vector2 WorldObject_ScreenPosition = new Vector2(
 ((ViewportPosition.x * canvasRTransform.sizeDelta.x) - (canvasRTransform.sizeDelta.x * 0.5f)),
 ((ViewportPosition.y * canvasRTransform.sizeDelta.y) - (canvasRTransform.sizeDelta.y * 0.5f)));

        rTransform.anchoredPosition = WorldObject_ScreenPosition;

    }
   
    /// <summary>
    /// Rotates the ring with the player
    /// </summary>
    void RotateWithPlayer()
    {
        //I cannot tell you why multiplying the rotation by -1 makes this work but it does. Rotating stuff in unity is hard -Ryan
        rTransform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, player.transform.eulerAngles.y * -1);
    }

    /// <summary>
    /// Dynamically changes the ring color gradually to the passed in rgb values
    /// </summary>
    /// <param name="red"></param>
    /// <param name="blue"></param>
    /// <param name="green"></param>
    void ChangeColor(float red, float green, float blue)
    {
        float newRed = myImage.color.r - red;
        float newGreen = myImage.color.g - green;
        float newBlue = myImage.color.b - blue;

        Color newColor = new Color(newRed, newGreen, newBlue);

        if (newColor == myImage.color)
        {
            isChangingColors = false;
        }
        else
        {
           myImage.color = newColor;
        }
        
    }
}
