using UnityEngine;
using System.Collections;

public class SimpleEnemyScript : MonoBehaviour {

    public float walkSpeed = 5f;
    public float debounceTime = 0.5f;


    private float lastChange = 0;
    private bool onGround = false;
    private Rigidbody2D rb2d;
    private Vector2 edgeScan = new Vector2(1, -1);

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
 
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        onGround = Physics2D.Raycast(transform.Find("EdgeDetector").position, edgeScan, 1, 1 << LayerMask.NameToLayer("Ground"));

        if (onGround)
        {
            rb2d.velocity = new Vector2(walkSpeed, 0);
        }
        else
        {
            if (Time.fixedTime - lastChange > debounceTime) {
                walkSpeed = walkSpeed * -1; 
                rb2d.velocity = new Vector2(walkSpeed * -1, 0);
                edgeScan.x = edgeScan.x * -1;
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
        }
             
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.Find("EdgeDetector").position,
            transform.Find("EdgeDetector").position + (new Vector3(edgeScan.x, edgeScan.y,0)) * 1);
    }
}
