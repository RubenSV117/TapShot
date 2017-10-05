using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowManager : MonoBehaviour
{
    public GameObject basicArrow;
    public GameObject arrowRainArrow;
    public GameObject tripleShotArrow;
    public GameObject knockBackArrow;
    public GameObject poisonArrow;
    public GameObject mindControlArrow;
    public float shootSpeed;
    public static Transform trans;
    public static string powerShot;
    public static Vector2 shootVector;
    public static int tripleShotCount;
    public static int poisonCount;


    private SoundEffectsManager _sound;
    private GameObject _arrow;
    
    

    // Use this for initialization
    void Start()
    {
        trans = GetComponent<Transform>();
        _sound = GameObject.Find("SoundEffects").GetComponent<SoundEffectsManager>();
        powerShot = "";
        tripleShotCount = 4;
        poisonCount = 3;


    }

    // Update is called once per frame
    void Update()
    {
        if (powerShot == "TripleShot")
        {
            _arrow = tripleShotArrow;
        }

        //once the triple bursts have run out
        if (powerShot == "TripleShot" && tripleShotCount <= 0)
        {
            powerShot = "";
        }

        if (powerShot == "Poison" && poisonCount <= 0)
            powerShot = "";

        else if (powerShot == "ArrowRain")
            _arrow = arrowRainArrow;

        else if (powerShot == "KnockBack")
            _arrow = knockBackArrow;

        else if (powerShot == "Poison")
            _arrow = poisonArrow;

        else if (powerShot == "MindControl")
            _arrow = mindControlArrow;

        else
            _arrow = basicArrow;
           

        if (Input.GetMouseButtonDown(0))
        {
            shootVector = GetMousePosition() - (Vector2)transform.position;
            shootVector = shootVector.normalized * shootSpeed;

            //shoot burst 
            if (powerShot == "TripleShot" && tripleShotCount > 0)
            {
                StartCoroutine(TripleShot());
            }

            //else shoot arrows that activate on impact
            else if (powerShot == "ArrowRain" || powerShot == "KnockBack" || powerShot == "Poison" || powerShot == "MindControl" || powerShot == "")
            {
                GameObject arrowInstance = Instantiate(_arrow, transform.position, transform.rotation);

                arrowInstance.GetComponent<Rigidbody2D>().velocity = shootVector;

                _sound.playNormalArrowShot();
            }

            
           
        }

    }

    public IEnumerator TripleShot()
    {
       
        for (int i = 0; i < 3; i++)
        {
            _arrow = tripleShotArrow;
            GameObject bulletInstance = Instantiate(_arrow, transform.position, transform.rotation);

            Vector2 newShootVector = new Vector2(shootVector.x, shootVector.y + i);

            bulletInstance.GetComponent<Rigidbody2D>().velocity = newShootVector;

            _sound.PlayTripleShot();

            yield return new WaitForSeconds(.13f);
        }

        tripleShotCount--;
    }

    public Vector2 GetMousePosition()
    {
        //coordinates of object are in world coordinates (0,0) at center. Mouse position returns Screen coordinates with (0,0) at bottom left,
        //in order for the mouse and object coordinates to match, must convert mouse coordinates from screen to world
        Vector3 mouseWorldPos3D = Camera.main.ScreenToWorldPoint(Input.mousePosition); //getting x and y from something, dont need new
        Vector2 mouseWorldPos2D = new Vector2(mouseWorldPos3D.x, mouseWorldPos3D.y); //raycastHit2D needs a vector2, giving x and y, needs new
        return mouseWorldPos2D;
    }

    public void activateTripleShot()
    {
        powerShot = "TripleShot";
        tripleShotCount = 4;
    }

    public void activateArrowRain()
    {
        powerShot = "ArrowRain";
    }

    public void activateKnockback()
    {
        powerShot = "KnockBack";
        KnockBackArrowManager.isFirstShot = true;
    }

    public void activatePoison()
    {
        powerShot = "Poison";
        poisonCount = 3;
    }

    public void activateMindControl()
    {
        powerShot = "MindControl";
    }

    public void deactivatePowerShot()
    {
        powerShot = "";
    }


    public void ArrowRain(Vector3 impactPosition)
    {
        _sound.PlayMainArrowRainImpact();
        if (CrossbowManager.powerShot == "ArrowRain")
        {
            CrossbowManager.powerShot = "";
            Vector3 shootVector = (impactPosition - new Vector3(-6, 6, 0)) * 1.5f; ;

            for (int i = 0; i < 6; i++)
            {
                GameObject arrowInstance = Instantiate(_arrow, new Vector3(-6, 6, 0), transform.rotation);
                arrowInstance.GetComponent<RainArrowManager>().isFirstShot = false;
                arrowInstance.GetComponent<Rigidbody2D>().velocity = new Vector3(shootVector.x, (shootVector.y + i + 5), 0);
            }


            shootVector = (impactPosition - new Vector3(6, 6, 0)) * 1.5f; ;
            for (int i = 0; i < 6; i++)
            {
                GameObject arrowInstance = Instantiate(_arrow, new Vector3(6, 6, 0), transform.rotation);
                arrowInstance.GetComponent<RainArrowManager>().isFirstShot = false;
                arrowInstance.GetComponent<Rigidbody2D>().velocity = new Vector3(shootVector.x, (shootVector.y + i + 5), 0);
            }


        }
    }

    public void KnockBack()
    {
        StartCoroutine(KnockBackCo());
    }

    public IEnumerator KnockBackCo()
    {
        _sound.PlayKnockBackImpact();
        Time.timeScale = .5f;

        //returns all object within radius that have a collider2D
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, 20);

        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].gameObject.tag == "Enemy")
            {
                objects[i].gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(100, 200));
                objects[i].gameObject.GetComponent<Rigidbody2D>().gravityScale = .1f;
                objects[i].gameObject.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-1f, 1f) * 80;
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

        powerShot = "";
    }
}
