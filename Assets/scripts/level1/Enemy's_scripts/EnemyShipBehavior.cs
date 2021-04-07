using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipBehavior : EnemyShipDefault
{
    protected override void OnEnable()
    {
        base.OnEnable();
        co = Shooting();
        StartCoroutine(co);
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
                    ammo.transform.position = transform.position;
                    shoot = false;
                    ammo.transform.rotation = transform.rotation;
                    ammo.SetActive(true);
                }
                else i++;
            }
        }

    }
}
