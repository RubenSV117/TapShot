using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonArrowManager : BasicArrowManager
{
    /* -Inherited-
    public Rigidbody2D rigidB;
    public int damage;
    public ParticleSystem impactParticle;
    public ParticleSystem deathParticle;
    public GameObject trailrenderer;
    */

    public GameObject poisonCloud;
    public GameObject poisonImpact;

    // Use this for initialization
    void Start()
    {
        //decrement crossBow managers poisonCount
        CrossbowManager.poisonCount--;
        print(CrossbowManager.poisonCount);
        _sound.PlayTripleShot();
    }

    // Update is called once per frame
    void Update()
    {
        if (rigidB != null)
            transform.eulerAngles = new Vector3(0, 0, (Mathf.Atan(rigidB.velocity.y / rigidB.velocity.x) * Mathf.Rad2Deg));
    }

    public new void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "PoisonCloud")
        {
            Instantiate(poisonImpact, transform.position, poisonCloud.transform.rotation);
            _sound.PlayPoisonImpact();
            _sound.PlayTripleShotImpact();
            Instantiate(poisonCloud, new Vector3(transform.position.x, -4, transform.position.z), poisonCloud.transform.rotation);
            Destroy(gameObject);
        }      
    }
}
