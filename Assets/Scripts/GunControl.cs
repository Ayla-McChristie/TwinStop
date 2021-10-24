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
    GameObject player; 

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
        player = this.gameObject;
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
        mousePos = Input.mousePosition;
         
        mousePos.z = Vector3.Distance(Camera.main.transform.position, player.transform.position); //The distance between the camera and object
        Vector3 objectPos = Camera.main.WorldToScreenPoint(player.transform.position);
        mousePos.x -= player.transform.position.x;
        mousePos.y -= player.transform.position.y;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        //direction = GetMousePos() - player.transform.position;
        //transform.forward = direction;
        //player.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);


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
        mousePos = Input.mousePosition;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, player.transform.position.y));

        return mouseWorld;
        //var ray = cam.ScreenPointToRay(Input.mousePosition);
        //if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity))
        //{
        //    return hitInfo.point;
        //}
        //else
        //    return Vector3.zero;
    }

    void Shoot()
    {
        if (Input.GetMouseButton(0) && !coolDown)
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
