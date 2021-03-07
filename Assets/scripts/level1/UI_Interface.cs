using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Interface: MonoBehaviour
{
    public Text Score;
    public Text Health;
    public GameObject Health_BG;
    public Text Shield;
    public GameObject shield_bar;

    [HideInInspector]
    public float rest_time;
    [HideInInspector]
    public float rest_begin;
    public Image Skillpercent;
    public GameObject Skill_BG;

    void Update()
    {
        Skill_BG.gameObject.SetActive(!GameManager.gameOver);
        Health_BG.gameObject.SetActive(!GameManager.gameOver);
        shield_bar.SetActive(GameManager.Shield_using & !GameManager.gameOver);
        Health.text = GameManager.health + "/" + GameManager.max_health;
        Score.gameObject.SetActive(!GameManager.gameOver);
        if (!FindObjectOfType<SkillPanelSkript>().SkillUsable)
        {
            if (ShipSpawner.time_count - rest_begin > rest_time)
            {
                Skillpercent.fillAmount = 0;
            }
            else
            {
                Skillpercent.fillAmount = ((float)(ShipSpawner.time_count - rest_begin) / (rest_time));
                Skillpercent.color = new Color32(255, 255, 0, 255);
            }
        }
        else
        {
            Skillpercent.fillAmount = 1;
            Skillpercent.color = new Color32(0, 255, 0, 255);
        }

        if (shield_bar.activeSelf)
        {
            Shield.text = string.Format("{0:0.0}/{1:0.0}", GameManager.shield, GameManager.max_shield);
        }
        Score.text = ""+ GameManager.score;
    }
}
