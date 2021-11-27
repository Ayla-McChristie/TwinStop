using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleCursor : MonoBehaviour
{
    [SerializeField]
    Camera cam;
    Vector3 regScale;
    Vector3 reticlePos;
    float reticleAdjuster;
    bool isFiring;
    bool underScale;
    bool atScale;

    RectTransform rectT;
    RectTransform canvasRTransform;
    // Start is called before the first frame update
    void Start()
    {
        canvasRTransform = GameObject.Find("GameUI").GetComponent<RectTransform>();
        regScale = this.transform.localScale;
        rectT = GetComponent<RectTransform>();
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.position = Input.mousePosition;
        //ReticlePosition();
        Vector3 point = cam.ScreenToViewportPoint(Input.mousePosition);
        rectT.anchoredPosition = (Vector2)Input.mousePosition - this.rectT.anchoredPosition;
        AdjustReticleSize();
        //rectT.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 1);
    }

    void ReticlePosition()
    {
        Vector2 worldPos = cam.ViewportToScreenPoint(Input.mousePosition);

        Vector2 WorldObject_ScreenPosition = new Vector2(
         ((worldPos.x * canvasRTransform.sizeDelta.x) - (canvasRTransform.sizeDelta.x * 0.5f)),
         ((worldPos.y * canvasRTransform.sizeDelta.y) - (canvasRTransform.sizeDelta.y * 0.5f)));
        rectT.anchoredPosition = worldPos;
    }

    void AdjustReticleSize()
    {
        if (isFiring && underScale)
            this.transform.localScale += new Vector3(reticleAdjuster, reticleAdjuster, 0);
        else if (isFiring && !underScale)
            this.transform.localScale = this.transform.localScale;
        else
            this.transform.localScale = regScale;

        if (this.transform.localScale.x <= 1.5f && this.transform.localScale.y <= 1.5f)
            underScale = true;
        else
            underScale = false;
    }

    public void GetSpreadModifier(float spreadMod, bool isfiring)
    {
        reticleAdjuster = spreadMod;
        isFiring = isfiring;
    }
}
