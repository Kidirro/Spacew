using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipDefault : EnemyDefault
{
    
    public int AmmoPoolLimit;
    public GameObject Ammo;
    protected List<GameObject> AmmoPool;
    public float fire_rate;

    protected IEnumerator co;

    [HideInInspector]
    public int line;

    public int size;

    virtual protected void Awake()
    {
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

    override protected void OnEnable()
    {
        base.OnEnable();
        for (int i = line - size; i<= line+size; i++)
        {
            if (i>=0 & i < Enemy_spawner.Lines_state.Length)
            {
                Enemy_spawner.Lines_state[i]--;

            }
        }
        health = def_health;
        rb = GetComponent<Rigidbody2D>();
        H = Random.Range(Camera.main.ViewportToWorldPoint(new Vector2(0, 0.7f)).y, Camera.main.ViewportToWorldPoint(new Vector2(0, 0.9f)).y);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (transform.position.y != H & Time.timeScale == 1)
        {
            transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, H), 0.01f);
        }

    }

    protected override void death()
    {
        base.death();
        StopCoroutine(co);
    }

    override protected void OnDisable()
    {
        base.OnDisable();
        for (int i = line - size; i <= line + size; i++)
        {
            if (i >= 0 & i < Enemy_spawner.Lines_state.Length)
            {
                Enemy_spawner.Lines_state[i]++;
            }
        }
    }
}
