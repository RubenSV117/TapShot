using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCloudManager : MonoBehaviour
{
    public int damage;
    public float damageCooldown;
    public float lifeTimer;

    private float _cooldownTimer;
    private List<GameObject> _enemies = new List<GameObject>();

	// Use this for initialization
	void Start ()
    {
        _cooldownTimer = damageCooldown;     
    }
	
	// Update is called once per frame
	void Update ()
    {
        _cooldownTimer -= Time.deltaTime;

        if(_cooldownTimer <= 0)
        {
            _cooldownTimer = damageCooldown;
            DamageEnemies();
        }

        lifeTimer -= Time.deltaTime;
        if(lifeTimer <= 0)
        {
            Destroy(gameObject);
        }

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            _enemies.Add(other.gameObject);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            _enemies.Remove(other.gameObject);
        }
    }

    public void DamageEnemies()
    {
        if (_enemies.Count > 0)
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                if(_enemies[i].GetComponent<BasicEnemy>() != null)
                    _enemies[i].GetComponent<BasicEnemy>().reduceHealth(damage);
            }
        }
    }
}
