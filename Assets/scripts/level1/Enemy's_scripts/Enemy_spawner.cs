using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_spawner : MonoBehaviour
{
    public GameObject[] Lines;

    [Header("Метеориты")]
    [Tooltip("Максимальное количество метеоритов")]
    public int MeteorPoolLimit;
    [Tooltip("Ссылка на обьект")]
    public GameObject Meteor;
    private List<GameObject> MeteorPool;
    [Tooltip("Начальная скорость спавна (1 обьект в t секунд)")]
    public double meteor_default_fire_rate = 5;
    [SerializeField]
    [Tooltip("Текущая скорость спавна")]
    private double meteor_fire_rate;
    [Range(0, 1)]
    [Tooltip("Коэффициент смены скорости спавна")]
    public float meteor_koef;
    [Min(0)]
    [Tooltip("Время, после которого начнется спавн")]
    public float meteor_start_time;
    [Tooltip("Лимит частиц метеора")]
    public int MeteorPSLimit;
    [Tooltip("Ссылка на ПартиклСистем")]
    public ParticleSystem MeteorPS;
    [Tooltip("Пул партиклов")]
    public List<ParticleSystem> MeteorPSPool;

    [Header("1 тип кораблей")]
    [Tooltip("Максимальное количество объектов ")]
    public int Ship1PoolLimit;
    [Tooltip("Ссылка на обьект")]
    public GameObject Ship1;
    private List<GameObject> Ship1Pool;
    [Tooltip("Начальная скорость спавна (1 обьект в t секунд)")]
    public double default_fire_rate1 = 5;
    [SerializeField]
    [Tooltip("Текущая скорость спавна")]
    private double fire_rate1;
    [Range(0, 1)]
    [Tooltip("Коэффициент смены скорости спавна")]
    public float koef1;
    [Min(0)]
    [Tooltip("Время, после которого начнется спавн")]
    public float start_time1;

    [Header("2 тип кораблей")]
    [Tooltip("Максимальное количество объектов ")]
    public int Ship2PoolLimit;
    [Tooltip("Ссылка на обьект")]
    public GameObject Ship2;
    private List<GameObject> Ship2Pool;
    [Tooltip("Начальная скорость спавна (1 обьект в t секунд)")]
    public double default_fire_rate2 = 5;
    [SerializeField]
    [Tooltip("Текущая скорость спавна")]
    private double fire_rate2;
    [Range(0, 1)]
    [Tooltip("Коэффициент смены скорости спавна")]
    public float koef2;
    [Min(0)]
    [Tooltip("Время, после которого начнется спавн")]
    public float start_time2;

    static public int[] Lines_state;

    public void Awake()
    {
        Lines_state = new int[Lines.Length];
        for (int i = 0; i < Lines.Length; i++) Lines_state[i] = 1;
       
        if (MeteorPool == null)
        {
            MeteorPool = new List<GameObject>();
        }

        for (int i = 0; i < MeteorPoolLimit; i++)
        {
            GameObject MeteorObject = Instantiate(Meteor);
            MeteorObject.transform.position = transform.position;
            MeteorObject.GetComponent<Meteor_behavior>().meteor_array = true;
            MeteorObject.SetActive(false);
            MeteorPool.Add(MeteorObject);
        }

        if (MeteorPSPool == null)
        {
            MeteorPSPool = new List<ParticleSystem>();
        }

        for (int i = 0; i < MeteorPSLimit; i++)
        {
            ParticleSystem particleSystem = Instantiate(MeteorPS);
            particleSystem.gameObject.transform.position = gameObject.transform.position;
            particleSystem.gameObject.SetActive(false);
            MeteorPSPool.Add(particleSystem);
        }

        if (Ship1Pool == null)
        {
            Ship1Pool = new List<GameObject>();
        }
        for (int i = 0; i < Ship1PoolLimit; i++)
        {
            GameObject ShipObject = Instantiate(Ship1);
            ShipObject.transform.position = transform.position;
            ShipObject.SetActive(false);
            Ship1Pool.Add(ShipObject);
            
        }  
        
        if (Ship2Pool == null)
        {
            Ship2Pool = new List<GameObject>();
        }
        for (int i = 0; i < Ship2PoolLimit; i++)
        {
            GameObject ShipObject = Instantiate(Ship2);
            ShipObject.transform.position = transform.position;
            ShipObject.SetActive(false);
            Ship2Pool.Add(ShipObject);
            
        }
    }

    public void ClearAll()
    {
        StopAllCoroutines();
        meteor_fire_rate = meteor_default_fire_rate;
        fire_rate1 = default_fire_rate1;
        fire_rate2 = default_fire_rate2;
        foreach (GameObject Ship in Ship1Pool)
        {
            Ship.SetActive(false);
        }
        foreach (GameObject Ship in Ship2Pool)
        {
            Ship.SetActive(false);
        }
        StartCoroutine(Start_spawn());
    }

    IEnumerator Start_spawn()
    {
        StartCoroutine(WaitForSpawnMeteor());
        StartCoroutine(WaitForSpawnShip1());
        StartCoroutine(WaitForSpawnShip2());
        yield break;
    }

    IEnumerator WaitForSpawnMeteor()
    {
        yield return new WaitForSeconds(meteor_start_time);
        StartCoroutine(SpawnMeteor());
        yield break;
    }

    IEnumerator WaitForSpawnShip1()
    {
        yield return new WaitForSeconds(start_time1);
        StartCoroutine(Spawn1Ship());
        yield break;
    }

    IEnumerator WaitForSpawnShip2()
    {
        yield return new WaitForSeconds(start_time2);
        StartCoroutine(Spawn2Ship());
        yield break;
    }

    IEnumerator SpawnMeteor()
    {
        while (true)
        {
            int i = 0;
            bool shoot = true;
            while (shoot & i < MeteorPoolLimit)
            {
                GameObject Meteor = MeteorPool[i];

                if (Meteor.activeSelf == false)
                {
                    Meteor.transform.position = new Vector2(Random.Range(Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).x, Camera.main.ViewportToWorldPoint(new Vector2(1, 0)).x), transform.position.y);
                    shoot = false;
                    Meteor.transform.rotation = transform.rotation;
                    switch (Random.Range(0, 10))
                    {
                        case 0:
                        case 1:
                        case 2:
                            Meteor.transform.localScale = new Vector2(2, 2);
                            break;
                        case 3:
                            Meteor.transform.localScale = new Vector2(3, 3);
                            break;
                        default:
                            Meteor.transform.localScale = new Vector2(1, 1);
                            break;
                    }
                    Meteor.SetActive(true);
                }
                else i++;
            }
            meteor_fire_rate = meteor_fire_rate * meteor_koef;
            yield return new WaitForSeconds((float)meteor_fire_rate);
        }
    }

    IEnumerator Spawn1Ship()
    {
        while (true)
        {
            int i = 0;
            bool shoot = true;
            while (shoot & i < Ship1PoolLimit)
            {
                GameObject Ship = Ship1Pool[i];
                foreach(int j in Lines_state)
                {
                    if (j == 1) { shoot = true; break; }
                    shoot = false;
                }
                if (shoot & Ship.activeSelf == false)
                {
                    int Line_spawn = Random.Range(0, Lines_state.Length);
                    while (Lines_state[Line_spawn]!=1) Line_spawn = Random.Range(0, Lines_state.Length);

                    Ship.GetComponent<EnemyShipDefault>().line = Line_spawn;                 
                    Ship.transform.position = Lines[Line_spawn].transform.position;
                    Ship.transform.rotation = Quaternion.Euler(0, 0, 180f);
                    shoot = false;
                    Ship.SetActive(true);
                }
                else i++;
            }
            fire_rate1 = fire_rate1 * koef1;
            yield return new WaitForSeconds((float)fire_rate1);
        }
    }

    IEnumerator Spawn2Ship()
    {
        while (true)
        {
            int i = 0;
            bool shoot = true;
            while (shoot & i < Ship2PoolLimit)
            {
                GameObject Ship = Ship2Pool[i];
                if (shoot & Ship.activeSelf == false)
                {
                    foreach (int j in Lines_state)
                    {
                        
                        if (j == 1) { shoot = true; break; }
                        shoot = false;
                    }
                    if (shoot & Ship.activeSelf == false)
                    {
                        int Line_spawn = Random.Range(0, Lines_state.Length);
                        while (Lines_state[Line_spawn] != 1) Line_spawn = Random.Range(0, Lines_state.Length);
                       
                        Ship.transform.position = Lines[Line_spawn].transform.position;
                        shoot = false;
                        Ship.SetActive(true);
                    }
                    else i++;
                }
                fire_rate2 = fire_rate2 * koef2;
                yield return new WaitForSeconds((float)fire_rate2);
            }
        }
    }

}
