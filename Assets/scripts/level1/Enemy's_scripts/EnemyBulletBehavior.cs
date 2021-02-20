using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBehavior : MonoBehaviour
{
    public Rigidbody2D rb;
    public int speed;

    [Range(1, 5)]
    public int damage_p;

    static public int damage;
    private GameObject ammo;

    public void Awake()
    {
        ammo = this.gameObject;
        damage = damage_p;
    }

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(-Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.PI / 180) * speed, Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.PI / 180) *speed));

    }

    private void Update()
    {
        if (ammo.activeSelf == true && ammo.transform.position.y< Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).y -10)
        {

            ammo.SetActive(false);

        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Shield"))
        {
            gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.take_damage(damage, 0, true, this.gameObject);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("Shield") || other.gameObject.CompareTag("Player"))
        {
            GameManager.take_damage(0, 0, true, this.gameObject);
        }
    }

    private void OnParticleTrigger()
    {
        this.gameObject.SetActive(false);
    }
}
