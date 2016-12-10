using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour {
    public float jumpForce = 100f;
    public float maxVelocity = 10f;
    public float velocityMultiple = 1f;
    public float moveForce = 100f;

    public float climbSpeed = 5f;

    private Rigidbody2D rb2d;
    private bool isGrounded = false;
    private bool facingRight = true;

    private bool canClimb = false;
    private bool isClimbing = false;

    private float startGravity;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        startGravity = rb2d.gravityScale;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void FixedUpdate()
    {

        isGrounded = Physics2D.Raycast(transform.position, transform.up * -1, 2, 1 << LayerMask.NameToLayer("Ground"));

        if (!isGrounded)
        {
            rb2d.drag = 0;
        }


        if (Input.GetAxis("Jump") > 0 && isGrounded)
        {
            rb2d.AddForce(new Vector2(0, jumpForce));
        }

        float inputHoriz = Input.GetAxis("Horizontal");
       
        if (inputHoriz != 0)
        {
            if (rb2d.velocity.magnitude < maxVelocity)
            {
                // rb2d.velocity = new Vector2(velocityMultiple * Input.GetAxis("Horizontal"), rb2d.velocity.y);
                rb2d.AddForce(new Vector2(inputHoriz * velocityMultiple, 0));
                if (inputHoriz < 0 && facingRight)
                {
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                    facingRight = false;
                }
                else if (inputHoriz > 0 && !facingRight)
                {
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                    facingRight = true;
                }
            }
        }

        float inputVert = Input.GetAxis("Vertical");
        canClimb = Physics2D.Raycast(transform.position, transform.up * rb2d.velocity.magnitude, 1, 1 << LayerMask.NameToLayer("Ladder"));
        if (rb2d.gravityScale == 0 && !canClimb)
        {
            rb2d.gravityScale = startGravity;
            isClimbing = false;
        }
        if (inputVert != 0 && canClimb)
        {
            if (isClimbing)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, inputVert*climbSpeed);
            }
            else
            {
                rb2d.gravityScale = 0;
                rb2d.velocity = new Vector2(rb2d.velocity.x, inputVert*climbSpeed);
            }
        }

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -1 * Vector2.up, 1.5f, ~((1 << LayerMask.NameToLayer("Player")) | 1 << LayerMask.NameToLayer("Ladder")));
            if (!object.ReferenceEquals(null, hit))
            {
                if (hit.transform.gameObject.tag == "Enemy")
                {
                    // good hit
                    other.gameObject.GetComponent<SimpleEnemyScript>().hit(1);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position,
            transform.position + transform.up * -1.5f);
    }
}
