using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBallSpawnManager : MonoBehaviour
{
    public GameObject powerball;
    public float spawnDelay;

    private float _timer;
    private GameObject _powerballInstance;
	// Use this for initialization
	void Start ()
    {
        _timer = spawnDelay;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (_powerballInstance == null)
            _timer -= Time.deltaTime;

		if(_timer <= 0 && _powerballInstance == null)
        {
            _timer = spawnDelay;

            Vector3 randomPosition = new Vector3(transform.position.x + Random.Range(-8, 9), transform.position.y + Random.Range(-4, 5), transform.position.z);
            _powerballInstance = Instantiate(powerball, randomPosition, powerball.transform.rotation);
        }

	}
}
