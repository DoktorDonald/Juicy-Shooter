using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [Header("Bullets")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject casingPrefab;
    [SerializeField] float bulletOriginOffset;
    [SerializeField] float bulletSpeed;
    [SerializeField] float firerate;
    [SerializeField] float bulletLifeTime;

    [Header("Gun")]
    [SerializeField] float spreadIncrease;
    [SerializeField] float spreadDelay;
    [SerializeField] float spreadDecrease;
    [SerializeField] float maxSpread;
    [SerializeField] float overheatTime;
    [SerializeField] float rechargeTime;
    [SerializeField] float recoil;

    [Header("Casings")]
    [SerializeField] float ejectionSpeed;
    [SerializeField] float ejectionSpeedRandomness;
    [SerializeField] float ejectAngleRandomness;
    [SerializeField] int maxCasingCount;

    [Header("Other")]
    [SerializeField] Image overheatBar;

    [SerializeField] List<GameObject> casings = new List<GameObject>();

    float spread;

    Vector2 mousePos;
    Vector2 gunPos;

    Vector3 aimPoint;
    float aimAngle;

    float shootingTimer;
    float spreadTimer;
    float spreadOffsetTimer;

    bool canShoot = true;

    GunAudio gunAudio;
    PlayerMovement playerMovement;
    FollowCamera followCamera;

    [SerializeField] GameObject muzzleFlash;

    private void Awake()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void Start()
    {
        gunAudio = GetComponent<GunAudio>();
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
            transform.localScale = new Vector3(1 * transform.parent.transform.localScale.x, 1, 1);
            aimAngle = Mathf.Atan((mousePos.y - transform.position.y) / (mousePos.x - transform.position.x)) * 180 / Mathf.PI;
        }
        else
        {
            transform.localScale = new Vector3(-1 * transform.parent.transform.localScale.x, 1, 1);
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
            shootingTimer = 0;

            GameObject bullet = Instantiate(bulletPrefab, gunPos + (mousePos - gunPos).normalized * bulletOriginOffset, Quaternion.identity);

            Vector3 velocity = ((Vector2)aimPoint - (gunPos + (mousePos - gunPos).normalized * bulletOriginOffset)).normalized * bulletSpeed;

            bullet.GetComponent<Rigidbody2D>().linearVelocity = velocity;

            StartCoroutine(nameof(MuzzleFlash));

            gunAudio.playAudio();

            playerMovement.Recoil(-velocity.normalized, recoil);
            followCamera.StartCameraShake();
           
            bullet.GetComponent<Bullet>().StartTimer(bulletLifeTime);

            EjectCasing();
        }
    }

    void EjectCasing()
    {
        GameObject casing = Instantiate(casingPrefab, transform.position, Quaternion.identity);

        casings.Add(casing);
        HandleCasings();

        float random = ejectAngleRandomness;

        Vector3 rand1 = new Vector2(Random.Range(-random, random), Random.Range(-random, random));
        Vector2 eject = GameObject.Find("Casing Eject Angle").transform.position + rand1 - transform.position;

        float rand2 = Random.Range(-ejectionSpeedRandomness / 2, ejectionSpeedRandomness / 2);

        casing.GetComponent<Rigidbody2D>().linearVelocity = eject.normalized * (ejectionSpeed + rand2);
        casing.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-360, 360);
    }

    void HandleCasings()
    {
        if (casings.Count > maxCasingCount)
        {
            Destroy(casings[0]);
            
            for (int i = 0; i < maxCasingCount; i++)
            {
                casings[i] = casings[i + 1];

                if (i == maxCasingCount - 1)
                {
                    casings.Remove(casings[maxCasingCount]);
                }
            }
        }
    }

    void EnableShooting()
    {
        canShoot = true;
    }

    IEnumerator MuzzleFlash()
    {
        muzzleFlash.GetComponent<SpriteRenderer>().enabled = true;
        muzzleFlash.GetComponentInChildren<Light2D>().enabled = true;

        yield return new WaitForSeconds(0.05f);

        muzzleFlash.GetComponent<SpriteRenderer>().enabled = false;
        muzzleFlash.GetComponentInChildren<Light2D>().enabled = false;
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
