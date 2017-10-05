using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackArrowManager : BasicArrowManager
{
    /* -Inherited-
    public Rigidbody2D rigidB;
    public int damage;
    public ParticleSystem impactParticle;
    public ParticleSystem deathParticle;
    public GameObject trailrenderer;
    */

    public ParticleSystem knockBackImpactParticle;
    public GameObject knockBackShotParticle;
    public static bool isFirstShot;

    // Use this for initialization
    void Start ()
    {
        if (isFirstShot)
            _sound.PlayKnockBackShot();

        else
        {
            _sound.playNormalArrowShot();
            knockBackShotParticle.transform.localScale = new Vector3(.5f, .5f, knockBackShotParticle.transform.localScale.z);
            rigidB.velocity = CrossbowManager.shootVector.normalized * 35;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (rigidB != null)
        {
            transform.eulerAngles = new Vector3(0, 0, (Mathf.Atan(rigidB.velocity.y / rigidB.velocity.x) * Mathf.Rad2Deg));
           
        }
    }

    public new void OnTriggerEnter2D(Collider2D other)
    {
        if (isFirstShot)
        {   
            isFirstShot = false;

            Destroy(GetComponent<SpriteRenderer>());
            Destroy(trailrenderer);
            Destroy(GetComponent<BoxCollider2D>());
            Destroy(knockBackShotParticle);

            _crossbow.KnockBack();
            Instantiate(knockBackImpactParticle, transform.position, knockBackImpactParticle.transform.rotation);

            //insantiate particle on every enemy
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            for (int i = 0; i < enemies.Length; i++)
            {
                Vector3 particlePosition = new Vector3(enemies[i].transform.position.x, -4.7f, enemies[i].transform.position.z - 1);
                Instantiate(knockBackImpactParticle, particlePosition, knockBackImpactParticle.transform.rotation);
            }
            
        }

        else
        {
            GetComponent<BasicArrowManager>().OnTriggerEnter2D(other);
        }
            

    }
}
