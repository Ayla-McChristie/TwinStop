using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageFlash : IDamagable
{
    Renderer FlashRenderer { get; set; }
    Material HurtMat { get; }
    public float FlashDuration { get; set; }
    float FlashTimer { get; set; }
}
