using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(wait2Secs());
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public IEnumerator wait2Secs()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.name == "Tower")
        {
            other.gameObject.GetComponent<TowerManager>().reduceHealth();
        }
    }
}
