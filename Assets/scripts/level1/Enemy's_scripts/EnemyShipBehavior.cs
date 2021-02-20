using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipBehavior : MonoBehaviour
{
    public GameObject Ship;    //Big Ship Little Ship
    public int health;
    public Rigidbody2D rb;
    public float speed;

    private double last_fire;
    public float fire_rate;

    public int AmmoPoolLimit;
    public GameObject Ammo;
    private List<GameObject> AmmoPool;

    [Range(1, 5)]
    public int damage_p;
    static public int damage;

    [Range(1, 5)]
    public int price_p;
    static public int price;

    private GameObject last;

    private float H;
    private IEnumerator co;



    private void Awake()
    {
        damage = damage_p;
        price = price_p;

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
    }

    private void OnEnable()
    {
        health = 5;
        rb = GetComponent<Rigidbody2D>();
        co = Shooting();
        StartCoroutine(co);
        H = Random.Range(Camera.main.ViewportToWorldPoint(new Vector2(0, 0.7f)).y, Camera.main.ViewportToWorldPoint(new Vector2(0, 0.9f)).y);
    }

    private void Update()
    {
        if (transform.position.y!=H & Time.timeScale==1)
        {
            transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, H), 0.01f);
        }
        if (health <= 0)
        {
            death();
        }
    }

    private void death()
    {
        GameManager.score += price;
        StopCoroutine(co);
        Ship.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {

            health = health - 1;
            other.gameObject.SetActive(false);
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
            GameManager.take_damage(damage, price, true, this.gameObject);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("GausBullet")&&other!=last)
        {
            health -=2;
            last = other;
        }
        if (other.gameObject.CompareTag("Shield") || other.gameObject.CompareTag("Player"))
        {
            health = 0;
        }
    }

    IEnumerator Shooting()
    {
        while (true)
        {
            yield return new WaitForSeconds(fire_rate);
            bool shoot = true;
            int i = 0;
            while (shoot & i < AmmoPoolLimit)
            {
                GameObject ammo = AmmoPool[i];
                if (shoot & ammo.activeSelf == false)
                {
                    ammo.transform.position = transform.position;
                    shoot = false;
                    ammo.transform.rotation = transform.rotation;
                    ammo.SetActive(true);
                }
                else i++;
            }
        }

    }
}
