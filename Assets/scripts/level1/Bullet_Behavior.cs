using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Behavior : MonoBehaviour
{
    public Rigidbody2D rb;
    public int speed;
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(-Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.PI / 180) * speed, Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.PI / 180) * speed));
    }

    private void Update()
    {
        if (transform.position.y - Camera.main.ViewportToWorldPoint(new Vector2(1, 1)).y > 10)
        {

            this.gameObject.SetActive(false);

        }
    }
}
