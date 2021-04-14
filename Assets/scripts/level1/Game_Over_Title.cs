using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;

public class Game_Over_Title : MonoBehaviour
{
    public Text Record;
    public Text Score;
    public GameObject pause_menu;

    private Ad ads;

    [Header("Spawners")]
    public ShipSpawner ShipSpawnerManager;
    private Enemy_spawner EnemyShipSpawner;
    private Station_spawner Station;

    public GameObject Respawn;

    public AudioMixer Mixer;
    public AudioMixerSnapshot Normal;
    public AudioMixerSnapshot Death;
    public Text New_Ship;

    public void Awake()
    {
        Station = GameObject.FindObjectOfType<Station_spawner>();
        EnemyShipSpawner = GameObject.FindObjectOfType<Enemy_spawner>();
        ads = GameObject.FindObjectOfType<Ad>();
    }

    public void Respawn_Ad()
    {

        ShipSpawnerManager.ClearAll();
        EnemyShipSpawner.ClearEnemy();
        Respawn.SetActive(false);
        ads.ShowRewardedVideo();
        GameManager.Can_Respawn = false;
    }

    public void Ship_break()
    {
        Time.timeScale = 0f;
        if (GameManager.Can_Respawn & Ad.ad_ready) Respawn.SetActive(true);
        else showMenu();
    }

    public void showMenu()
    {
        New_Ship.gameObject.SetActive(false);
        Respawn.SetActive(false);
        pause_menu.SetActive(true);
        if (GameManager.score > PlayerPrefs.GetInt("Records"))
        {
            PlayerPrefs.SetInt("Records",GameManager.score);
        }
        Record.text = "" + PlayerPrefs.GetInt("Records");
        Score.text = "" + GameManager.score;
        Time.timeScale = 0.2f;
        Death.TransitionTo(0.5f);
        GameManager.gameOver = true;
        
        if (!GameManager.Unlocked_ship[1] && PlayerPrefs.GetInt("Ship1_prog")>=1)
        {
            PlayerPrefs.SetInt("New_Ship", 1);
            GameManager.Unlocked_ship[1] = true;
            New_Ship.gameObject.SetActive(true);
            Debug.LogError(GameManager.Unlocked_ship[1]);
        }

        if (!GameManager.Unlocked_ship[2] && PlayerPrefs.GetInt("Ship2_prog") >= 1)
        {
            PlayerPrefs.SetInt("New_Ship", 2);
            GameManager.Unlocked_ship[2] = true;
            New_Ship.gameObject.SetActive(true);
        }
        
        if (!GameManager.Unlocked_ship[3] && PlayerPrefs.GetInt("Ship3_prog") >= 30)
        {
            PlayerPrefs.SetInt("New_Ship", 3);
            GameManager.Unlocked_ship[3] = true;
            New_Ship.gameObject.SetActive(true);
        }
    }

    public void Restart()
    {
        GameManager.Can_Respawn = true;
        Time.timeScale = 1f;
        GameManager.score = 0;
        ShipSpawnerManager.ClearAll();
        ShipSpawnerManager.ClearSkill();
        EnemyShipSpawner.ClearAll();
        Station.ClearAll();
        New_Ship.gameObject.SetActive(false);
        Normal.TransitionTo(0.5f);
        GameManager.gameOver = false;
        foreach (GameObject Bullet in GameObject.FindGameObjectsWithTag("EnemyBullet"))
        {
            Bullet.SetActive(false);
        }
        pause_menu.SetActive(false);
    }

    private void OnEnable()
    {        transform.position = new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
    }
}
