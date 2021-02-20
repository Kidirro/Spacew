using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Store_UI_Script : MonoBehaviour
{
    public MovingMenu[] Sheets;
    private int Curent_Page = 0;
    public Text Money;
    public Text Health_info;
    public Text Speed_info;
    public Text Rate_info;

    [HideInInspector]
    static public bool can_mov = true;

    public int HealthPrice;
    public int SpeedPrice;
    public int RatePrice;

    public void Awake()
    {
        Debug.Log(GameManager.chosen_ship);
        for(int i=0; i < Sheets.Length; i++)
        {
            if (i < GameManager.chosen_ship)
            {
                Sheets[i].gameObject.transform.position = new Vector2(-0.5f * Camera.main.pixelWidth, Camera.main.pixelHeight / 2f);
                Debug.Log(i);
                Debug.Log(-1.5f * Camera.main.pixelWidth);
            }
            else if (i > GameManager.chosen_ship)
            {
                Sheets[i].gameObject.transform.position = new Vector2(1.5f * Camera.main.pixelWidth, Camera.main.pixelHeight / 2f);
                Debug.Log(i);
                Debug.Log(1.5f * Camera.main.pixelWidth);
            }
            else
            {
                Sheets[i].gameObject.transform.position = new Vector2(Camera.main.pixelWidth/2f, Camera.main.pixelHeight / 2f);
                Debug.Log(i);
                Debug.Log(Camera.main.pixelWidth/2f);
            }
        }
        Curent_Page = GameManager.chosen_ship;
    }
   
    //Добавлять сюда сброс прогресса ачивок
    public void ClearBuff()
    {
        PlayerPrefs.SetInt("Ship1_prog", 0);
        PlayerPrefs.SetInt("Ship2_prog", 0);
        PlayerPrefs.SetInt("New_Ship", -1);
        PlayerPrefs.SetInt("Records", 0);
        PlayerPrefs.SetInt("Unlocked_ships", (int)Mathf.Pow(10, Sheets.Length - 1));
        PlayerPrefs.Save();
    }
        
    public void ExitStore()
    {
        SceneManager.LoadScene(0);
    }

    public void MoveLeft()
    {
        if (can_mov && Curent_Page!=Sheets.Length-1)
        {
            can_mov = false;
            Sheets[Curent_Page].MoveLeft();
            Curent_Page++;
            Sheets[Curent_Page].MoveLeft();
            GameManager.chosen_ship = Curent_Page;
        }
    }

    public void MoveRight()
    {
        if (can_mov && Curent_Page != 0)
        {
            can_mov = false;
            Sheets[Curent_Page].MoveRight();
            Curent_Page--;
            Sheets[Curent_Page].MoveRight();
            GameManager.chosen_ship = Curent_Page;
        }
    }

}


