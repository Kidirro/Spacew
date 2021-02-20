using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO_Behavior : MonoBehaviour
{

    [Range(1,20)]
    public int Default_health; 
    private int health;
    public Rigidbody2D rb;
    public float speed;

    private double last_fire;
    public float fire_rate;

    public int AmmoPoolLimit;
    public GameObject Ammo;
    private List<GameObject> AmmoPool;
    private float t;

    [Header("Места стрельбы")]
    public Transform pos1;
    public Transform pos2;

    private GameObject last;
    private GameObject Ship;

    private void Awake()
    {
        if (AmmoPool == null)
        {
            AmmoPool = new List<GameObject>();
        }
        for (int i = 0; i < AmmoPoolLimit; i++)
        {
            GameObject ammoObject = Instantiate(Ammo);
            ammoObject.transform.position = transform.position;
            ammoObject.SetActive(false);
            AmmoPool.Add(ammoObject);
            
        }
        t = 0;
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(0, -speed * ((float)ShipSpawner.time_count * 0.05f)));

    }

    private void OnEnable()
    {
        Ship = this.gameObject;
        health = Default_health;
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(0, -speed));
        StartCoroutine(Shooting());
    }

    private void Update()
    {
        if (Ship.transform.position.y - Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).y < -10)
        {
            Ship.SetActive(false);
        }
        if (health <= 0)
        {
            death();
        }

        if (!GameManager.gameOver)
        {
            Ship.transform.position = new Vector2(Mathf.Sin(t) *3, Ship.transform.position.y);
            t += 0.05f;
        }
    }

    private void death()
    {
        StopCoroutine(Shooting());
        Ship.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {

            health = health - 1;
            other.gameObject.SetActive(false);
            if (health <= 0)
            {
                GameManager.score += 2;
            }
        }
        else if (other.gameObject.CompareTag("Shield"))
        {
            death();
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("GausBullet") && other != last)
        {
            health -=2;
            GameManager.score += 2;
            last = other;
        }
    }

    IEnumerator Shooting()
    {
        int i = -1;
        while (true)
        {
            yield return new WaitForSeconds(fire_rate);
            bool shoot = true;
            foreach (GameObject ammo in AmmoPool)
            {
                if (shoot & ammo.activeSelf == false)
                {
                    if (i > 0) {
                        ammo.transform.position = pos1.position;
                    }
                    else
                    {
                        ammo.transform.position = pos2.position;
                    }

                    shoot = false;
                    ammo.transform.rotation = Quaternion.Euler(180, 0, 0);
                    ammo.SetActive(true);
                    i = -i;
                    last_fire = ShipSpawner.time_count;
                }
            }
        }
    }
}
