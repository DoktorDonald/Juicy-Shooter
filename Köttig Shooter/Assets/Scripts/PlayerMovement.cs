using Unity.Mathematics;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Moevement")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] float movementSmoothing = 2f;

    float velocity;
    bool canJump;

    CapsuleCollider2D capsuleCollider;
    BoxCollider2D feetCollider;
    Rigidbody2D myRigidBody;

    GameObject playerSprite;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        myRigidBody = GetComponent<Rigidbody2D>();

        playerSprite = transform.GetChild(0).gameObject;
    }
    void Update()
    {
        Move();
        Jump();
        Crouch();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            canJump = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            canJump = false;
        }
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

    void Crouch()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            capsuleCollider.size = new Vector2(1, 1);
            playerSprite.transform.localScale = new Vector3(1, 0.5f, 1);
        }
        else
        {
            capsuleCollider.size = new Vector2(1, 2);
            playerSprite.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void Recoil(Vector2 direction, float magnitude)
    {
        myRigidBody.linearVelocity += direction * magnitude;
    }
}
