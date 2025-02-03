using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float wallDistanceCheck;
    [SerializeField] float stepdownSize;
    [SerializeField] float playerFollowDistance;

    int direction = 1;

    Rigidbody2D enemyRB;

    GameObject player;

    private void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        player = FindFirstObjectByType<PlayerMovement>().gameObject;
    }


    private void Update()
    {
        if (!Physics2D.Raycast(transform.position + new Vector3(direction, 0), Vector2.down, stepdownSize, LayerMask.GetMask("Ground")))
        {
            direction *= -1;
        }

        if (Physics2D.Raycast(transform.position, new Vector3(direction, 0), wallDistanceCheck, LayerMask.GetMask("Ground")))
        {
            direction *= -1;
        }

        Move();
    }

    private void Move()
    {
        Vector2 playerDistance = (player.transform.position - transform.position);

        if (playerDistance.magnitude < playerFollowDistance)
        {
            enemyRB.linearVelocityX = moveSpeed * Mathf.Sign(playerDistance.x);
        }
        else
        {
            Debug.Log("WUT");
            enemyRB.linearVelocityX = moveSpeed * direction;
        }
    }
}
