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

    Color defaultColor, newActiveColor, cantMoveColor, firingColor, timeStopColor;

    PlayerMovement pmScript;
    GunControl gcScript;

    bool isChangingColors;



    // Start is called before the first frame update
    void Start()
    {
        defaultColor = new Color(0, 15, 94);
        cantMoveColor = new Color(145, 145, 145);
        firingColor = new Color(173, 216, 230);
        timeStopColor = new Color(91, 84, 81);





        isChangingColors = false;
        myImage = this.GetComponent<Image>();
        //player = GameObject.FindGameObjectWithTag("Player");

        pmScript = player.GetComponent<PlayerMovement>();
        gcScript = player.GetComponent<GunControl>();

        rTransform = GetComponent<RectTransform>();
        //gameCam = Camera.Main;
        canvasRTransform = GameObject.Find("GameUI").GetComponent<RectTransform>();
        //gameCam = GameObject.FindGameObjectWithTag("MainCam");

        defaultColor = myImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        TrackPlayer();
        RotateWithPlayer();

        if (gcScript.isAttacking)
        {
            NewActiveColor(firingColor);
        }
        else
        {
            NewActiveColor(defaultColor);
        }

        if (isChangingColors)
        {
            ChangeColor();
        }

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
    void ChangeColor()
    {
        float newRed = newActiveColor.r - myImage.color.r;
        float newGreen = newActiveColor.g - myImage.color.g ;
        float newBlue = newActiveColor.b - myImage.color.b ;

        Color newColor = new Color(newRed, newGreen, newBlue);

        if (newColor == myImage.color)
        {
            isChangingColors = false;
            Debug.Log("NewColor = my color");
        }
        else
        {
           myImage.color = newColor;
        }
        
    }

    /// <summary>
    /// sets the new active color for the ring to change to
    /// </summary>
    /// <param name="red"></param>
    /// <param name="green"></param>
    /// <param name="blue"></param>
    void NewActiveColor(Color newColor)
    {
        newActiveColor = newColor;
        isChangingColors = true;
    }

    void ResetColor()
    {
        myImage.color = defaultColor;
    }
}
