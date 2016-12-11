using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

    private float score = 0;
    private GameObject scoreText;

	// Use this for initialization
	void Start () {
        scoreText = GameObject.Find("ScoreText");
        updateUI();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void addPoints(int points)
    {
        score = score + points;
        updateUI();
    }

    public void updateUI()
    {
        scoreText.GetComponent<Text>().text = score.ToString();
    }
}
