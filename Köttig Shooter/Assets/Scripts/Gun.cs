using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed;
    [SerializeField] float firerate;
    [SerializeField] float bulletLifeTime;

    [SerializeField] float bulletOriginOffset;


    Vector2 mousePos;
    Vector3 aimPoint;
    float aimAngle;
    float shootingTimer;

    private void Update()
    {
        HandleAiming();
        HandleShooting();

        shootingTimer += Time.deltaTime;
    }

    private void HandleAiming()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimPoint = mousePos;

        if (aimPoint.x - transform.position.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            aimAngle = Mathf.Atan((aimPoint.y - transform.position.y) / (aimPoint.x - transform.position.x)) * 180 / Mathf.PI;
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            aimAngle = Mathf.Atan((aimPoint.y - transform.position.y) / (aimPoint.x - transform.position.x)) * 180 / Mathf.PI;
        }

        transform.eulerAngles = new Vector3(0, 0, aimAngle);
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButton(0) && shootingTimer > 1/firerate)
        {
            GameObject bullet = Instantiate(bulletPrefab, new Vector2(transform.position.x, transform.position.y) + (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized * bulletOriginOffset, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized * bulletSpeed;
           
            bullet.GetComponent<Bullet>().StartTimer(bulletLifeTime);

            shootingTimer = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(mousePos, 0.2f);
        Gizmos.DrawWireSphere(new Vector2 (transform.position.x, transform.position.y) + (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized * bulletOriginOffset, 0.2f);
    }
}
