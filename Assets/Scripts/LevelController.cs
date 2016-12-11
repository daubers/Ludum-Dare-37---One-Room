using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {

    private Dictionary<string, float> completedLevels = new Dictionary<string, float>();
    

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void completeLevel(string levelName)
    {
        float score = gameObject.GetComponent<ScoreController>().getScore();
        if (!completedLevels.ContainsKey(levelName))
        {
            completedLevels.Add(levelName, score);
        }
        if (completedLevels[levelName] < score)
        {
            completedLevels[levelName] = score;
        }

        StartCoroutine("goToLevelSelect");

    }


    IEnumerator goToLevelSelect()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadSceneAsync("LevelSelect");
    }


    public bool isCompleted(string levelName)
    {
        return completedLevels.ContainsKey(levelName);
    }

    public float getScore(string levelName)
    {
        return completedLevels[levelName];
    }
}
