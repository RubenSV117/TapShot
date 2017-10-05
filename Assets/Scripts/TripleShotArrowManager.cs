﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShotArrowManager : BasicArrowManager
{
    /* -Inherited-
      public Rigidbody2D rigidB;
      public int damage;
      public ParticleSystem impactParticle;
      public ParticleSystem deathParticle;
      public GameObject trailrenderer;
    */

    public GameObject explosionParticle;

    // Use this for initialization
    void Start ()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (rigidB != null)
            transform.eulerAngles = new Vector3(0, 0, (Mathf.Atan(rigidB.velocity.y / rigidB.velocity.x) * Mathf.Rad2Deg));
    }

    public new void OnTriggerEnter2D(Collider2D other)
    {
        //returns all object within radius that have a collider2D
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, 1.3f);

        //simulate blast radius
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].gameObject.tag == "Enemy")
            {

                objects[i].gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(700, 50));
                objects[i].gameObject.GetComponent<BasicEnemy>().reduceHealth(1);
            }
        }

        //instantiate explosion particle
        Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
        //play explosion sounds
        _sound.PlayTripleShotImpact();

    }
}
