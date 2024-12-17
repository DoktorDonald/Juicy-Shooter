using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed;
    [SerializeField] float firerate;
    [SerializeField] float bulletLifeTime;

    [SerializeField] float spreadIncrease;
    [SerializeField] float spreadDelay;
    [SerializeField] float spreadDecrease;
    [SerializeField] float maxSpread;
    [SerializeField] float overheatTime;
    [SerializeField] float rechargeTime;

    [SerializeField] float bulletOriginOffset;

    [SerializeField] float recoil;

    [SerializeField] Image overheatBar;

    float spread;

    Vector2 mousePos;
    Vector2 gunPos;

    Vector3 aimPoint;
    float aimAngle;

    float shootingTimer;
    float spreadTimer;

    bool canShoot = true;

    PlayerMovement playerMovement;
    FollowCamera followCamera;

    private void Awake()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void Start()
    {
        followCamera = FindFirstObjectByType<FollowCamera>();
    }

    private void Update()
    {
        HandleAiming();
        HandleShooting();
        HandleSpread();

        shootingTimer += Time.deltaTime;

        gunPos = new Vector2(transform.position.x, transform.position.y);
    }

    private void HandleAiming()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePos.x - transform.position.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            aimAngle = Mathf.Atan((mousePos.y - transform.position.y) / (mousePos.x - transform.position.x)) * 180 / Mathf.PI;
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            aimAngle = Mathf.Atan((mousePos.y - transform.position.y) / (mousePos.x - transform.position.x)) * 180 / Mathf.PI;
        }

        transform.eulerAngles = new Vector3(0, 0, aimAngle);
    }

    void HandleSpread()
    {
        if (Input.GetMouseButton(0) && canShoot)
        {
            spreadTimer += Time.deltaTime;

            if (spreadTimer >= spreadDelay)
            {
                spread += spreadIncrease * Time.deltaTime;
                spread = Mathf.Clamp(spread, 0, maxSpread);
            }

            if (spreadTimer >= overheatTime)
            {
                canShoot = false;
                Invoke(nameof(EnableShooting), rechargeTime);
            }
        }
        else if (spread > 0)
        {
            spread -= spreadDecrease * Time.deltaTime;
        }

        if (spreadTimer > 0 && !(Input.GetMouseButton(0) && canShoot))
        {
            spreadTimer -= (overheatTime * Time.deltaTime) / rechargeTime;
        }

        overheatBar.fillAmount = spreadTimer / overheatTime;

        Vector2 rand = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        Vector2 closestPoint = gunPos + (mousePos - gunPos).normalized * bulletOriginOffset + (mousePos - gunPos).normalized;

        aimPoint = closestPoint + rand * spread * bulletOriginOffset * (maxSpread + 2);
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButton(0) && canShoot && shootingTimer > 1/firerate)
        {
            GameObject bullet = Instantiate(bulletPrefab, gunPos + (mousePos - gunPos).normalized * bulletOriginOffset, Quaternion.identity);

            Vector3 velocity = ((Vector2)aimPoint - (gunPos + (mousePos - gunPos).normalized * bulletOriginOffset)).normalized * bulletSpeed;

            bullet.GetComponent<Rigidbody2D>().linearVelocity = velocity;

            playerMovement.Recoil(-velocity.normalized, recoil);
            followCamera.StartCameraShake();
           
            bullet.GetComponent<Bullet>().StartTimer(bulletLifeTime);

            shootingTimer = 0;
        }
    }

    void EnableShooting()
    {
        canShoot = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(mousePos, 0.2f);
        Gizmos.DrawWireSphere(gunPos + (mousePos - gunPos).normalized * bulletOriginOffset, 0.2f);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(aimPoint, 0.1f);
    }
}
