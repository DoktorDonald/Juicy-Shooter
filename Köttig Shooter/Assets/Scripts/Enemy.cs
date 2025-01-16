using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float hitPoints = 5f;
    [SerializeField] float maxHitPoints = 10f;
    [SerializeField] float damageDealt = 1f;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] bool canShoot = false;

    Vector2 enemyMovement;

    CapsuleCollider2D capsuleCollider;
    Rigidbody2D myRigidbody;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        myRigidbody = GetComponent<Rigidbody2D>();

        hitPoints = maxHitPoints;
    }
    private void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Bullet")
        {
            hitPoints = hitPoints--;
        }
    }

    void Die()
    {

    }
}
