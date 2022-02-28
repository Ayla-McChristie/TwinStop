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
    [SerializeField]
    private float controllerDeadzone = 0.1f;
    [SerializeField]
    float rotationSmoothing = 1000f;

    public float SpreadModifier { get { return spreadModifier; } }
    //this is where the bullet spawns
    GameObject projectileStartPos;
    //this is a shortcut to the parent object but i probably dont need this
    GameObject player;

    private PlayerActionControls playerActionControls;
    private PlayerInput playerInput;

    [SerializeField]
    Pool bulletPool;

    Vector3 direction;
    Vector3 mousePos;
    Vector3 targetLoc;

    Animator anim;
    GameObject obj;
    GameObject uiCursor;
    bool coolDown;
    float fireTimer;
    float fireRateModifier;
    bool isGamepad;

    public bool isAttacking = false;
    private void Awake()
    {
        playerActionControls = new PlayerActionControls();
    }

    private void OnEnable()
    {
        playerActionControls.Enable();
    }

    private void OnDisable()
    {
        playerActionControls.Disable();
    }
    //Turned on during special scenes like door transitions
    bool freezeFire;
    void Start()
    {
        player = this.gameObject;
        ObjectPool_Projectiles.Instance.InstantiatePool(bulletPool);
        uiCursor = GameObject.Find("Cursor");
        projectileStartPos = this.gameObject.transform.GetChild(0).gameObject;
        fireRateModifier = 1f;
        coolDown = false;
        fireTimer = 0;
        freezeFire = false;
        anim = this.GetComponent<Animator>();
    }

    void Update()
    {
        if (!freezeFire)
        {
            Aim();
            //Shoot();
            SpreadShoot();
        }

        if (coolDown)
        {
            //this means our shooting cooldown is affected by time slow
            fireTimer += Time.unscaledDeltaTime * fireRateModifier;
            //this could be fun maybe? just need to get the bullets to not explode on one another
            //fireTimer += Time.unscaledDeltaTime;

            if (fireTimer >= fireRate)
            {
                fireTimer = 0;
                coolDown = false;
            }
        }
    }

    public void GetTimeSlow(bool isTimeSlowed)
    {
        if (isTimeSlowed)
            fireRateModifier = 0.5f;
        else
            fireRateModifier = 1f;
    }

    void Aim()
    {
        var aim = playerActionControls.Player.Aim.ReadValue<Vector2>();
        if (isGamepad)
        {
            if (Mathf.Abs(aim.x)>controllerDeadzone||Mathf.Abs(aim.y)>controllerDeadzone)
            {
                Vector3 direction = Vector3.right * aim.x + Vector3.forward * aim.y;

                if (direction.sqrMagnitude > 0.0f)
                {
                    Quaternion newRot = Quaternion.LookRotation(direction, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, rotationSmoothing * Time.unscaledDeltaTime);
                }
            }
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(aim);
            Plane groundPlane = new Plane(Vector3.up, player.transform.position);

            float rayDistance;

            if (groundPlane.Raycast(ray, out rayDistance))
            {
                Vector3 point = ray.GetPoint(rayDistance);
                LookAt(point);
            }
        }
        

        //var aimPos = playerActionControls.Player.Aim.ReadValue<Vector2>();
        //var aimWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(aimPos.x, aimPos.y, player.transform.position.y));


        //direction = aimWorldPos - player.transform.position;
        //direction.y = 0;
        //transform.forward = direction;

    }

    private void LookAt(Vector3 point)
    {
        Vector3 heightCorrectPoint = new Vector3(point.x, transform.position.y, point.z);
        transform.LookAt(heightCorrectPoint);
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
        if (isAttacking && !coolDown)
        {
            Vector3 target = transform.forward + new Vector3(Random.Range(-spreadModifier, spreadModifier), 0, Random.Range(-spreadModifier, spreadModifier));
            if (ObjectPool_Projectiles.Instance == false)
            {
                ObjectPool_Projectiles.CreateObjectPoolInstance();
            }
            var obj = ObjectPool_Projectiles.Instance.GetProjectile(bulletPool.prefab.name);
            obj.transform.position = projectileStartPos.transform.position;
            obj.GetComponent<Projectile>().SetUp(target, projectileStartPos.transform.position, this.gameObject.tag);
            obj.GetComponent<AudioSource>().Play();
            coolDown = true;
            if(spreadModifier <= .2f)
                spreadModifier += 0.02f;
            if (uiCursor != null)
            {
                uiCursor.GetComponent<ReticleCursor>().GetSpreadModifier(spreadModifier, true);
            }
            anim.SetBool("isFiring", true);
        }
        else if (!isAttacking)
        {
            spreadModifier = 0;
            if (uiCursor != null)
            {
                uiCursor.GetComponent<ReticleCursor>().GetSpreadModifier(spreadModifier, false);

            }
        }
    }

    public void OnFire(CallbackContext context)
    {
        if (context.performed)
            isAttacking = true;
        if (context.canceled)
        {
            isAttacking = false;
            anim.SetBool("isFiring", false);
        }
    }

    public void OnDeviceChange(PlayerInput pi)
    {
        isGamepad = pi.currentControlScheme.Equals("Gamepad") ? true : false;
    }
}
