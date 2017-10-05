using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public int health;
    public float speed;
    public float attackCooldown;
    public bool grounded;
    public SpriteRenderer spriteRend;
    public bool BeginMindControl;
    public bool isMindControlled;
    public GameObject nearestEnemyToAttack;

    protected float _attackCooldownTimer;
    protected GameObject _tower;
    protected Rigidbody2D _rigidB;
    protected SoundEffectsManager _sound;
    


	// Use this for initialization
	protected void Start ()
    {
        _tower = GameObject.Find("Tower");
        _rigidB = GetComponent<Rigidbody2D>();
        _attackCooldownTimer = attackCooldown;
        _sound = GameObject.Find("SoundEffects").GetComponent<SoundEffectsManager>();
        isMindControlled = false;
        BeginMindControl = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(health <= 0)
        {
            _sound.playDeathImpact();
            Destroy(gameObject);
        }

        if(grounded && !isMindControlled)
        {
            _rigidB.velocity = new Vector2(-speed, _rigidB.velocity.y);
        }

        //begin mind control, start coroutine to reset isMindControlled
        if(BeginMindControl)
        {           
            BeginMindControl = false;
            isMindControlled = true;

            mindControl();         
        }  

        if(isMindControlled)
        {
            if(nearestEnemyToAttack != null)
                _rigidB.velocity = (Vector2)(Vector3.Normalize(nearestEnemyToAttack.transform.position - transform.position) * (speed * 1.5f));
        }

        //if this mindControlled enemy managed to kill the nearest enemy, move on to the next one
        if (isMindControlled && nearestEnemyToAttack == null)
        {
            mindControl();
        }

        _attackCooldownTimer -= Time.deltaTime;

    }

    public IEnumerator wait5secs()
    {
        yield return new WaitForSeconds(5);
        isMindControlled = false;
        spriteRend.color = new Color(1, 0, 0);
        transform.gameObject.layer = 11;
    }



    public void mindControl()
    {
        transform.gameObject.layer = 12;
        StartCoroutine(wait5secs());
        
        spriteRend.color = new Color(0, 1, 0);

        GameObject[] fellowEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        //greater than 2 because it includes THIS enemy automatically
        if(fellowEnemies.Length > 2)
        {
            //get first enemy on the list that isnt this 
            for(int i = 0; i < fellowEnemies.Length; i++)
            {
                if(fellowEnemies[i] != this.gameObject)
                {
                    nearestEnemyToAttack = fellowEnemies[i];
                    break;
                }
            }

            //get nearest fellowEnemy to attack
            for (int i = 0; i < fellowEnemies.Length; i++)
            {
                if ((((fellowEnemies[i].transform.position - transform.position).magnitude) < ((nearestEnemyToAttack.transform.position - transform.position).magnitude)) && fellowEnemies[i] != this.gameObject)
                    nearestEnemyToAttack = fellowEnemies[i];
            }
        }
    }

    public void reduceHealth(int damage)
    {
        health -= damage;
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.name == "Tower")
        {
            other.gameObject.GetComponent<TowerManager>().reduceHealth();
            _attackCooldownTimer = attackCooldown;
            print("attacked");
        }

        if (other.gameObject.tag == "Enemy" && isMindControlled)
        {
            other.gameObject.GetComponent<BasicEnemy>().reduceHealth(3);
            _attackCooldownTimer = attackCooldown;
        }

            if (other.gameObject.tag == "Ground")
        {
            grounded = true;
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }

    public void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.name == "Tower" && _attackCooldownTimer <= 0)           
        {
            _attackCooldownTimer = attackCooldown;
            other.gameObject.GetComponent<TowerManager>().reduceHealth();
            print("attackedAgain");
        }

        if (other.gameObject.tag == "Enemy" && isMindControlled && _attackCooldownTimer <= 0)
        {
            _attackCooldownTimer = attackCooldown;
            other.gameObject.GetComponent<BasicEnemy>().reduceHealth(3);
            _attackCooldownTimer = attackCooldown;
        }



        if (other.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }

    public void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            grounded = false;
        }
    }


}
