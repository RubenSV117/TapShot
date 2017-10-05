using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindControlOrbManager : MonoBehaviour
{
    public GameObject targetEnemy;
    public Rigidbody2D rigidB;
    public SoundEffectsManager _sound;

	// Use this for initialization
	void Start ()
    {
        _sound = GameObject.Find("SoundEffects").GetComponent<SoundEffectsManager>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (targetEnemy != null)
        {
            rigidB.velocity = (targetEnemy.transform.position - transform.position).normalized * 5;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            _sound.PlayMindControlOrbImpact();

            other.GetComponent<BasicEnemy>().isMindControlled = true;
            Destroy(gameObject);
        }
            
    }
}
