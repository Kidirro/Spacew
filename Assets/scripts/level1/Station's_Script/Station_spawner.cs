using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station_spawner : MonoBehaviour
{
    public GameObject Station_pref;
    public GameObject Station;
    private GameObject station_enter;
    public double default_fire_rate = 30;
    public double last_fire = 30;
    public bool can_enter;

    public GameObject title;
    // Update is called once per frame

    private void Awake()
    {
        Station = Instantiate(Station_pref) as GameObject;
        Station.SetActive(false);
        station_enter = Station.transform.GetChild(1).gameObject;
    }

    void Update()
    {
        if (last_fire + default_fire_rate <= ShipSpawner.time_count || Input.GetKeyDown(KeyCode.S))
        {
            if (Station.activeSelf == false)
            {
                Station.transform.position = new Vector2(Random.Range(Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).x, Camera.main.ViewportToWorldPoint(new Vector2(1, 0)).x), transform.position.y);

                Station.transform.rotation = transform.rotation;
                Station.SetActive(true);
                last_fire = ShipSpawner.time_count;
            }
        }

        if (Station.activeSelf == false)
        {
            can_enter = true;
        }

        if (station_enter.activeSelf == false && title.activeSelf == false && can_enter == true && Station.activeSelf==true)
        {
            GameManager.gameOver = true;
            title.SetActive(true);
            can_enter = false;
        }
    }

    public void ClearAll()
    {
        default_fire_rate = 30;
        last_fire = 30;
    }
}
