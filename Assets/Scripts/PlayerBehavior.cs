using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour {
    public float jumpForce = 100f;
    public float maxVelocity = 10f;
    public float velocityMultiple = 1f;
    public float moveForce = 100f;

    private Rigidbody2D rb2d;
    private bool isGrounded = false;
    private float startDrag = 0f;
    private bool facingRight = true;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        startDrag = rb2d.drag;
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
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position,
            transform.position + transform.up * -2);
    }
}
