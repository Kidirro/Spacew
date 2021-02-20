using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Interface: MonoBehaviour
{
    public Text Score;
    public Image percent;
    public Image Shieldpercent;
    public GameObject shield_bar;

    [HideInInspector]
    public float rest_time;
    [HideInInspector]
    public float rest_begin;
    public Image Skillpercent;

    void Update()
    {
        shield_bar.SetActive(GameManager.Shield_using);
        percent.fillAmount = ((float)GameManager.health / ((float)GameManager.defaulthealth));
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
            Shieldpercent.fillAmount = ((float)GameManager.shield / ((float)GameManager.max_shield));
        }
        Score.text = "Score\n" + GameManager.score;
    }
}
