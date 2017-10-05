using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    private ParticleSystem _particle;

    // Use this for initialization
    void Start()
    {
        _particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_particle.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
