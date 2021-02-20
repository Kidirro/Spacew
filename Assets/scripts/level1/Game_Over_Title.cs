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

    [Header("Spawners")]
    public ShipSpawner ShipSpawnerManager;
    private Enemy_spawner EnemyShipSpawner;

    public AudioMixer Mixer;
    public AudioMixerSnapshot Normal;
    public AudioMixerSnapshot Death;
    public PostProcessVolume postProcessing;
    public Text New_Ship;

    public void Awake()
    {
        EnemyShipSpawner = GameObject.FindObjectOfType<Enemy_spawner>();
    }

    public void showMenu()
    {     
        Debug.Log("Menu showed");
        pause_menu.SetActive(true);
        if (GameManager.score > PlayerPrefs.GetInt("Records"))
        {
            GameManager.record = GameManager.score;
            PlayerPrefs.SetInt("Records",GameManager.record);
        }
        Record.text = "HiScore:\n" + PlayerPrefs.GetInt("Records");
        Score.text = "Score:\n" + GameManager.score;
        Time.timeScale = 0.2f;
        Debug.Log(pause_menu.activeSelf);
        Death.TransitionTo(0.5f);
        
        //Изменить количество метеоритов тут
        if (!GameManager.Unlocked_ship[1] && PlayerPrefs.GetInt("Ship1_prog")>=15)
        {
            PlayerPrefs.SetInt("New_Ship", 1);
            GameManager.Unlocked_ship[1] = true;
            PlayerPrefs.SetInt("Unlocked_ships", (int)Mathf.Pow(10, GameManager.Unlocked_ship.Length - 1 -1) + PlayerPrefs.GetInt("Unlocked_ships"));
            New_Ship.gameObject.SetActive(true);
        }

        //Изменить количество станций тут
        if (!GameManager.Unlocked_ship[2] && PlayerPrefs.GetInt("Ship2_prog") >= 1)
        {
            PlayerPrefs.SetInt("New_Ship", 2);
            GameManager.Unlocked_ship[2] = true;
            PlayerPrefs.SetInt("Unlocked_ships", (int)Mathf.Pow(10, GameManager.Unlocked_ship.Length - 2 - 1) + PlayerPrefs.GetInt("Unlocked_ships"));
            New_Ship.gameObject.SetActive(true);
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        GameManager.score = 0;
        ShipSpawnerManager.ClearAll();
        EnemyShipSpawner.ClearAll();
        New_Ship.gameObject.SetActive(false);
        pause_menu.SetActive(false);
        Normal.TransitionTo(0.5f);
        foreach (GameObject Bullet in GameObject.FindGameObjectsWithTag("EnemyBullet"))
        {
            Bullet.SetActive(false);
        }
    }

    public void toggleBloom()
    {
        if (postProcessing != null)
        {
            Bloom bloom;
            postProcessing.profile.TryGetSettings(out bloom);
            if (bloom != null) {
                bloom.active  = !bloom.active;
                Debug.Log(bloom.active);
                
            }
        }
    }

    public void toggleChromatic()
    {
        if (postProcessing != null)
        {
            ChromaticAberration chromatic;
            postProcessing.profile.TryGetSettings(out chromatic);
            if (chromatic != null)
            {
                chromatic.active = !chromatic.active;
                Debug.Log(chromatic.active);

            }
        }
    }
}
