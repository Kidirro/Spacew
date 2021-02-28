using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip2Behavior : EnemyShipDefault
{
    private GameObject Gun;

    protected override void Awake()
    {
        base.Awake();
        if (Gun == null)
        {
            Gun = this.transform.GetChild(0).gameObject;
        }
       
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        co = ActiveGun();
        StartCoroutine(co);
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
