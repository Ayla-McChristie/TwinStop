using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControl : MonoBehaviour
{
    [SerializeField]
    [Range(0f,2f)]
    float fireRate;

    public Camera cam;
    public GameObject projectileStartPos;
    public ObjectPool_Projectiles opP;

    Vector3 direction;
    Vector3 mousePos;
    Vector3 targetLoc;

    GameObject obj;

    string projectileType;
    bool coolDown;
    float fireTimer;
    void Start()
    {
        projectileType = "PlayerBullet";
        cam = Camera.main;
        coolDown = false;
        fireTimer = 0;
    }

    void Update()
    {
        Aim();
        Shoot();

        if (coolDown)
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireRate)
            {
                fireTimer = 0;
                coolDown = false;
            }
        }
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
        if (Input.GetMouseButtonDown(0) && !coolDown)
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity))
            {
                targetLoc = hitInfo.point;
                targetLoc.y = 0;
                targetLoc = targetLoc.normalized;
            }
            obj = opP.GetProjectile(projectileType);
            obj.GetComponent<Projectile>().SetUp(targetLoc, projectileStartPos.transform.position, this.gameObject.tag);
            coolDown = true;
        }
    }
}
