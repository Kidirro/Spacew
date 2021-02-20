using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship1 : DefaultShip
{

    public GameObject pos_fire1;
    public GameObject pos_fire2;

    public int AmmoPoolLimit;
    public GameObject Ammo;
    private List<GameObject> AmmoPool;
    [Range((float)0.1, 1)]
    public double defaultfire_rate = 0.5;

    public AudioClip shoot_sound;

    protected override void OnEnable()
    {
        base.OnEnable();
        foreach (GameObject ammo in AmmoPool)
        {
            ammo.SetActive(false);
        }
        StartCoroutine(Shoot_coroutine());
    }

    protected override void Awake()
    {
        base.Awake();
        shoot_sound=Resources.Load<AudioClip>("Audio/lazer_shoot");
        if (AmmoPool == null)
        {
            AmmoPool = new List<GameObject>();
        }
        for (int i = 0; i < AmmoPoolLimit; i++)
        {
            GameObject ammoObject = Instantiate(Ammo);
            ammoObject.transform.position = transform.position;
            ammoObject.SetActive(false);
            AmmoPool.Add(ammoObject);
        }
    }

    protected override void Update()
    {
        base.Update();
        delete_ammo();      
    }

    protected void delete_ammo()
    {
        foreach (GameObject ammo in AmmoPool)
        {
            if (ammo.activeSelf == true && ammo.transform.position.y - Camera.main.ViewportToWorldPoint(new Vector2(1, 1)).y > 10)
            {

                ammo.SetActive(false);

            }
        }
    }

    protected IEnumerator Shoot_coroutine()
    {
        int i = -1;
        while (true)
        {
            bool shoot = true;
            foreach (GameObject ammo in AmmoPool)
            {
                if (shoot & ammo.activeSelf == false & gameObject.layer == 12)
                {
                    if (i > 0)
                    {
                        ammo.transform.position = pos_fire1.transform.position;
                        i = -i;
                    }
                    else
                    {
                        ammo.transform.position = pos_fire2.transform.position;

                        i = -i;
                    }
                    shoot = false;
                    ammo.transform.rotation = transform.rotation;
                    ammo.SetActive(true);
                    Shot_source.PlayOneShot(shoot_sound);
                }
            }
            yield return new WaitForSeconds((float)GameManager.fire_rate);
        }
    }
}
