using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControl : MonoBehaviour
{
    [SerializeField]
    [Range(1,3)]
    int fireRate;

    public Camera cam;

    Vector3 direction;
    Vector3 mousePos;
    Vector3 targetLoc;

    GameObject obj;
    public GameObject projectileStartPos;
    public ObjectPool_Bullets obP;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Aim();
        Shoot();
    }

    void Aim()
    {
        direction = GetMousePos() - transform.position;
        direction.y = 0;
        transform.forward = direction;
    }

    Vector3 GetMousePos()
    {
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity))
        {
            return hitInfo.point;
        }
        else
            return Vector3.zero;
    }

    void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity))
            {
                targetLoc = hitInfo.point;
                targetLoc.y = 0;
                targetLoc = targetLoc.normalized;
            }
            obj = obP.GetProjectile(this.gameObject.tag);
            obj.GetComponent<MagicBullet>().SetUp(targetLoc, projectileStartPos.transform.position, this.gameObject.tag);
        }
    }
}
