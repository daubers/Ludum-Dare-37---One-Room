using UnityEngine;
using System.Collections;

public class RopeSwitch : MonoBehaviour
{

    public GameObject[] rope = new GameObject[4];
    public GameObject destroyEffect;

    private float resetSpeed = 2f;
    private float tweenTime = 0f;
    private Vector3 tweenStartPosition;
    private Vector3 tweenEndPosition;
    private bool sinking = false;
    private bool pressed = false;

    // Use this for initialization
    void Start()
    {
        tweenStartPosition = transform.position;
        tweenEndPosition = transform.position - new Vector3(0, 0.4f, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (sinking)
        {
            tweenTime = tweenTime + Time.deltaTime;
            transform.position = Vector3.Lerp(tweenStartPosition, tweenEndPosition, tweenTime / resetSpeed);
            if (transform.position == tweenEndPosition)
            {
                tweenTime = 0f;
                sinking = false;
                onPressed();
            }
        }
    }

    void onPressed()
    {
        pressed = true;
        for (int i = 0; i < rope.Length; i++)
        {

            GameObject fx = (GameObject)Instantiate(destroyEffect, rope[i].transform.position, Quaternion.identity);
            Destroy(rope[i]);
            Destroy(fx, 2);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Player")
        {
            bool isPlayerAbove = Physics2D.Raycast(transform.position, transform.up, 4, (1 << LayerMask.NameToLayer("Player")));
            if (!pressed && isPlayerAbove)
                sinking = true;
        }
    }
}
