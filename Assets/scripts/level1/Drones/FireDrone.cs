using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDrone : DefaultDrone
{
    public int AmmoPoolLimit;
    public GameObject Ammo;
    private List<GameObject> AmmoPool;
    
    [Range(0, 2)]
    public float fire_rate;

    public GameObject pos_fire;

    public override void Start()
    {
        base.Start();
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

    public void OnEnable()
    {
        if (AmmoPool == null)
        {
            AmmoPool = new List<GameObject>();
            for (int i = 0; i < AmmoPoolLimit; i++)
            {
                GameObject ammoObject = Instantiate(Ammo);
                ammoObject.transform.position = transform.position;
                ammoObject.SetActive(false);
                AmmoPool.Add(ammoObject);
            }
            foreach (GameObject ammo in AmmoPool)
            {
                ammo.SetActive(false);
            }
        }
        StartCoroutine(Shoot_coroutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        foreach (GameObject ammo in AmmoPool)
        {
            if (ammo)  ammo.SetActive(false);
        }
    }

    public override void Update()
    {
        base.Update();
    }

    IEnumerator Shoot_coroutine()
    {
        while (true)
        {
            bool shoot = true;
            foreach (GameObject ammo in AmmoPool)
            {
                if (shoot & ammo.activeSelf == false & gameObject.GetComponent<Collider2D>().enabled)
                {
                     ammo.transform.position = pos_fire.transform.position;

                    shoot = false;
                    ammo.transform.rotation = transform.rotation;
                    ammo.SetActive(true);
                }
                
            }
            yield return new WaitForSeconds(fire_rate);
        }
    }
}
