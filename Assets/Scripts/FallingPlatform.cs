using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour
{

    public float graceTime = 2f;

    private Rigidbody2D rb2d;
    private bool dropped = false;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            if (!dropped)
            {
                StartCoroutine("dropPlatform");
                dropped = true;
            }
        }
    }

    IEnumerator dropPlatform()
    {
        yield return new WaitForSeconds(graceTime);
        rb2d.isKinematic = false;
        Debug.Log(rb2d.isKinematic);
    }
}
