using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor_behavior : MonoBehaviour
{
    private Rigidbody2D rb;
    public int default_speed;
    private Enemy_spawner ES;
    public bool meteor_array = false;

    public GameObject meteor;
    public int default_health=5;
    private GameObject ExplosionObject;
    public Vector2 Vector_pos;

    [Range(1, 5)]
    public int damage_p;
    static public int damage;

    [Range(1, 5)]
    public int price_p;
    static public int price;

    [HideInInspector]
    public UnityEngine.Object explosion;
    private GameObject Last;
    public int health;

    public void Awake()
    {
        damage = damage_p;
        price = price_p;
        Last = this.gameObject;
        
    }

    public void Start()
    {
        ES = GameObject.FindObjectOfType<Enemy_spawner>();
        if (!meteor_array)
        {
            health = (int) (default_health* transform.localScale.x);
            rb = GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2(0, -default_speed ));
        }
    }

    private void OnEnable()
    {
        health = (int)(default_health * transform.localScale.x);
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(0, -default_speed ));
    }

    private void Update()
    {
        if (meteor.transform.position.y - Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).y < -10)
        {
            if (meteor_array)
            {
                meteor.SetActive(false);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        if (health <= 0)
        {
            Death();
        }
        if (gameObject.activeSelf)
        {
            Vector_pos = gameObject.transform.position;
        }
    }

    public void Death()
    {
        if (gameObject != null)
        {
            GameManager.score += price;
            int j = 0;
            bool shoot = true;
            while (shoot & j <ES.MeteorPSLimit)
            {
                ParticleSystem particle = ES.MeteorPSPool[j];
                if (particle.gameObject.activeSelf == false)
                {
                    particle.gameObject.transform.position = gameObject.transform.position;
                    particle.gameObject.SetActive(true);
                    shoot = false;

                }
                else j++;
            }
            if (transform.localScale.x > 1)
            {
                for (int i = 0; i < Random.Range(2, 4); i++)
                {
                    GameObject new_part = Instantiate(meteor);
                    new_part.transform.position = new Vector2(Vector_pos.x+Random.Range(-5,5)*transform.localScale.x,Vector_pos.y + Random.Range(-5, 5) * transform.localScale.y);
                    new_part.transform.localScale = new Vector2(transform.localScale.x - 1, transform.localScale.y - 1);
                }
            }
            if (meteor_array)
            {
                meteor.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            health = health - 1;
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Shield"))
        {
            Death();
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("GausBullet") && Last != other.gameObject)
        {
            health -= 2;
            Last = other.gameObject;
        }

        if (other.gameObject.CompareTag("Shield") || other.gameObject.CompareTag("Player"))
        {
            GameManager.take_damage(0, 0, false, meteor);
            Death();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("Ship1_prog", PlayerPrefs.GetInt("Ship1_prog")+1 );
            PlayerPrefs.Save();
            Debug.Log(PlayerPrefs.GetInt("Ship1_prog"));
        }
        if (other.gameObject.CompareTag("Shield")|| other.gameObject.CompareTag("Player"))
        {
            GameManager.take_damage(damage, price,false, meteor);
            Death();
        }
    }
}
