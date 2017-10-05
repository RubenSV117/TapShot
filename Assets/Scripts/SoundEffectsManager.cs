using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    public AudioSource normalArrowShot;
    public AudioSource[] arrowImpact;
    public AudioSource deathImpact;
    public AudioSource groundImpact;
    public AudioSource arrowRainShot;
    public AudioSource mainArrowRainImpact;
    public AudioSource[] arrowRainImpact;
    public AudioSource tripleShot;
    public AudioSource[] tripleShotImpact;
    public AudioSource powerballPop;
    public AudioSource knockBackShot;
    public AudioSource knockBackImpact;
    public AudioSource poisonImpact;
    public AudioSource mindControlArrowImpact;
    public AudioSource mindControlOrbImpact;

    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {

        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    public void playNormalArrowShot()
    {
        if (PlayerPrefs.GetInt("PlaySound", 1) == 1)       
            normalArrowShot.Play();
    }

    public void playArrowImpact()
    {
        if (PlayerPrefs.GetInt("PlaySound", 1) == 1)
            arrowImpact[Random.Range(0, arrowImpact.Length)].Play();
    }

    public void playDeathImpact()
    {
        if (PlayerPrefs.GetInt("PlaySound", 1) == 1)
            deathImpact.Play();
    }

    public void PlayGroundImpact()
    {
        if (PlayerPrefs.GetInt("PlaySound", 1) == 1)
            groundImpact.Play();                
    }

    public void PlayTripleShot()
    {
        if (PlayerPrefs.GetInt("PlaySound", 1) == 1)
            tripleShot.Play();
    }

    public void PlayTripleShotImpact()
    {
        if (PlayerPrefs.GetInt("PlaySound", 1) == 1)
        {
            tripleShotImpact[Random.Range(0, tripleShotImpact.Length)].Play();
        }
    }

    //first lightning arrow
    public void PlayArrowRainShot()
    {
        if (PlayerPrefs.GetInt("PlaySound", 1) == 1)
            arrowRainShot.Play();
    }

    //when the first lighting arrow lands
    public void PlayMainArrowRainImpact()
    {
        if (PlayerPrefs.GetInt("PlaySound", 1) == 1)
            mainArrowRainImpact.Play();
    }

    //sounds for rain arrows
    public void PlayArrowRainImpact()
    {
        if (PlayerPrefs.GetInt("PlaySound", 1) == 1)      
            arrowRainImpact[Random.Range(0, arrowRainImpact.Length)].Play();
    }

    public void PlayPowerballPop()
    {
        if (PlayerPrefs.GetInt("PlaySound", 1) == 1)
            powerballPop.Play();
    }

    public void PlayKnockBackImpact()
    {
        if (PlayerPrefs.GetInt("PlaySound", 1) == 1)
            knockBackImpact.Play();
    }

    public void PlayKnockBackShot()
    {
        if (PlayerPrefs.GetInt("PlaySound", 1) == 1)
            knockBackShot.Play();
    }

    public void PlayPoisonImpact()
    {
        if (PlayerPrefs.GetInt("PlaySound", 1) == 1)
            poisonImpact.Play();
    }

    public void PlayMindControlArrowImpact()
    {
        if (PlayerPrefs.GetInt("PlaySound", 1) == 1)
            mindControlArrowImpact.Play();
    }

    public void PlayMindControlOrbImpact()
    {
        if (PlayerPrefs.GetInt("PlaySound", 1) == 1)
            mindControlOrbImpact.Play();
    }


}
