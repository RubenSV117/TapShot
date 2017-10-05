using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastRadiusManager : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(600, 50));
            other.gameObject.GetComponent<BasicEnemy>().reduceHealth(1);
            print("Enter");
        }
    }

    public void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(600, 50));
            other.gameObject.GetComponent<BasicEnemy>().reduceHealth(1);
            print("Stay");
        }
    }
}
