using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageFlash : IDamagable
{
    SkinnedMeshRenderer FlashRenderer { get; set; }
    public float FlashIntensity { get; set; }
    public float FlashDuration { get; set; }
    float FlashTimer { get; set; }
}
