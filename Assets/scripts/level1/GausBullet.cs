using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GausBullet : MonoBehaviour
{

    public float speed;
    public GameObject explosion;
    private Rigidbody2D rgb2;
    private bool _first_shoot = true;
    private GameObject exp;

    private void Awake()
    {
        exp = Instantiate(explosion) as GameObject;
        exp.SetActive(false);
    }
    public bool first_shoot
    {
        get
        {
            return first_shoot;
        }
        set
        {
            _first_shoot = value;
        }
    }

    private void OnEnable()
    {
        rgb2 = GetComponent<Rigidbody2D>();
        rgb2.AddForce(new Vector2(-Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.PI / 180) * speed, Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.PI / 180) * speed));
    }

    private void Update()
    {
        if (transform.position.y - Camera.main.ViewportToWorldPoint(new Vector2(1, 1)).y > 10)
        {
            _first_shoot = true;
            this.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (_first_shoot)
        {
            _first_shoot = false;
        }
        else
        {
            if (exp)
            {
                exp.transform.position = transform.position;
                exp.SetActive(true);
                exp.GetComponent<ParticleSystem>().Play();
            }
        }
    }
}
