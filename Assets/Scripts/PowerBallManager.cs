using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBallManager : MonoBehaviour
{
    public float maxForce;
    public float minForce;
    public float directionChangeTimer;
    public ParticleSystem burstParticle;
    public float lifeTimer;
    public ParticleSystem spawnParticle;

    private string _powershotType;
    private float _directionTimer;
    private Rigidbody2D _rigidB;
    private float _forceStrength;
    private float _xDirection;
    private float _yDirection;
    private Vector2 _pushVector;
    private SoundEffectsManager _sound;
    private CrossbowManager _crossbow;

    // Use this for initialization
    void Start ()
    {
        _crossbow = GameObject.Find("CrossbowManager").GetComponent<CrossbowManager>();
        Instantiate(spawnParticle, transform.position, spawnParticle.transform.rotation);

        _rigidB = GetComponent<Rigidbody2D>();
        _directionTimer = directionChangeTimer;

        PlayerPrefs.SetInt("PowerBall", ((PlayerPrefs.GetInt("PowerBall", 0) + 1) % 5));

     

        _sound = GameObject.Find("SoundEffects").GetComponent<SoundEffectsManager>();

        StartCoroutine(wait2Secs());

    }

    // Update is called once per frame
    void Update()
    {
        _directionTimer -= Time.deltaTime;

        if (_directionTimer <= 0)
        {
            _directionTimer = directionChangeTimer;
            Push();
        }

        lifeTimer -= Time.deltaTime;

        if(lifeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator wait2Secs()
    {
        yield return new WaitForSeconds(2);
        Push();
    }

    public void Push()
    {
        //magnitude
        _forceStrength = Random.Range(minForce, maxForce);

        //direction components
         _xDirection = Random.Range(-1f, 1f);
         _yDirection = Random.Range(-1f, 1f);

        _pushVector = _forceStrength * new Vector2(_xDirection, _yDirection);

        _rigidB.AddForce(_pushVector);
    }

    public void OnBecameInvisible()
    {
        transform.position = new Vector3(-transform.position.x, -transform.position.y, transform.position.z);
        _directionTimer += 1;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Arrow")
        {
            _sound.PlayPowerballPop();
            Instantiate(burstParticle, transform.position, burstParticle.transform.rotation);
            if (PlayerPrefs.GetInt("PowerBall", 0) == 0)
                _crossbow.activateTripleShot();

            else if (PlayerPrefs.GetInt("PowerBall", 0) == 1) 
                _crossbow.activateArrowRain();

            else if (PlayerPrefs.GetInt("PowerBall", 0) == 2)
            {
                _crossbow.activateKnockback();
            }

            else if (PlayerPrefs.GetInt("PowerBall", 0) == 3)
            {
                _crossbow.activatePoison();
            }

            else if (PlayerPrefs.GetInt("PowerBall", 0) == 4)
            {
                _crossbow.activateMindControl();
            }

            print(CrossbowManager.powerShot);
            Destroy(gameObject);
        }
    }
}
