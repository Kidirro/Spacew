using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissiles : MonoBehaviour
{
    public GameObject target;
    private Rigidbody2D rb2D;
    public int damage;
    public float speed;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        Debug.LogWarning(rb2D);
        target = new GameObject();
    }

    private void OnEnable()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Homming());

    }

    float CheckAngle()
    {
        float angle=Mathf.Sign(transform.position.x -target.transform.position.x) * Vector2.Angle(transform.position - target.transform.position, Vector2.up);
        return angle;
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

    public IEnumerator Homming()
    {
       
        while (true)
        {
            bool in_screen = (transform.position.x > Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).x & transform.position.x < Camera.main.ViewportToWorldPoint(new Vector2(1, 1)).x) &
                (transform.position.y > Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).y & transform.position.y < Camera.main.ViewportToWorldPoint(new Vector2(1, 1)).y);
            if (target)
            {
                rb2D.velocity = transform.up * speed;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 180 - CheckAngle()), 0.05f);
            }
            if (!in_screen) gameObject.SetActive(false);
            
            yield return new WaitForEndOfFrame();
        }
    }
}
