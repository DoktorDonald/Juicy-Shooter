using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float wallDistanceCheck;
    [SerializeField] float stepdownSize;
    [SerializeField] float playerFollowDistance;
    [SerializeField] float attackDistance;
    [SerializeField] float attackInterval;

    float attackTimer;

    int direction = 1;

    Rigidbody2D enemyRB;
    SpriteRenderer spriteRenderer;
    GameObject visuals;

    GameObject player;

    float velocity;

    private void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        player = FindFirstObjectByType<PlayerMovement>().gameObject;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        visuals = spriteRenderer.gameObject;
    }


    private void Update()
    {
        attackTimer += Time.deltaTime;

        if (!Physics2D.Raycast(transform.position + new Vector3(direction, 0), Vector2.down, stepdownSize, LayerMask.GetMask("Ground")))
        {
            direction *= -1;
        }

        if (Physics2D.Raycast(transform.position, new Vector3(direction, 0), wallDistanceCheck, LayerMask.GetMask("Ground")))
        {
            direction *= -1;
        }

        Movement();
    }

    private void Movement()
    {
        Vector2 playerDistance = (player.transform.position - transform.position);

        if (playerDistance.magnitude < playerFollowDistance)
        {
            Move(moveSpeed * Mathf.Sign(playerDistance.x));
            visuals.transform.eulerAngles = new Vector3(0, 90 * (Mathf.Sign(playerDistance.x) - 1), 1);
        }
        else
        {
            Move(moveSpeed * direction);
            visuals.transform.eulerAngles = new Vector3(0, 90 * (direction - 1), 1);
        }

        if (playerDistance.magnitude < attackDistance && attackTimer > attackInterval)
        {
            player.GetComponent<PlayerHealth>().Damage(1, transform.position);

            attackTimer = 0;
        }
    }

    private void Move(float movement)
    {
        enemyRB.linearVelocityX = Mathf.SmoothDamp(enemyRB.linearVelocityX, movement, ref velocity, 0.1f);
    }
}
