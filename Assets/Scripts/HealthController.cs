﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HealthController : MonoBehaviour {

    public int totalHealth = 1;
    public GameObject heartFull;
    public GameObject heartEmpty;
    public float damageDebounce = 1f;

    private float lastDamage = 0f;

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
	
    public int hit(int damage)
    {
        /* Takes damage, returns score
         * 
         */
        if (Time.fixedTime  > lastDamage + damageDebounce)
        {
            currentHealth = currentHealth - damage;
            if (currentHealth <= 0)
            {
                int score = gameObject.GetComponent<EnemyProperties>().getPoints();
                Destroy(gameObject);
                return score;
            }
            lastDamage = Time.fixedTime;
            updateHealthUI();
        }
        return 0;
    }

    public void instaDeath()
    {
        hit(currentHealth);
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
        for (int i = 1; i < totalHealth+1; i++)
        {
            GameObject heartToUse;
            if (i-1 < (totalHealth - currentHealth))
                heartToUse = heartEmpty;
            else
                heartToUse = heartFull;
            if (i>heartGauge.Count)
            {
                GameObject child = Instantiate(heartToUse);
                child.transform.position = (heartToUse.GetComponent<RectTransform>().position) + (new Vector3(heartToUse.GetComponent<RectTransform>().sizeDelta.x*(i-1), 0, 0));
                child.transform.SetParent(UICanvas.transform, false);
                heartGauge.Add(child);
            }
            else
            {
                heartGauge[heartGauge.Count-i].GetComponent<Image>().sprite = heartToUse.GetComponent<Image>().sprite;
            }
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
