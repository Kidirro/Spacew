using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissiles : MonoBehaviour
{
    public GameObject target;
    private Rigidbody2D rb2D;
    public int damage;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    float CheckAngle()
    {
        float angle=Mathf.Sign(transform.position.x -target.transform.position.x) * Vector2.Angle(transform.position - target.transform.position, Vector2.up);
        return angle;
    }

    void Update()
    {
        if (target)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 180 - CheckAngle()), 0.1f);
            rb2D.velocity = new Vector2(-Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.PI / 180) * 20, Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.PI / 180) * 20);
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
}
