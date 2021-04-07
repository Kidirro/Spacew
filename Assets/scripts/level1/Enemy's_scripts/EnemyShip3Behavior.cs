using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip3Behavior : EnemyShipDefault
{
    public GameObject pos1;
    public GameObject pos2;

    private bool left = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        co = Shooting();
        StartCoroutine(co);
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void death()
    {
        base.death();
    }

    protected override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
    }

    protected override void OnParticleCollision(GameObject other)
    {
        base.OnParticleCollision(other);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }

    protected override void Update()
    {
        base.Update();
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
                    
                    ammo.transform.position = (left)? pos1.transform.position: pos2.transform.position;
                    shoot = false;
                    ammo.transform.rotation = transform.rotation;
                    ammo.transform.rotation = Quaternion.Euler(0, 0, 180f);
                    ammo.SetActive(true);
                    left = !left;
                }
                else i++;
            }
        }

    }
}
