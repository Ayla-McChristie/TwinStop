using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControl : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 2f)]
    float fireRate = .5f;
    [SerializeField]
    [Tooltip("Changes how far the bullets are spread. 0 means no spread")]
    [Range(0f, 1f)]
    float spreadModifier = .2f;

    //this is where the bullet spawns
    GameObject projectileStartPos;
    //this is a shortcut to the parent object but i probably dont need this
    GameObject player;

    [SerializeField]
    Pool bulletPool;

    Vector3 direction;
    Vector3 mousePos;
    Vector3 targetLoc;

    GameObject obj;

    bool coolDown;
    float fireTimer;

    //Turned on during special scenes like door transitions
    bool freezeFire;
    void Start()
    {
        player = this.gameObject;
        ObjectPool_Projectiles.Instance.InstantiatePool(bulletPool);

        projectileStartPos = this.gameObject.transform.GetChild(0).gameObject;

        coolDown = false;
        fireTimer = 0;
    }

    void Update()
    {
        Aim();
        //Shoot();
        SpreadShoot();

        if (coolDown)
        {
            //this means our shooting cooldown is affected by time slow
            fireTimer += Time.deltaTime;
            //this could be fun maybe? just need to get the bullets to not explode on one another
            //fireTimer += Time.unscaledDeltaTime;

            if (fireTimer >= fireRate)
            {
                fireTimer = 0;
                coolDown = false;
            }
        }
    }

    void Aim()
    {

        direction = GetMousePos() - player.transform.position;
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
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity))
        {
            return hitInfo.point;
        }
        else
            return Vector3.zero;
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
            var obj = ObjectPool_Projectiles.Instance.GetProjectile(bulletPool.name);
            obj.GetComponent<Projectile>().SetUp(direction, projectileStartPos.transform.position, this.gameObject.tag);
            coolDown = true;
        }
    }

    void SpreadShoot()
    {
        if (Input.GetMouseButton(0) && !coolDown)
        {
            //float offset = (float)Random.Range(-maxSpread, maxSpread);

            Vector3 target = transform.forward + new Vector3(Random.Range(-spreadModifier, spreadModifier), 0, Random.Range(-spreadModifier, spreadModifier));

            var obj = ObjectPool_Projectiles.Instance.GetProjectile(bulletPool.name);
            obj.GetComponent<Projectile>().SetUp(target, projectileStartPos.transform.position, this.gameObject.tag);
            coolDown = true;
        }
    }
}
