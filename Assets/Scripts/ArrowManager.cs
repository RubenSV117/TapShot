using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    public Rigidbody2D rigidB;
    public int damage;
    public ParticleSystem impactParticle;
    public ParticleSystem deathParticle;
    public GameObject arrow;
    public GameObject arrowRainParticle;
    public ParticleSystem arrowRainImpact;
    public GameObject tripleShotParticle;
    public GameObject tripleShotImpact;
    public GameObject knockBackParticle;
    public GameObject knockBackParticleImpact;
    public bool isRainArrow;
    public bool isKnockBackArrow;
    public GameObject trailrenderer;

    //after shooting the 3 arrows, CrossbowManager decrements tripleShotCount, so this boolean makes it independant after being shot
    private bool _tripleShoot;
    public static  bool alreadyKnockedBack;
    private SoundEffectsManager _sound;

	// Use this for initialization
	void Start ()
    {
        _tripleShoot = false;
    
        _sound = GameObject.Find("SoundEffects").GetComponent<SoundEffectsManager>();

        //ArrowRain
        if (CrossbowManager.powerShot == "ArrowRain")
        {
            arrowRainParticle.SetActive(true);
            _sound.PlayArrowRainShot();
        }

        //Triple Fire Shot
        if (CrossbowManager.tripleShotCount > 0 && CrossbowManager.powerShot == "TripleShot")
        {
            tripleShotParticle.SetActive(true);
            _tripleShoot = true;
        }

        if(CrossbowManager.powerShot == "KnockBack" && !alreadyKnockedBack)
        {
            isKnockBackArrow = true;
            _sound.PlayKnockBackShot();
            knockBackParticle.SetActive(true);
            
        }

    

   
            
        //transform.eulerAngles = new Vector3(0, 0, rigidB.velocity.y * 7);
    }
	
	// Update is called once per frame
	void Update ()
    { 
        if(rigidB != null)
            transform.eulerAngles = new Vector3(0, 0,  (Mathf.Atan(rigidB.velocity.y / rigidB.velocity.x) * Mathf.Rad2Deg));

        if (CrossbowManager.powerShot == "KnockBack")
        {
            if (GetComponent<Rigidbody2D>() != null)
            {
                GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized * 25 ;
            }
        }
          
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //if AOE powerShot activated, then simulate blast radius and do more damage
        if (isRainArrow || _tripleShoot)
        {
            //returns all object within radius that have a collider2D
            Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, 1.3f);

            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i].gameObject.tag == "Enemy")
                {
                  
                    objects[i].gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(700, 50));
                    objects[i].gameObject.GetComponent<BasicEnemy>().reduceHealth(2);
                }
            }
        }

        //for slo-mo levitating powerUp
        if(isKnockBackArrow && !alreadyKnockedBack)
        {
            alreadyKnockedBack = true;
            Instantiate(knockBackParticleImpact, transform.position, knockBackParticleImpact.transform.rotation);

            //insantiate particle on every enemy
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            for(int i = 0; i < enemies.Length; i++)
            {
                Vector3 particlePosition = new Vector3(enemies[i].transform.position.x, enemies[i].transform.position.y, enemies[i].transform.position.z - 1);
                Instantiate(knockBackParticleImpact, particlePosition, knockBackParticleImpact.transform.rotation);
            }

            StartCoroutine(KnockBack());
        }

        //Hitting the ground, with or without powerShots activated       
        if (other.name == "Ground")
        {
            //ArrowRain
            if (CrossbowManager.powerShot == "ArrowRain")
            {
                //prevent arrows from triggering rainArrow that were shot before shooting the powerBall;
                //only the ones shot after getting the power ball will have the particleSystem active
                if(arrowRainParticle.activeInHierarchy)
                {
                    ArrowRain();
                    Instantiate(arrowRainImpact, transform.position, arrowRainImpact.transform.rotation);
                }
                
            }

            //wave arrows that were spawned after the first lightning arrow lands
            else if (isRainArrow)
            {
                Instantiate(arrowRainImpact, transform.position, arrowRainImpact.transform.rotation);
                _sound.PlayArrowRainImpact();
            }


            //TripleShot
            else if (_tripleShoot)
            {
                Instantiate(tripleShotImpact, transform.position, tripleShotImpact.transform.rotation);
                _sound.PlayTripleShotImpact();
                _tripleShoot = false; ;
            }


            //hits the ground with no powerShot Active
            else
                _sound.PlayGroundImpact();
        }


        //Hitting the enemy with or without powerShots selected
        else if (other.tag == "Enemy")
        {
            //ArrowRain
            if (CrossbowManager.powerShot == "ArrowRain")
            {
                if (arrowRainParticle.activeInHierarchy)
                {
                    ArrowRain();
                    Instantiate(arrowRainImpact, transform.position, arrowRainImpact.transform.rotation);
                }
            }

            //these are the arrows instantiated after the first arrow activates ArrowRain()
            else if (isRainArrow)
            {
                Instantiate(arrowRainImpact, transform.position, arrowRainImpact.transform.rotation);
                _sound.PlayArrowRainImpact();
            }

            //hits enemy with triple shoot activated
            else if (_tripleShoot)
            {
                Instantiate(tripleShotImpact, transform.position, tripleShotImpact.transform.rotation);
                _sound.PlayTripleShotImpact();
                _tripleShoot = false;
            }

            //death shot sound
            if (other.GetComponent<BasicEnemy>().health == 1)
                _sound.playDeathImpact();

            //normal impact shots
            else
                _sound.playArrowImpact();



            //set splatter particle to either normal shot or death shot
            GameObject splatter;

            if (other.GetComponent<BasicEnemy>().health <= 1)
                splatter = deathParticle.gameObject;

            else
                splatter = impactParticle.gameObject;

            //insantiate blood particle effect
             Instantiate(splatter, new Vector3(other.transform.position.x + .1f, transform.position.y - .2f, other.transform.position.z + 1), impactParticle.transform.rotation);

            //set rotation of blood according to direction of arrow
            if(rigidB != null)
                splatter.GetComponent<Transform>().eulerAngles = new Vector3(-rigidB.velocity.y * 7, 90, 0);

            //normal shot
            if (!isRainArrow &&  !_tripleShoot)
            {
                //reduce health
                other.GetComponent<BasicEnemy>().reduceHealth(damage);
                //push enemy back

                if(other.GetComponent<BasicEnemy>().grounded)
                     other.GetComponent<Rigidbody2D>().AddForce(new Vector2(400, 0));

                else
                    other.GetComponent<Rigidbody2D>().AddForce(new Vector2(50, 0));

            }



        }

        if(CrossbowManager.powerShot == "KnockBack")
        {
            StartCoroutine(WaitToDestroy5());
        }

        //prevent arrow from being destroyed immediately so KnockBack coroutine can execute fully (theres a 3 second wait in it)
        //do not interact with player, tower, or other arrows
        else if ((other.tag == "Enemy" || other.tag == "PowerBall" ) && CrossbowManager.powerShot != "KnockBack")
        {
            Destroy(gameObject);
        }

        else
        {
            StartCoroutine(WaitToDestroy());
            Destroy(GetComponent<BoxCollider2D>());
            Destroy(trailrenderer);
        }
            

    }
    
    public IEnumerator KnockBack()
    {
        _sound.PlayKnockBackImpact();
        Time.timeScale = .5f;
       
        //returns all object within radius that have a collider2D
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, 12);

        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].gameObject.tag == "Enemy")
            {
                objects[i].gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(100, 200));
                objects[i].gameObject.GetComponent<Rigidbody2D>().gravityScale = .1f;
                objects[i].gameObject.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-1f, 1f) * 50;
            }
        }

        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1;

        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i] != null)
            {
                if (objects[i].gameObject.tag == "Enemy")
                {
                    objects[i].gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
                }
            }
        }

        CrossbowManager.powerShot = "";
    }

    public IEnumerator WaitToDestroy()
    {
        Destroy(rigidB);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    public IEnumerator WaitToDestroy5()
    {
        Destroy(rigidB);
        Destroy(trailrenderer);
        transform.position = new Vector3(10, 10, 0);
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    public void  ArrowRain()
    {
        _sound.PlayMainArrowRainImpact();
        if (CrossbowManager.powerShot == "ArrowRain")
        {
            CrossbowManager.powerShot = "";
            Vector3 shootVector = (transform.position - new Vector3(-6, 6, 0)) * 1.5f; ;

            for (int i = 0; i < 6; i++)
            {             
                GameObject bulletInstance = Instantiate(arrow, new Vector3(-6, 6, 0), transform.rotation);
                bulletInstance.GetComponent<ArrowManager>().isRainArrow = true;
                bulletInstance.GetComponent<Rigidbody2D>().velocity = new Vector3(shootVector.x, (shootVector.y + i + 3), 0);
            }

           
            shootVector = (transform.position - new Vector3(6, 6, 0)) * 1.5f; ;
            for (int i = 0; i < 6; i++)
            {
                GameObject bulletInstance = Instantiate(arrow, new Vector3(6, 6, 0), transform.rotation);
                bulletInstance.GetComponent<ArrowManager>().isRainArrow = true;
                bulletInstance.GetComponent<Rigidbody2D>().velocity = new Vector3(shootVector.x, (shootVector.y + i + 3), 0);
            }


        }
    }
}
