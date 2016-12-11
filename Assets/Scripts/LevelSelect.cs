using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour {

    private GameObject player;
    public GameObject highScoreText;

	// Use this for initialization
	void Start () {

        player = GameObject.Find("player");
        highScoreText.GetComponent<Text>().text = "Current High Score: " + player.GetComponent<ScoreController>().getScore().ToString();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void loadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void loadSceneByNo(int sceneNo)
    {
        SceneManager.LoadScene(1);
    }
}
