using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float hitPoints = 5f;
    [SerializeField] float maxHitPoints = 10f;
    [SerializeField] float damageDealt = 1f;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] bool canShoot = false;

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
        myRigidbody.linearVelocityX = enemyMovement.x;

        Debug.DrawLine(transform.position, transform.position + transform.TransformDirection(new Vector3(transform.position.x + 5f, transform.position.y - 5f, 0f)) * Mathf.Sqrt(50f));

        if (Physics.Raycast(transform.position, transform.TransformDirection(new Vector3(transform.position.x + 5f, transform.position.y - 5f, 0f)), Mathf.Sqrt(50f), layerMask) == false)
        {
            if (facingRight)
            {
                gameObject.transform.localScale = new Vector3(-1, gameObject.transform.localScale.y, gameObject.transform.localScale.z);

                facingRight = false;
            }
            else if (!facingRight)
            {
                gameObject.transform.localScale = new Vector3(1, gameObject.transform.localScale.y, gameObject.transform.localScale.z);

                facingRight = true;
            }
        }
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
