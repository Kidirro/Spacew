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
        int left = -1;
        while (true)
        {
            int i = 0;
            bool shoot = true;
            while (shoot & i < AmmoPoolLimit & gameObject.layer == 12)
            {
                GameObject ammo = AmmoPool[i];
                if (ammo.activeSelf == false)
                {
                    if (left > 0)
                    {
                        ammo.transform.position = pos_fire1.transform.position;
                        left = -left;
                    }
                    else
                    {
                        ammo.transform.position = pos_fire2.transform.position;

                        left = -left;
                    }
                    shoot = false;
                    ammo.transform.rotation = transform.rotation;
                    ammo.SetActive(true);
                    if (GameManager.Using_sound) Shot_source.PlayOneShot(shoot_sound);
                }
                else i++;
            }
            yield return new WaitForSeconds((float)GameManager.fire_rate);
        }
    }

    private void OnDestroy()
    {
        foreach (GameObject ammo in AmmoPool) Destroy(ammo);
        AmmoPool = null;
    }
}
