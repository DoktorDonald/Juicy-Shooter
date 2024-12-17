using UnityEngine;

public class AltMovement : MonoBehaviour
{
    // Serialized Variables

    [SerializeField][Tooltip("The max speed of the player on the X-axis")] float movementSpeed;
    [SerializeField][Tooltip("The smoothness of the player's acceleration")] float movementSmoothing;
    [SerializeField][Tooltip("If enabled, allows the player to retain speed above the movementSpeed. Decelerates while on the ground")] bool inertia;

    [SerializeField][Tooltip("The speed on the Y-axis given to the player when jumping")] float jumpPower;
    [SerializeField][Tooltip("The time after leaving the ground for which the player can still jump")] float coyoteTime;

    [SerializeField][Tooltip("Number of times the player can bounce in a row")] float maxBounces;

    [SerializeField][Tooltip("If enabled, the player can dash")] bool canDash;
    [SerializeField][Tooltip("The speed given to the player upon dashing")] float dashSpeed;

    [SerializeField] PhysicsMaterial2D[] materials;


    // Private Variables

    Vector2 input;
    Vector2 mousePos;
    Vector2 mouseDirection;

    [SerializeField] float movementSpeedActual;

    float velocity1;

    int bounces;

    [SerializeField] bool dash;

    [SerializeField] float coyoteTimer;

    [SerializeField] bool grounded;


    // Cached References

    Rigidbody2D playerRB;
    CircleCollider2D playerCollider;
    BoxCollider2D feetCollider;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CircleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        movementSpeedActual = movementSpeed;
    }

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mouseDirection = (mousePos - new Vector2(1, 1) * transform.position);

        Movement();
        Bounce();
        Dash();

        if (inertia)
        {
            SpeedController();
        }

        if (!grounded)
        {
            coyoteTimer -= Time.deltaTime;
        }
    }

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && dash && !grounded && canDash && coyoteTimer < 0)
        {
            playerRB.linearVelocity = mouseDirection.normalized * dashSpeed;
            dash = false;
        }
    }

    private void Movement()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        float inputVelocityX = Mathf.SmoothDamp(playerRB.linearVelocity.x, input.x * movementSpeedActual, ref velocity1, movementSmoothing);

        playerRB.linearVelocityX = inputVelocityX;

        if (Input.GetKeyDown(KeyCode.Space) && coyoteTimer > 0)
        {
            playerRB.linearVelocityY = jumpPower;
            coyoteTimer = 0;
        }
    }

    private void SpeedController()
    {
        if (Mathf.Abs(playerRB.linearVelocity.x) > movementSpeed)
        {
            movementSpeedActual = Mathf.Abs(playerRB.linearVelocity.x);
        }


        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && movementSpeedActual > movementSpeed)
        {
            movementSpeedActual = movementSpeed;
        }
    }

    private void Bounce()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            playerCollider.sharedMaterial = materials[1];
            spriteRenderer.color = Color.green;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) || bounces >= maxBounces)
        {
            playerCollider.sharedMaterial = materials[0];
            spriteRenderer.color = Color.white;
        }

        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && !Input.GetKey(KeyCode.LeftControl))
        {
            bounces = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            grounded = true;
            coyoteTimer = coyoteTime;
        }

        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && (bounces >= maxBounces || !Input.GetKey(KeyCode.LeftControl)))
        {
            dash = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (playerCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && Input.GetKey(KeyCode.LeftControl) && bounces < maxBounces)
        {
            bounces++;
            grounded = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            grounded = false;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {

    }

    private void OnDrawGizmos()
    {
        if (playerRB == null) { return; }

        // Input
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(input.x, input.y).normalized);

        // Velocity
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(playerRB.linearVelocity.x, playerRB.linearVelocity.y) / 5);

        // Cursor
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(mousePos, 0.5f);

        Vector3 mouseDirNorm = new Vector3(1, 1, 0) * mouseDirection.normalized;
        Gizmos.DrawLine(transform.position + mouseDirNorm * 1.2f, transform.position + mouseDirNorm * 2);

        // Grounded
        if (grounded)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(transform.position + new Vector3(0, -0.25f), 0.2f);
        }

        // Dash
        if (dash)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(transform.position + new Vector3(1, 0.3f), 0.2f);
        }
    }
}
