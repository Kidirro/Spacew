using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip2Behavior : MonoBehaviour
{
    private GameObject Gun;
    public int health;
    public int Gun_Health;
    public Rigidbody2D rb;
    public float speed;

    [Range(1, 5)]
    public int damage_p;
    static public int damage;

    [Range(1, 5)]
    public int price_p;
    static public int price;

    public int AmmoPoolLimit;
    public GameObject Ammo;
    private List<GameObject> AmmoPool;
    public float fire_rate;

    private GameObject last;
    private float H;
    private int def_health;
    private int def_gun_health;

    private IEnumerator co;

    private void Awake()
    {
        if (Gun == null)
        {
            Gun = this.transform.GetChild(0).gameObject;
        }
        damage = damage_p;
        price = price_p;
        def_gun_health = Gun_Health;
        def_health = health;

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
        health = def_health;
        Gun_Health = def_gun_health;
        rb = GetComponent<Rigidbody2D>();
        H = Random.Range(Camera.main.ViewportToWorldPoint(new Vector2(0, 0.7f)).y, Camera.main.ViewportToWorldPoint(new Vector2(0, 0.9f)).y);
        co = ActiveGun();
        StartCoroutine(co);
    }

    private void Update()
    {
        if (transform.position.y != H & Time.timeScale == 1)
        {
            transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, H), 0.01f);
        }
        if (health <= 0)
        {
            death();
        }
        if (Gun_Health <= 0)
        {
            StopCoroutine(co);
        }
    }

    private void death()
    {
        GameManager.score += price;
        StopCoroutine(co);
        this.gameObject.SetActive(false);
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
        if (other.gameObject.CompareTag("GausBullet") && other != last)
        {
            health -= 2;
            last = other;
        }
        if (other.gameObject.CompareTag("Shield") || other.gameObject.CompareTag("Player"))
        {
            GameManager.take_damage(0, price, true, this.gameObject);
        }
    }

    IEnumerator ActiveGun()
    {
        float last_shot=0;
        GameObject player=null;
        while (true)
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
            if (player)
            {
                Gun.transform.rotation = Quaternion.Lerp(Gun.transform.rotation, Quaternion.Euler(0, 0, Mathf.Sign(player.transform.position.x - Gun.transform.position.x) * Vector2.Angle(Gun.transform.position - player.transform.position, Vector2.up)), 0.1f);
            }
            yield return new WaitForEndOfFrame();
            if (last_shot+ fire_rate<= ShipSpawner.time_count)
            {
                bool shoot = true;
                int i = 0;
                while (shoot & i < AmmoPoolLimit)
                {
                    GameObject ammo = AmmoPool[i];
                    if (shoot & ammo.activeSelf == false)
                    {
                        ammo.transform.position = Gun.transform.position;
                        shoot = false;
                        ammo.transform.rotation = Quaternion.Euler(0, 0, 180 + Gun.transform.rotation.eulerAngles.z);
                        ammo.SetActive(true);
                        last_shot = (float)ShipSpawner.time_count;
                    }
                    else i++;
                }
            }
        }
    }
   
}
