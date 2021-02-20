﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillPanelSkript : MonoBehaviour
{

    public ShipSpawner shsp;
    private GameObject player;
    private List<GameObject> objects;

    [HideInInspector]
    public bool SkillUsable;

    [Range(0, 20)]
    [Header("Drone skill")]
    public float Drone_time;
    public GameObject drone_pref;
    public GameObject Drone_skill_prfab;
    [Range(0, 20)]
    public float Drone_cooldown;

    [Header("Invisibility Skill")]
    public float Invs_time;
    [Range(0, 20)]
    public float Invs_cooldown;

    [Header("Shotgun Skill")]
    [Range(0, 20)]
    public float shtg_cooldown;
    public Ship3 ship3_obj;

    private UI_Interface bars;
    private float[] cds;

    [Header("Drones")]
    public GameObject Fire_Drone;
    public GameObject Shield_Drone;
    private List<GameObject> Fire_Drones;
    private int active_Fire_drones=0;
    private List<GameObject> Shield_Drones;
    private int active_Shield_drones=0;

    private void Update()
    {
        if (player == null) player = shsp.Player;
        if (player.activeSelf)
        {
            foreach (GameObject obj in objects)
            {
                obj.transform.position = player.transform.position;
            }
            if (GameManager.chosen_ship == 2 && ship3_obj == null)
            {
                ship3_obj = FindObjectOfType<Ship3>();
            }
            if (GameManager.skills[3].state - 1 > active_Fire_drones)
            {
                Fire_Drones[active_Fire_drones].SetActive(true);
                Fire_Drones[active_Fire_drones].transform.position = player.transform.position;
                active_Fire_drones++;
            }
            if (GameManager.skills[5].state - 1 > active_Shield_drones)
            {
                Shield_Drones[active_Shield_drones].SetActive(true);
                Shield_Drones[active_Shield_drones].transform.position = player.transform.position;
                active_Shield_drones++;
            }
        }
        else
        {
            StopAllCoroutines();
            active_Fire_drones = 0;
            active_Shield_drones = 0;
            foreach (GameObject drone in Fire_Drones)
            {
                drone.SetActive(false);
            }
            foreach (GameObject drone in Shield_Drones)
            {
                drone.SetActive(false);
            }
        }
    }

    private void Awake()
    {
        Fire_Drones = new List<GameObject>();
        for (int i = 0; i < GameManager.skills[3].max_grade; i++)
        {
            GameObject FireObject = Instantiate(Fire_Drone);
            FireObject.SetActive(false);
            Fire_Drones.Add(FireObject);
        }
        Shield_Drones = new List<GameObject>();
        for (int i = 0; i < GameManager.skills[5].max_grade; i++)
        {
            GameObject FireObject = Instantiate(Shield_Drone);
            FireObject.SetActive(false);
            Shield_Drones.Add(FireObject);
        }
        cds = new float[3];
        cds[0] = Invs_cooldown;
        cds[1] = Drone_cooldown;
        cds[2] = shtg_cooldown;
        if (objects == null)
        {
            objects = new List<GameObject>();
        }
        if (bars == null) bars = FindObjectOfType<UI_Interface>();
    }
    
    
    private void OnEnable()
    {
        if (GameManager.chosen_ship==2 && ship3_obj == null)
        {
            ship3_obj = FindObjectOfType<Ship3>();
        }
        SkillUsable = true;
        player = shsp.Player;
        bars.rest_time = cds[GameManager.chosen_ship];
    }


    //Изменить при ветке скиллов
    public void Skill()
    {
        if (SkillUsable)
        {
            switch (GameManager.chosen_ship) {
                case 0:
                    StartCoroutine(InvisibilitySkill_process());
                    break;
                case 1:
                    StartCoroutine(DroneSkill_process());
                    break;
                case 2:
                    StartCoroutine(Shotgun());
                    break;
            }

        }
    }


    IEnumerator DroneSkill_process()
    {
        Debug.Log("Drine");
        GameObject drone_fire= Instantiate(drone_pref) as  GameObject;
        SkillUsable = false;
        /*GameObject drone = Instantiate(Drone_skill_prfab);
        objects.Add(drone);
        float time = 0f;
        while (time <= Drone_time)
        {

            if (GameManager.max_health > GameManager.health)
            {
                GameManager.health += 0.05f;
            }
            else if (GameManager.max_health < GameManager.health)
            {
                GameManager.health = GameManager.max_health;
            }

            if (GameManager.max_health == GameManager.health && GameManager.Shield_using)
            {
                if (GameManager.max_shield > GameManager.shield)
                {
                    GameManager.shield += 0.05f;
                }
                else if (GameManager.max_shield < GameManager.shield)
                {
                    GameManager.shield = GameManager.max_shield;
                }

            }
            time += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
        
        
        objects.Remove(drone);
        Destroy(drone.gameObject);*/
        StartCoroutine(Cooldown());
        //Destroy(drone_fire);
        yield break;

    }

    IEnumerator InvisibilitySkill_process()
    {
        SkillUsable = false;
        player.layer = 14;
        SpriteRenderer spr = player.GetComponent<SpriteRenderer>();
        Color cl = spr.material.color;
        cl.a = 0.3f;
        spr.color = cl;
        yield return new WaitForSeconds(Invs_time);
        player.layer = 12;
        cl.a = 1f;
        spr.color = cl;
        StartCoroutine(Cooldown());
        yield break;

    }

    IEnumerator Shotgun()
    {
        SkillUsable = false;
        ship3_obj.skill_mass_shot();
        Debug.Log(cds[GameManager.chosen_ship]);
        StartCoroutine(Cooldown());
        yield break;
    }

    IEnumerator Cooldown()
    {
        bars.rest_begin = (float)ShipSpawner.time_count;
        yield return new WaitForSeconds(cds[GameManager.chosen_ship]);
        SkillUsable = true;
        yield break;
    }


}