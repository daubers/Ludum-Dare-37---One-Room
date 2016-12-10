using UnityEngine;
using System.Collections;

public class HealthController : MonoBehaviour {

    public int totalHealth = 1;

    private int currentHealth;

	// Use this for initialization
	void Start () {

        currentHealth = totalHealth;

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

	// Update is called once per frame
	void Update () {
	
	}
}
