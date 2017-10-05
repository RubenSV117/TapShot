using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour
{
    public int health;
    public Text towerHealth;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(health <= 0)
        {
            Destroy(gameObject);
        }

        towerHealth.text = "Tower HP: " + health;

    }

    public void reduceHealth()
    {
        health--;
    }
}
