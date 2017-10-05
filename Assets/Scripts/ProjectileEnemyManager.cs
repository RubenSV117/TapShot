using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemyManager : BasicEnemy
{
    /*Inherited
    public int health;
    public float speed;
    public float attackCooldown;
    public bool grounded;
    public SpriteRenderer spriteRend;
    public bool BeginMindControl;
    public bool isMindControlled;
    public GameObject nearestEnemyToAttack;

    private float _attackCooldownTimer;
    private GameObject _tower;
    private Rigidbody2D _rigidB;
    private SoundEffectsManager _sound;
    */

    public GameObject projectile;

    private bool _withinRange;

    // Use this for initialization
    new void Start ()
    {
        _tower = GameObject.Find("Tower");
        _rigidB = GetComponent<Rigidbody2D>();
        _attackCooldownTimer = attackCooldown;
        _sound = GameObject.Find("SoundEffects").GetComponent<SoundEffectsManager>();
        isMindControlled = false;
        BeginMindControl = false;
        _withinRange = true;
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (health <= 0)
        {
            _sound.playDeathImpact();
            Destroy(gameObject);
        }

        if (grounded && !isMindControlled)
        {
            _rigidB.velocity = new Vector2(-speed, _rigidB.velocity.y);
        }

        //begin mind control, start coroutine to reset isMindControlled
        if (BeginMindControl)
        {
            BeginMindControl = false;
            isMindControlled = true;

            mindControl();
        }

        if (isMindControlled)
        {
            if (nearestEnemyToAttack != null)
                _rigidB.velocity = (Vector2)(Vector3.Normalize(nearestEnemyToAttack.transform.position - transform.position) * (speed * 1.5f));
        }

        //if this mindControlled enemy managed to kill the nearest enemy, move on to the next one
        if (isMindControlled && nearestEnemyToAttack == null)
        {
            mindControl();
        }

        _attackCooldownTimer -= Time.deltaTime;

        if (_attackCooldownTimer <= 0)
        {
            _attackCooldownTimer = attackCooldown;
            shootProjectile();
        }
	}

    public void shootProjectile()
    {
        if (_tower != null && _withinRange)
        {
            if(Mathf.Abs(_tower.transform.position.x - transform.position.x) < 15)
            {
                GameObject projectileInstance = Instantiate(projectile, transform.position, projectile.transform.rotation);

                Vector2 shootVector = ((Vector2)(_tower.transform.position - transform.position));

                shootVector = new Vector2(shootVector.x, shootVector.y + 3);

                projectileInstance.GetComponent<Rigidbody2D>().velocity = shootVector;
            }
            
        }
    }

    public void OnBecameInvisible()
    {
        _withinRange = false;
    }

    public void OnBecameVisible()
    {
        _withinRange = true;
    }
}
