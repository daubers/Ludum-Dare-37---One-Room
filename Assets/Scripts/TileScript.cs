using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    SpriteRenderer sprite;

    void Awake()
    {
        // Get the current sprite with an unscaled size
        sprite = GetComponent<SpriteRenderer>();
        Vector2 spriteSize = new Vector2(sprite.bounds.size.x / transform.localScale.x, sprite.bounds.size.y / transform.localScale.y);

        // Generate a child prefab of the sprite renderer
        GameObject childPrefab = new GameObject();
        SpriteRenderer childSprite = childPrefab.AddComponent<SpriteRenderer>();
        childPrefab.transform.position = transform.position;
        childSprite.sprite = sprite.sprite;

        // Loop through and spit out repeated tiles
        GameObject child;
        for (int i = 0, l = (int)Mathf.Round(sprite.bounds.size.y/spriteSize.y); i < l+1; i++)
        {
            for (int x = 0, lx = (int)Mathf.Round(sprite.bounds.size.x/spriteSize.x); x < lx+1; x++) { 
                child = Instantiate(childPrefab) as GameObject;
                child.transform.position = (sprite.bounds.extents + transform.position) - (new Vector3(spriteSize.x*x, spriteSize.y*i, 0));
                child.transform.parent = transform;
            }
        }

        // Set the parent last on the prefab to prevent transform displacement
        childPrefab.transform.parent = transform;

        // Disable the currently existing sprite component since its now a repeated image
        sprite.enabled = false;
    }
}
