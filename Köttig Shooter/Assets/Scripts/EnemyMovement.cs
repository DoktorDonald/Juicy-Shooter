using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    int direction = 1;

    Rigidbody2D enemyRB;

    private void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        if (!Physics2D.Raycast(transform.position + new Vector3(direction, 0), Vector2.down, 10, LayerMask.GetMask("Ground")))
        {
            direction *= -1;
        }

        if (Physics2D.Raycast(transform.position, new Vector3(direction, 0), 1, LayerMask.GetMask("Ground")))
        {
            direction *= -1;
        }

        Move();
    }

    private void Move()
    {
        enemyRB.linearVelocityX = moveSpeed * direction;
    }
}
