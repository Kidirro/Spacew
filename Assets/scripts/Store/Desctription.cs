using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Desctription : MonoBehaviour
{
    [HideInInspector]
    public GameObject New_notif;

    public Text Title;
    private string[] names = { "1", "2", "3" };

    public Text Decs;
    private string[] decs_str = { "Описание первого корабля", "Описание второго корабля", "Описание 3 корабля" };
    private string[] unlocking = { "Описание разблокировки первого корабля", "Описание разблокировки второго корабля", "Описание разблокировки 3 корабля" };

    private void OnEnable()
    {
        Title.text = names[GameManager.chosen_ship];

        if (GameManager.Unlocked_ship[GameManager.chosen_ship])
        {
            Decs.text = decs_str[GameManager.chosen_ship];
        }
        else
        {
            Decs.text = unlocking[GameManager.chosen_ship];
        }

        New_notif.SetActive(GameManager.chosen_ship == PlayerPrefs.GetInt("New_Ship"));
        if (GameManager.chosen_ship == PlayerPrefs.GetInt("New_Ship")) PlayerPrefs.SetInt("New_Ship", -1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) this.gameObject.SetActive(false);
    }
}
