using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRingFollow : MonoBehaviour
{
    [SerializeField]
    GameObject player, timeManagerObject;
    [SerializeField]
    Camera gameCam;
    RectTransform rTransform;
    [SerializeField]
     RectTransform canvasRTransform;

    //Image myImage;
    Material mat;
    [ColorUsage(true, true)]
    [SerializeField]
    Color defaultColor, cantMoveColor, firingColor, timeStopColor;
    Color newActiveColor;
    PlayerMovement pmScript;
    GunControl gcScript;
    TimeManager tmScript;

    bool isChangingColors;



    // Start is called before the first frame update
    void Start()
    {
        //defaultColor = new Color(0, 15, 94);
        //cantMoveColor = new Color(145, 145, 145);
        //firingColor = new Color(0, 91, 90);
        //timeStopColor = new Color(1, 147, 1);

        isChangingColors = false;
        //myImage = this.GetComponent<Image>();
        mat = this.GetComponent<MeshRenderer>().material;
        //player = GameObject.FindGameObjectWithTag("Player");
        timeManagerObject = GameObject.FindGameObjectWithTag("TimeManager");

        if (player == null)
            player = GameObject.Find("Player_2.0");

        pmScript = player.GetComponent<PlayerMovement>();
        gcScript = player.GetComponent<GunControl>();
        tmScript = timeManagerObject.GetComponent<TimeManager>();

        //rTransform = GetComponent<RectTransform>();
        //gameCam = Camera.Main;
        //canvasRTransform = GameObject.Find("GameUI").GetComponent<RectTransform>();
        //gameCam = GameObject.FindGameObjectWithTag("MainCam");

        defaultColor = mat.color;
    }

    // Update is called once per frame
    void Update()
    {
        //TrackPlayer();
        //RotateWithPlayer();

        ResetColor();
        if (gcScript.isAttacking)
        {
            NewActiveColor(firingColor);
        }
        if (pmScript.freezeMovement)
        {
            NewActiveColor(cantMoveColor);
        }
        if(tmScript.isTimeStopped)
        {
            NewActiveColor(timeStopColor);
        }

        Debug.Log("Is time stopped: " + tmScript.ToString());
        

        if (isChangingColors)
        {
            ChangeColor();
        }
        else
        {
            ResetColor();
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
        if (Color.Equals(mat.color, newActiveColor))
        {
            isChangingColors = false;
        }
        else
        {
            mat.color = Color.Lerp(mat.color, newActiveColor, .2f); ;
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
        mat.color = Color.Lerp(mat.color, defaultColor, .2f);
        isChangingColors = false;
    }
}
