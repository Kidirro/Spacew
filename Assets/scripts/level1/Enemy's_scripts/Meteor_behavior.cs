using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor_behavior : EnemyDefault
{
    private Enemy_spawner ES;
    public bool meteor_array = false;

    public GameObject meteor;
    public Vector2 Vector_pos;


    [HideInInspector]
    public UnityEngine.Object explosion;

    public void Start()
    {
        ES = GameObject.FindObjectOfType<Enemy_spawner>();
        if (!meteor_array)
        {
            health = (int) (def_health* transform.localScale.x);
            multiplier = (int)transform.localScale.x;
            rb = GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2(0, -speed ));
        }
    }

    override protected void OnEnable()
    {
        base.OnEnable();
        health = (int)(def_health * transform.localScale.x);
        multiplier = (int)transform.localScale.x;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, -speed);
    }

    protected override void Update()
    {
        base.Update();
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
        if (gameObject.activeSelf)
        {
            Vector_pos = gameObject.transform.position;
        }
    }

    protected override void death()
    {
       
        if (gameObject != null)
        {
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
        base.death();
    }


    protected override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
    }
}
