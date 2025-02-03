using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float wallDistanceCheck;
    [SerializeField] float stepdownSize;

    int direction = 1;

    Rigidbody2D enemyRB;

    private void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
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
        enemyRB.linearVelocityX = moveSpeed * direction;
    }
}
