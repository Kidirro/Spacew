using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUP_BoostShoot : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.fire_rate = GameManager.fire_rate * 0.9;
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (!GameManager.gameOver)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - (float)0.1);
        }
        if (transform.position.y - Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).y < -10 || GameManager.health==0)
        {
            Destroy(this.gameObject);
        }
    }
}
