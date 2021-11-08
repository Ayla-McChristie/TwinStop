using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

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

    private PlayerActionControls playerActionControls;

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
        freezeFire = false;

        //i think this is technical debt but i dunno
        playerActionControls = new PlayerActionControls();
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
        var input = playerActionControls.Player.Aim.ReadValue<Vector2>();
        Vector3 vNewInput = new Vector3(input.x, input.y, 0.0f);

        var angle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle, 0);

        //var aimPos = playerActionControls.Player.Aim.ReadValue<Vector2>();
        //var aimWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(aimPos.x, aimPos.y, player.transform.position.y));


        //direction = aimWorldPos - player.transform.position;
        //direction.y = 0;
        //transform.forward = direction;

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
        if (Input.GetMouseButton(0) && !coolDown && !freezeFire)
        {
            //var ray = cam.ScreenPointToRay(Input.mousePosition);
            //if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity))
            //{
            //    targetLoc = hitInfo.point;
            //    targetLoc.y = 0;
            //    targetLoc = targetLoc.normalized;
            //}
            direction = direction.normalized;
            var obj = ObjectPool_Projectiles.Instance.GetProjectile(bulletPool.prefab.name);
            obj.GetComponent<Projectile>().SetUp(direction, projectileStartPos.transform.position, this.gameObject.tag);
            coolDown = true;
        }
    }

    /// <summary>
    /// Freezes firing during transitions
    /// </summary>
    public void FrezeFire()
    {
        freezeFire = true;
    }
    /// <summary>
    /// UnFreezes firing during transitions
    /// </summary>
    public void UnFrezeFire()
    {
        freezeFire = false;
    }
    
    void SpreadShoot()
    {
        //float offset = (float)Random.Range(-maxSpread, maxSpread);

        Vector3 target = transform.forward + new Vector3(Random.Range(-spreadModifier, spreadModifier), 0, Random.Range(-spreadModifier, spreadModifier));

        var obj = ObjectPool_Projectiles.Instance.GetProjectile(bulletPool.prefab.name);
        obj.GetComponent<Projectile>().SetUp(target, projectileStartPos.transform.position, this.gameObject.tag);
        coolDown = true;
    }

    public void OnFire()
    {
        SpreadShoot();
    }
}
