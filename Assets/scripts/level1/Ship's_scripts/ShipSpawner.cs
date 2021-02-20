﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ShipSpawner : MonoBehaviour
{

    public GameObject[] ships;

    public Game_Over_Title title;
    public ParticleSystem StarSystem;

    [HideInInspector]
    public AudioMixer Mixer;
    [HideInInspector]
    public GameObject LevelWall;
    [HideInInspector]
    private GameObject _player;
    public GameObject Player
    {
        get { return _player; }
    }

    private double last_touch;


    [Header("Аудиомиксеры")]
    public AudioMixerSnapshot Death;
    public AudioMixerSnapshot Normal;
    public AudioSource bg_music;
    public AudioSource death_sound;

    [Header("Объекты главного меню и игры")]
    public GameObject Playing_objs;
    public GameObject Main_menu_objs;
    


    static public double time_count = 0;

    public void show_main_menu()
    {
        if (_player)
        {
            Destroy(_player);
        } 
        Death.TransitionTo(0.5f);
        Playing_objs.SetActive(false);
        Main_menu_objs.SetActive(true);
        Time.timeScale = 0.2f;
    }

    public void start_game()
    {
        Normal.TransitionTo(0.5f);
        Playing_objs.SetActive(true);
        Main_menu_objs.SetActive(false);
        if (_player == null)
        {
            _player = Instantiate(ships[GameManager.chosen_ship]) as GameObject;
            _player.transform.position = transform.position;
            _player.SetActive(false);
        }
        GameManager.max_health = GameManager.defaulthealth;
        title.Restart();
        StarSystem.transform.localScale = new Vector3(Camera.main.ViewportToWorldPoint(new Vector2(1, 1)).x * 2, 1, 1);
    }

    private void Awake()
    {

        for (int i = 0; i <= 3; i++)
        {
            GameObject Wall = Instantiate(LevelWall) as GameObject;
            switch (i)
            {
                case 0:
                    Wall.transform.position = Camera.main.ViewportToWorldPoint(new Vector2((float)0.5, 0));
                    Wall.transform.position = new Vector2(Wall.transform.position.x, Wall.transform.position.y - 1);
                    Wall.transform.localScale = new Vector2(Camera.main.ViewportToWorldPoint(new Vector2((float)0, 0)).x * 2, 1);
                    break;
                case 1:
                    Wall.transform.position = Camera.main.ViewportToWorldPoint(new Vector2((float)0.5, 1));
                    Wall.transform.position = new Vector2(Wall.transform.position.x, Wall.transform.position.y + 1);
                    Wall.transform.localScale = new Vector2(Camera.main.ViewportToWorldPoint(new Vector2((float)0, 0)).x * 2, 1);
                    break;
                case 2:
                    Wall.transform.position = Camera.main.ViewportToWorldPoint(new Vector2(0, (float)0.5));
                    Wall.transform.position = new Vector2(Wall.transform.position.x - 1, Wall.transform.position.y);
                    Wall.transform.localScale = new Vector2(1, Camera.main.ViewportToWorldPoint(new Vector2((float)0, 0)).y * 2);
                    break;
                case 3:
                    Wall.transform.position = Camera.main.ViewportToWorldPoint(new Vector2(1, (float)0.5));
                    Wall.transform.position = new Vector2(Wall.transform.position.x + 1, Wall.transform.position.y);
                    Wall.transform.localScale = new Vector2(1, Camera.main.ViewportToWorldPoint(new Vector2((float)0, 0)).y * 2);
                    break;

            }
            show_main_menu();
        }
    }

    void Start()
    {
        StartCoroutine(TimePerSec());

    }

    private void Update()
    {
        if (_player)
        {
            if (_player.activeSelf == false && title.pause_menu.activeSelf == false)
            {
                death_sound.Play();
                _player.SetActive(false);
                title.showMenu();
                bg_music.pitch = 0.8f;
                bg_music.volume = 0.05f;

            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (time_count - last_touch <= 0.5f)
                { 
                    FindObjectOfType<SkillPanelSkript>().Skill();

                }
                last_touch = time_count;
            }


            GameManager.Shield_using = GameManager.skills[0].state >= 2;
            fix_shield();

        }
    }

    private void fix_shield()
    {
        if (GameManager.last_hit + GameManager.shield_rest_time < ShipSpawner.time_count)
        {
            if (GameManager.shield <= GameManager.max_shield)
            {
                GameManager.shield += GameManager.max_shield * 0.01;
            }
        }
    }

    public IEnumerator TimePerSec()
    {
        while (true)
        {
            time_count = time_count + 0.1f * Time.timeScale;
            yield return new WaitForSeconds(0.1f);
        }

    }

    public void ClearAll()
    {
        GameManager.fire_rate = GameManager.defaultfire_rate;
        bg_music.pitch = 1f;
        bg_music.volume = 0.1f;
        _player.transform.position = transform.position;
        time_count = 0;
        GameManager.health = GameManager.max_health;
        GameManager.shield = GameManager.max_shield;
        GameManager.last_hit = 0;
        last_touch = -1;

        for (int i = 0; i < GameManager.skills.Length; i++)
        {
            GameManager.skills[i].state = GameManager.skills[i].def_state;
        }
        _player.SetActive(true);
    }
}