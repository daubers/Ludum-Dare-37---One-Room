using UnityEngine;
using System.Collections;

public class KnifeBehaviour : MonoBehaviour
{

    public AudioClip tapSound;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            AudioSource.PlayClipAtPoint(tapSound, other.contacts[0].point);
        }
    }
}