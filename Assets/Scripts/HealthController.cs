using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthController : MonoBehaviour {

    public int totalHealth = 1;
    public GameObject heartFull;
    public GameObject heartEmpty;

    private bool isPlayer;
    private int currentHealth;
    
    private GameObject UICanvas;
    private List<GameObject> heartGauge;

	// Use this for initialization
	void Start () {
        
        currentHealth = totalHealth;

        if (gameObject.tag == "Player")
        {
            isPlayer = true;
            
        }
        else
            isPlayer = false;

        if (isPlayer)
        {
            heartGauge = new List<GameObject>();
            UICanvas = GameObject.Find("UI");
            updateHealthUI();
        }

	}
	
    public void hit(int damage)
    {
        currentHealth = currentHealth - damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    void addHealth(int extraHealth)
    {
        currentHealth = currentHealth + extraHealth;
    }

    void addPermanantHealthBuff(int extraHealth)
    {
        totalHealth = totalHealth + extraHealth;
        currentHealth = totalHealth;
    }

    private void updateHealthUI()
    {
        for (int i = 0; i < totalHealth; i++)
        {
            GameObject heartToUse;
            Debug.Log(totalHealth-currentHealth);
            if (i < (totalHealth - currentHealth))
                heartToUse = heartEmpty;
            else
                heartToUse = heartFull;

            if (i>=heartGauge.Count)
            {
                GameObject child = Instantiate(heartToUse);
                child.transform.position = (heartToUse.GetComponent<RectTransform>().position) + (new Vector3(heartToUse.GetComponent<RectTransform>().sizeDelta.x*i, 0, 0));
                child.transform.SetParent(UICanvas.transform, false);
            }
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
