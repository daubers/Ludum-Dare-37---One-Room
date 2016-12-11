using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour {
    public float jumpForce = 100f;
    public float maxVelocity = 10f;
    public float velocityMultiple = 1f;
    public float moveForce = 100f;

    public float climbSpeed = 5f;

    public float enemyBounceForce = 100f;

    private Rigidbody2D rb2d;
    private bool isGrounded = false;
    private bool facingRight = true;

    private bool canClimb = false;
    private bool isClimbing = false;

    private float startGravity;
    private ScoreController scoreController;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        scoreController = GetComponent<ScoreController>();
        startGravity = rb2d.gravityScale;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    bool isGroundedB(float arc, float rayLength, int direction)
    {
        bool centerRay = Physics2D.Raycast(transform.position, transform.up * direction, rayLength, (1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Ladder")));
        bool leftRay = Physics2D.Raycast(transform.position, Quaternion.AngleAxis(-1*arc, new Vector3(0, 0, 1))*transform.up * direction, rayLength, (1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Ladder")));
        bool rightRay = Physics2D.Raycast(transform.position, Quaternion.AngleAxis(arc, new Vector3(0, 0, 1)) * transform.up * direction, rayLength, (1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Ladder")));

        return leftRay || centerRay || rightRay;
    }

    void FixedUpdate()
    {

        isGrounded = isGroundedB(15, 1.6f, -1);
        bool isLadderAbove = Physics2D.Raycast(transform.position, transform.up, 2, (1 << LayerMask.NameToLayer("Ladder")));
        bool isLadderBelow = Physics2D.Raycast(transform.position, transform.up*-1, 2, (1 << LayerMask.NameToLayer("Ladder")));
        if (rb2d.velocity.y>maxVelocity)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, maxVelocity);
        }



        if (Input.GetAxis("Jump") > 0 && isGrounded && !isLadderAbove)
        {
            float tmpJumpForce = jumpForce;
            if (isLadderBelow)
                tmpJumpForce = tmpJumpForce / 10;
                
            rb2d.AddForce(new Vector2(0, tmpJumpForce));
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
        if (!canClimb)
        {
            //rb2d.gravityScale = startGravity;
            isClimbing = false;
        }
        if (inputVert != 0 && canClimb)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, inputVert*climbSpeed);
            isClimbing = true;
        }

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Vector2 dir = other.contacts[0].point - new Vector2(transform.position.x, transform.position.y);
            dir = -dir.normalized;
            rb2d.AddForce(enemyBounceForce * dir);

            bool hit = Physics2D.Raycast(transform.position, -1 * Vector2.up, 1.5f, 1 << LayerMask.NameToLayer("Enemy"));
            if (hit)
            {    
                scoreController.addPoints(other.gameObject.GetComponent<SimpleEnemyScript>().hit(1));
            }
            else
            {
                gameObject.GetComponent<HealthController>().hit(1);
            }
            
        }
        else if (other.gameObject.tag == "instadeath")
        {
            gameObject.GetComponent<HealthController>().instaDeath();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position,
            transform.position + transform.up * -1.6f);
        Gizmos.DrawLine(transform.position,
    transform.position + Quaternion.AngleAxis(-1 * 15, new Vector3(0, 0, 1)) * transform.up * -1.6f);
        Gizmos.DrawLine(transform.position,
            transform.position + Quaternion.AngleAxis(1 * 15, new Vector3(0, 0, 1)) * transform.up * -1.6f);

    }
}
