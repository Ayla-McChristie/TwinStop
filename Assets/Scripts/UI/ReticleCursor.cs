using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ReticleCursor : MonoBehaviour
{
    [SerializeField]
    Texture2D reticle;
    float reticleAdjuster;
    bool isFiring;
    bool underScale;
    bool atScale;
    

    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(reticle, new Vector2(reticle.width/2, reticle.height/2), CursorMode.Auto);
    }

    public void GetSpreadModifier(float spreadMod, bool isfiring)
    {
        reticleAdjuster = spreadMod;
        isFiring = isfiring;
    }
}
