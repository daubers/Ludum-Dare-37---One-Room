using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class QuickPlayerStartPosition : MonoBehaviour {

    private GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("player");
        player.transform.position = transform.position;
    }
	
    void Awake()
    {
        
        
    }

	// Update is called once per frame
	void Update () {
	
	}
}
