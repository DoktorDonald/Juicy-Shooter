using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Moevement")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] float movementSmoothing = 2f;

    float velocity;
    bool canJump = true;

    CapsuleCollider2D capsuleCollider;
    Rigidbody2D myRigidBody;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        myRigidBody = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Move();
        Jump();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        canJump = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        canJump = false;
    }
    void Move()
    {
        myRigidBody.linearVelocityX = Mathf.SmoothDamp(myRigidBody.linearVelocityX, Input.GetAxisRaw("Horizontal") * moveSpeed, ref velocity, movementSmoothing);
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            myRigidBody.linearVelocityY = jumpHeight;
        }  
    }
}
