using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicArrowManager : MonoBehaviour
{
    public Rigidbody2D rigidB;
    public int damage;
    public ParticleSystem impactParticle;
    public ParticleSystem deathParticle;
    public GameObject trailrenderer;

    protected static SoundEffectsManager _sound;
    protected static CrossbowManager _crossbow;
    

    // Use this for initialization
    void Start ()
    {
        _sound = GameObject.Find("SoundEffects").GetComponent<SoundEffectsManager>();
        _crossbow = GameObject.Find("CrossbowManager").GetComponent<CrossbowManager>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (rigidB != null)
            transform.eulerAngles = new Vector3(0, 0, (Mathf.Atan(rigidB.velocity.y / rigidB.velocity.x) * Mathf.Rad2Deg));
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        //Hitting the ground      
        if (other.name == "Ground")
        {
            _sound.PlayGroundImpact();
            StartCoroutine(WaitToDestroy());
        }

        //Hitting an enemy
        else if(other.tag == "Enemy")
        {
            //if it's the killing shot plays the juicier sound 
            if (other.GetComponent<BasicEnemy>().health == 1)
                _sound.playDeathImpact();

            //normal impact shots
            else
                _sound.playArrowImpact();



            //set splatter particle to either normal shot or death shot
            GameObject splatter;

            //the killing shot has a bigger splatter
            if (other.GetComponent<BasicEnemy>().health <= 1)
                splatter = deathParticle.gameObject;

            //non killing shot has a smaller splatter
            else
                splatter = impactParticle.gameObject;

            //insantiate blood particle effect
            Instantiate(splatter, new Vector3(other.transform.position.x + .1f, transform.position.y - .2f, other.transform.position.z + 1), impactParticle.transform.rotation);

            //set rotation of blood according to direction of arrow, 
            //particleSystem coordinates seem to have axes rotated (the z coordinates of the arrow rotation are needed in the x-component rotation of the particle for desired effect)
            if (rigidB != null)
                splatter.GetComponent<Transform>().eulerAngles = new Vector3(-rigidB.velocity.y * 7, 90, 0);

            //reduce health
            other.GetComponent<BasicEnemy>().reduceHealth(damage);
            //push enemy back

            //if enemy grounded, push back 
            if (other.GetComponent<BasicEnemy>().grounded)
                other.GetComponent<Rigidbody2D>().AddForce(new Vector2(500, 0));

            //if enemy is airborne, reduce the force 
            else
                other.GetComponent<Rigidbody2D>().AddForce(new Vector2(50, 0));

            //if hit enemy or powerBall, destroy on impact
            if (other.tag == "Enemy" || other.tag == "PowerBall")
            {
                Destroy(gameObject);
            }

            //else wait to destroy to keep arrow buried in the ground, destroy collider and trail
            else
            {
                StartCoroutine(WaitToDestroy());
                Destroy(GetComponent<BoxCollider2D>());
                Destroy(trailrenderer);
            }
        }
    }

    public IEnumerator WaitToDestroy()
    {
        Destroy(rigidB);
        Destroy(trailrenderer);
        Destroy(GetComponent<BoxCollider2D>());
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }
}
