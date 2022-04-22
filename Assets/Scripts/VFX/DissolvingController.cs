using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class DissolvingController : MonoBehaviour
{
    Animator anim;
    Material[] mat;
    [SerializeField]
    SkinnedMeshRenderer smr;
    float dissolveSpeed = 2f;
    int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (smr != null)
            mat = smr.materials;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.GetComponent<Enemy>().isDead)
            Dissolve();
    }

    void Dissolve()
    {
        for(int i = 0; i < mat.Length; i++)
        {
            mat[i].SetFloat("DissolveAmt", dissolveSpeed * Time.deltaTime);
        }
    }
}
