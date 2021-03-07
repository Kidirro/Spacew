using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultShip : MonoBehaviour
{

    protected int RocketPoolLimit = 10;
    protected List<GameObject> RocketPool;
    protected double defaultfire_rate_rocket =0.5;
    protected AudioClip rocket_sound;
    protected AudioSource Shot_source;
    protected GameObject Rocket;

    protected bool following;
    protected GameObject player;

    private bool afk = false;

    protected float Range=10;

    protected Rigidbody2D rg2D;
    private Camera cam;

    [Header("Ship parametrs")]
    [Range(1, 10)]
    public int defaulthealth = 5;

    public int[] unlocked_skills;

    
    private IEnumerator co;

    protected virtual void OnEnable()
    {
        GameManager.defaulthealth = defaulthealth;
        if (unlocked_skills != null)
        {
            for (int i = 0; i < unlocked_skills.Length; i++) {
                GameManager.skills[unlocked_skills[i]].state = 2;
            }
            co = null;
        }
    }

    protected virtual void Awake()
    {
        if (Shot_source == null)
        {
            Shot_source = this.gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        }
        cam = Camera.main;
        rg2D = GetComponent<Rigidbody2D>();
        player = this.gameObject;
        Rocket = Resources.Load("Pref/GausBullet", typeof(GameObject)) as GameObject;
        rocket_sound = Resources.Load<AudioClip>("Audio/Spermi");
    }

    protected virtual void Update()
    {
        if (!GameManager.gameOver & this.gameObject.activeSelf)
        {
            if (GameManager.skills[1].state == 2 & co == null)
            {      
                Debug.Log("Start_fire");
                co = Rocket_shot();
                StartCoroutine(co);

            }

            move();
            death();
        }
        GameManager.following = following;
    }

    protected virtual void move()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {

            Vector2 mousePos = Input.mousePosition;
            Vector2 point = cam.ScreenToWorldPoint(new Vector2(mousePos.x, mousePos.y));
            if (((new Vector2(player.transform.position.x, player.transform.position.y) - point).magnitude < Range || following))
            {
                rg2D.velocity = Vector2.zero;
                point.x = Mathf.Clamp(point.x,Camera.main.ViewportToWorldPoint(new Vector2(0.05f, 0.05f)).x, Camera.main.ViewportToWorldPoint(new Vector2(0.95f, 0.95f)).x);
                point.y = Mathf.Clamp(point.y,Camera.main.ViewportToWorldPoint(new Vector2(0.05f, 0.05f)).y, Camera.main.ViewportToWorldPoint(new Vector2(0.95f, 0.95f)).y);
                player.transform.position = point;
                following = true;
            }
            afk = false;
        }
        else
        {
            following = false;
            if (!afk)
            {
                afk = true;
                //StartCoroutine(Jiggle_afk(transform.position, Mathf.Clamp((int)(GameManager.max_health - GameManager.health), 0, 5)));
            }
        }
    }

    protected virtual void death()
    {
        if (GameManager.health <= 0 || Input.GetKeyDown(KeyCode.D))
        {
            GameManager.health = 0;
            StopAllCoroutines();
            player.SetActive(false);
        }
    }

    protected virtual IEnumerator Jiggle_afk(Vector2 start_pos, float range)
    {
        yield return new WaitForSecondsRealtime(1);
        transform.position = Vector2.Lerp(transform.position, new Vector2(start_pos.x, start_pos.y + range), 0.02f);
        while (!following)
        {
            if (Mathf.Abs(transform.position.y - start_pos.y - range) <= Mathf.Abs(range / 5))
            {
                range = -range;
            }
            transform.position = Vector2.Lerp(transform.position, new Vector2(start_pos.x, start_pos.y + range), 0.035f);
            yield return new WaitForEndOfFrame();
        }
    }

    protected IEnumerator Rocket_shot()
    {
        if (RocketPool == null)
        {
            RocketPool = new List<GameObject>();
            for (int i = 0; i < RocketPoolLimit; i++)
            {
                GameObject ammoObject = Instantiate(Rocket);
                ammoObject.transform.position = transform.position;
                ammoObject.SetActive(false);
                RocketPool.Add(ammoObject);
            }
        }
        while (true)
        {
            int i = 0;
            bool shoot = true;
            while (shoot & i < RocketPoolLimit & gameObject.layer == 12)
            {
                GameObject ammo = RocketPool[i];
                if (ammo.activeSelf == false)
                {
                    ammo.transform.position = transform.position;
                    shoot = false;
                    ammo.transform.rotation = transform.rotation;
                    ammo.SetActive(true);
                    if (GameManager.Using_sound) Shot_source.PlayOneShot(rocket_sound);
                }
                else i++;
            }
            yield return new WaitForSeconds((float)defaultfire_rate_rocket);
        }
    }
}
