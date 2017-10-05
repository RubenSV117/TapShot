using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindControlArrowManager : BasicArrowManager
{
    /* -Inherited-
      public Rigidbody2D rigidB;
      public int damage;
      public ParticleSystem impactParticle;
      public ParticleSystem deathParticle;
      public GameObject trailrenderer;
    */

    public GameObject mindControlOrb;
    public ParticleSystem mindControlImpactParticle;

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

    new void OnTriggerEnter2D(Collider2D other)
    {
        _sound.PlayMindControlArrowImpact();
        Instantiate(mindControlImpactParticle, transform.position, mindControlImpactParticle.transform.rotation);
       
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject nearestEnemy = null;

        if(enemies.Length > 0)
        {
            //set nearestEnemy to the first enemy on the list
            nearestEnemy = enemies[0];

            for(int i = 0; i < enemies.Length; i++)
            {
                //if there is an enemy that is closer than the current nearestEnemy, set nearestEnemy to that
                if ((Mathf.Abs(enemies[i].transform.position.x - transform.position.x)) < (Mathf.Abs(nearestEnemy.transform.position.x - transform.position.x)))
                    nearestEnemy = enemies[i]; 
            }
        }

        if(nearestEnemy != null)
        {
            MindControlOrbManager orb = Instantiate(mindControlOrb, transform.position, mindControlOrb.transform.rotation).GetComponent<MindControlOrbManager>();
            orb.targetEnemy = nearestEnemy;        
        }

        GetComponent<BasicArrowManager>().OnTriggerEnter2D(other);

        _crossbow.deactivatePowerShot();

    }
}
