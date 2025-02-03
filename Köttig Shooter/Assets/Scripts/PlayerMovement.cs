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
    float scaleOriginal;
    Vector2 capsuleOriginal;

    CapsuleCollider2D capsuleCollider;
    BoxCollider2D feetCollider;
    Rigidbody2D myRigidBody;

    Canvas canvas;

    GameObject playerSprite;
    Animator animator;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        myRigidBody = GetComponent<Rigidbody2D>();

        playerSprite = transform.GetChild(0).gameObject;
        animator = playerSprite.GetComponent<Animator>();

        canvas = GetComponentInChildren<Canvas>();

        scaleOriginal = playerSprite.transform.localScale.x;
        capsuleOriginal = capsuleCollider.size;
    }
    void Update()
    {
        Move();
        Jump();
        Crouch();
        Animate();
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
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(Input.GetAxisRaw("Horizontal")), 1, 1);
            canvas.transform.localScale = new Vector3(Mathf.Sign(Input.GetAxisRaw("Horizontal")), 1, 1);
        }
        else
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.localScale = new Vector3(Mathf.Sign(mousePos.x - transform.position.x), 1, 1);
            canvas.transform.localScale = new Vector3(Mathf.Sign(mousePos.x - transform.position.x), 1, 1);
        }
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
            capsuleCollider.size = new Vector2(capsuleOriginal.x, capsuleOriginal.y / 2);
            playerSprite.transform.localScale = new Vector3(1, 0.5f, 1) * scaleOriginal;
        }
        else
        {
            capsuleCollider.size = new Vector2(capsuleOriginal.x, capsuleOriginal.y);
            playerSprite.transform.localScale = new Vector3(1, 1, 1) * scaleOriginal;
        }
    }

    void Animate()
    {
        bool jumping = !feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        bool walking = Input.GetAxisRaw("Horizontal") != 0 && !jumping;
        bool idle = !walking && !jumping;

        if (idle) { animator.SetBool("Idle", true); } else { animator.SetBool("Idle", false); }
        if (walking) { animator.SetBool("Walking", true); } else { animator.SetBool("Walking", false); }
        if (jumping) { animator.SetBool("Jumping", true); } else { animator.SetBool("Jumping", false); }
    }

    public void Recoil(Vector2 direction, float magnitude)
    {
        myRigidBody.linearVelocity += direction * magnitude;
    }
}
