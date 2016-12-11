using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour {
    public float jumpForce = 100f;
    public float maxVelocity = 10f;
    public float velocityMultiple = 1f;
    public float moveForce = 100f;

    public float climbSpeed = 5f;

    public float enemyBounceForce = 100f;

    public GameObject deathEffect;

    private Rigidbody2D rb2d;
    private bool isGrounded = false;
    private bool facingRight = true;

    private bool canClimb = false;
    private bool isClimbing = false;

    private float startGravity;
    private ScoreController scoreController;

    private Vector3 startPoint;

    private float resetSpeed = 5f;
    private float tweenTime = 0f;
    private Vector3 tweenStartPosition;
    private bool resetting=false;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        scoreController = GetComponent<ScoreController>();
        startPoint = GameObject.Find("StartPosition").transform.position;
        startGravity = rb2d.gravityScale;
	}
	
	// Update is called once per frame
	void Update () {
	    if (resetting)
        {
            tweenTime = tweenTime + Time.deltaTime;
            transform.position = Vector3.Lerp(tweenStartPosition, startPoint, tweenTime / resetSpeed);
            if (transform.position == startPoint)
            {
                tweenTime = 0f;
                resetting = false;
                onRevival();
            }
        }
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
        Debug.Log(other.gameObject.tag);
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

    void OnParticleCollision(GameObject src)
    {
        if (src.gameObject.tag == "instadeath")
        {
            gameObject.GetComponent<HealthController>().instaDeath();
        }
    }

    public void onDeath()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<PolygonCollider2D>().enabled = false;
        rb2d.gravityScale = 0;
        GameObject fx = (GameObject) Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(fx, 2);
        StartCoroutine("resetToStart");
    }

    IEnumerator resetToStart()
    {
        yield return new WaitForSeconds(2);
        Debug.Log("Resetting");
        resetting = true;
        tweenStartPosition = transform.position;
    }

    void onRevival()
    {
        gameObject.GetComponent<HealthController>().resetHealth();
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<PolygonCollider2D>().enabled = true;
        rb2d.gravityScale = 1;
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

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
