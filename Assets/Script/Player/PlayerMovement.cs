using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool isJumping;

    private bool isGrounded;

    public float horizontalMoveSpeed; // Move speed of the player

    public float jumpingForce; // Move speed of the player

    public Rigidbody2D rb;

    private Vector3 velocity = Vector3.zero;

    private Animator animator;

    public Transform groundCheck;

    public float groundCheckRadius;

    public LayerMask collisionLayers;

    private float horizontalMovement;

    public static PlayerMovement instance;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de PlayerMovement dans la scène");
            return;
        }

        instance = this;

        animator = gameObject.GetComponentInChildren<Animator>();
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayers);
        animator.SetBool("IsGrounded", isGrounded);
    }

    private void Update()
    {
        horizontalMovement = Input.GetAxis("Horizontal") * horizontalMoveSpeed * Time.fixedDeltaTime;


        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("Attack");
        }

        // Flip(rb.velocity.x);

        animator.SetFloat("Velocity", rb.velocity.x);
    }
    private void FixedUpdate() // On ne met pas les Input ici. On ne fait que la physique tel qu MovePlayer 
    {

        if (isGrounded != Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayers))
        {
            isGrounded = !isGrounded;
            animator.SetBool("IsGrounded", isGrounded);
        }
        
        MovePlayer(horizontalMovement);
    }

    private void MovePlayer(float _horizontalMovement)
    {
        Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);

        if (isJumping)
        {
            rb.AddForce(new Vector2(0f, jumpingForce));
            isJumping = false;
        }
    }

    private void Flip(float _velocity)
    {
        if (_velocity > 0.3f)
        {
            transform.eulerAngles = new Vector3(0, 0, 0); // normal
        }
        else if (_velocity < -0.3f)
        {
            transform.eulerAngles = new Vector3(0, 180, 0); // Flipped
        }
    }

    private void OnDrawGizmos() // On créér une représentation visuelle du GroundCheck
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
