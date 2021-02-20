using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main_menu_UI : MonoBehaviour
{
    public Sprite[] Ships;
    public Text hiscore;
    public Image ship;
    [Header("Ссылка на уведомление")]
    public Image New_Ship;
    public ShipSpawner shipSpawner;
    public GameObject Desctr;

    public void Awake()
    {
        New_Ship.gameObject.SetActive(false);
        if (PlayerPrefs.GetInt("Unlocked_ships") == 0 || (PlayerPrefs.GetInt("Unlocked_ships") < (int)Mathf.Pow(10, Ships.Length - 1)))
        {
            PlayerPrefs.SetInt("Unlocked_ships", (int)Mathf.Pow(10, Ships.Length - 1) + PlayerPrefs.GetInt("Unlocked_ships"));
        }
        Debug.Log(PlayerPrefs.GetInt("Unlocked_ships"));
        GameManager.Unlocked_ship = new bool[Ships.Length];
        GameManager.number_ship = Ships.Length;
        PlayerPrefs.Save();
    }

    public void Startgame()
    {
        if (GameManager.Unlocked_ship[GameManager.chosen_ship])
        {
            shipSpawner.start_game();
            GameManager.gameOver = false;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void Check_unlock()
    {
        int n = PlayerPrefs.GetInt("Unlocked_ships");
        for (int i = Ships.Length - 1; i >= 0; i--)
        {
            GameManager.Unlocked_ship[i] = (n % 10) == 1;
            n = n / 10;
        }
    }

    private void Update()
    { 
        if (PlayerPrefs.GetInt("New_Ship") != -1)
        {
            New_Ship.gameObject.SetActive(true);
        }
        Check_unlock();
        hiscore.text = "Record:\n" + PlayerPrefs.GetInt("Records");
        GameManager.record = PlayerPrefs.GetInt("Records");
        ship.sprite = Ships[GameManager.chosen_ship];
        if (GameManager.Unlocked_ship[GameManager.chosen_ship])
        {
            ship.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        else
        {
            ship.GetComponent<Image>().color = new Color32(0, 0, 0, 100);
        }
    }

    public void LoadStore() 
    {
        Desctr.SetActive(true);
    }

    public void SwitchShip()
    {
        if (GameManager.chosen_ship == GameManager.number_ship-1)
        {
            GameManager.chosen_ship = 0;
        }
        else GameManager.chosen_ship++;
    }

}


