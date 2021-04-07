using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDefault : MonoBehaviour
{
    [Min(1)]
    public int def_health;
    protected int health;

    protected int multiplier=1;
    public int Hp
    {
        get
        {
            return this.health;
        }
        set
        {
            this.health = value;
        }
    }
    public Rigidbody2D rb;
    public float speed;

    [Range(1, 10)]
    public int damage;

    [Range(1, 10)]
    public int price;

    protected GameObject last;
    protected float H;

    virtual protected void death()
    {
        GameManager.score += (int) (price*multiplier);
        this.gameObject.SetActive(false);
    }

    virtual protected void Update()
    {
        if (health <= 0)
        {
            death();
        }
    }

    virtual protected void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case ("Bullet"):
                health = health - 1;
                other.gameObject.SetActive(false);
                break;
            case ("Shield"):
                death();
                break;
            case ("SafeStation"):
                gameObject.SetActive(false);
                break;

        }
    }

    virtual protected void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.take_damage(damage, 0, false, this.gameObject,multiplier);
            death();
        }
    }

    virtual protected void OnParticleCollision(GameObject other)
    {
        switch (other.gameObject.tag)
        {
            case ("GausBullet"):
                if (other != last)
                {
                    health -= 2;
                    last = other;
                }
                break;
            case ("Shield"):
            case ("Player"):
                GameManager.take_damage(0, price, true, this.gameObject, multiplier);
                break;
        }
    }

    virtual protected void OnEnable()
    {
        if (GameManager.EnemyOnScreen == null) GameManager.EnemyOnScreen = new List<EnemyDefault>();
        GameManager.EnemyOnScreen.Add(this);
    }

    virtual protected void OnDisable()
    {
        GameManager.EnemyOnScreen.Remove(this);
    }
}
