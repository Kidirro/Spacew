using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipDefault : MonoBehaviour
{
    [Min(1)]
    public int def_health;
    protected int health;
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
    protected List<GameObject> AmmoPool;
    public float fire_rate;

    protected GameObject last;
    protected float H;

    protected IEnumerator co;

   
    public int line;

    public int size;

    virtual protected void Awake()
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

    virtual protected void OnEnable()
    {
        for (int i = line - size; i<= line+size; i++)
        {
            if (i>=0 & i < Enemy_spawner.Lines_state.Length)
            {
                Enemy_spawner.Lines_state[i]--;
                Debug.LogFormat("{0}:{1}", i, Enemy_spawner.Lines_state[i]);

            }
        }
        health = def_health;
        rb = GetComponent<Rigidbody2D>();
        H = Random.Range(Camera.main.ViewportToWorldPoint(new Vector2(0, 0.7f)).y, Camera.main.ViewportToWorldPoint(new Vector2(0, 0.9f)).y);
    }

    // Update is called once per frame
    virtual protected void Update()
    {
            if (transform.position.y != H & Time.timeScale == 1)
            {
                transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, H), 0.01f);
            }
            if (health <= 0)
            {
                death();
            }
       
    }

    virtual protected void death()
    {
        GameManager.score += price;
        StopCoroutine(co);
        this.gameObject.SetActive(false);
    }

    virtual protected void OnTriggerEnter2D(Collider2D other)
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

    virtual protected void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.take_damage(damage, price, true, this.gameObject);
        }
    }

    virtual protected void OnParticleCollision(GameObject other)
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

    private void OnDisable()
    {
        for (int i = line - size; i <= line + size; i++)
        {
            if (i >= 0 & i < Enemy_spawner.Lines_state.Length)
            {
                Enemy_spawner.Lines_state[i]++;
                Debug.LogFormat("{0}:{1}",i,Enemy_spawner.Lines_state[i]);
            }
        }
    }
}
