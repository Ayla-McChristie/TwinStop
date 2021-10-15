using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControl : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 2f)]
    float fireRate = .5f;

    Camera cam;
    GameObject projectileStartPos;
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
        projectileStartPos = this.gameObject.transform.GetChild(0).gameObject;
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
        //Ray cameraRay = cam.ScreenPointToRay(Input.mousePosition);
        //Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        //float rayLength; // Length of line from Camera to nearest ground

        //if (groundPlane.Raycast(cameraRay, out rayLength))
        //{
        //    Vector3 pointToLook = cameraRay.GetPoint(rayLength) - this.transform.position;
        //    pointToLook = new Vector3(pointToLook.x, this.transform.position.y, pointToLook.z);
        //    Debug.DrawLine(cameraRay.origin, pointToLook, Color.green);
        //    this.transform.LookAt(pointToLook);
        //    pointToLook.y = 0;
        //    direction = pointToLook;

        //    //var rotation = Quaternion.LookRotation(pointToLook);
        //    //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotSpeed*Time.deltaTime);

        //}
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
            //var ray = cam.ScreenPointToRay(Input.mousePosition);
            //if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity))
            //{
            //    targetLoc = hitInfo.point;
            //    targetLoc.y = 0;
            //    targetLoc = targetLoc.normalized;
            //}
            direction = direction.normalized;
            obj = opP.GetProjectile(projectileType);
            obj.GetComponent<Projectile>().SetUp(direction, projectileStartPos.transform.position, this.gameObject.tag);
            coolDown = true;
        }
    }
}
