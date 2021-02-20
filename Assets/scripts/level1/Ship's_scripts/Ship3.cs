using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship3 : DefaultShip
{
    private int skill_count=4;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    public void skill_mass_shot()
    {
        for (int i = 0; i < skill_count; i++)
        {
            GameObject ammo = RocketPool[RocketPoolLimit - i -1];

            ammo.transform.position = transform.position;
            ammo.transform.rotation = Quaternion.Euler(0f, 0f, (float)(-15 + i * 30 / skill_count));
            ammo.SetActive(true);
        }
        Shot_source.PlayOneShot(rocket_sound);
    }
}