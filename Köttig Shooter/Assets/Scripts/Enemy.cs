using System.Collections;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Enemy : MonoBehaviour
{
    [SerializeField] float hitPoints = 5f;
    [SerializeField] float maxHitPoints = 10f;
    [SerializeField] float damageDealt = 1f;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] bool canShoot = false;
    [SerializeField] bool canMove = true;

    [SerializeField] GameObject[] bodyParts;

    Vector2 enemyMovement;
    LayerMask layerMask;

    CapsuleCollider2D capsuleCollider;
    Rigidbody2D myRigidbody;
    SpriteRenderer spriteRenderer;

    bool facingRight = true;

    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        myRigidbody = GetComponent<Rigidbody2D>();

        layerMask = LayerMask.GetMask("Ground");
        hitPoints = maxHitPoints;
        enemyMovement = new Vector2(moveSpeed, 0f);
    }

    private void FixedUpdate()
    {
        Die();

        if (canMove)
        {
            myRigidbody.linearVelocityX = enemyMovement.x;
        }

        Debug.DrawLine (new Vector2(transform.position.x + 4f, transform.position.y), new Vector2(transform.position.x + 4f, transform.position.y) + Vector2.down * 10);

        if (Physics2D.Raycast(new Vector2(transform.position.x + 4, transform.position.y), Vector2.down, 10, layerMask) == false && facingRight)
        {
            if (facingRight)
            {
                gameObject.transform.localScale = new Vector3(-1, gameObject.transform.localScale.y, gameObject.transform.localScale.z);

                facingRight = false;
            }
            else
            {
                gameObject.transform.localScale = new Vector3(1, gameObject.transform.localScale.y, gameObject.transform.localScale.z);

                facingRight = true;
            }
            Debug.Log("yes");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("aj");
        if (collision.transform.tag == "Bullet")
        {
            Debug.Log("ajaj");
            hitPoints--;
        }
    }

    void Die()
    {
        if (hitPoints <= 0)
        {
            for (int i = 0; i < 6; i++)
            {
                GameObject part = Instantiate(bodyParts[i], transform.position, Quaternion.identity);
                part.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
            }

            Destroy(gameObject);
        }
    }
}
