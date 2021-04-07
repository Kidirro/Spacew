using UnityEngine;
using System.Collections.Generic;

static public class GameManager
{
    static public bool first_play;

    static public int record = 0;
    static public int score = 0;
    static public bool gameOver = false;

    static public float health;
    static public int defaulthealth=5;
    static public float max_health;

    static public int number_ship = 4;

    static public List<EnemyDefault> EnemyOnScreen;

    static public double fire_rate;
    static public double defaultfire_rate=0.5;
    static public double buff_rate = 0.9;

    static public int chosen_ship = 0;

    static public bool Shield_using;
    static public double max_shield=10;
    static public float shield_rest_time = 3;
    static public double shield;
    static public double defaultshield=10;

    static public double last_hit;
    static public bool following;
    static public bool wall_trigger;
    static public bool[] Unlocked_ship;

    static public bool Can_Respawn=true;

    static public Class_Skill[] skills;

    static public bool Using_sound;

    static public void take_damage(int damage, int price,bool kill, GameObject other,int multiplier = 1)
    {
        if (Shield_using & shield > 0)
        {
            shield -= damage*multiplier;
            if (shield < 0)
            {
                shield = 0;
                ShieldBreak();
            }
            last_hit = ShipSpawner.time_count;
        }

        else
        {
            health -= damage*multiplier;
        }

        score += price*multiplier;
        if (kill)
        {
            other.SetActive(false);
        }
    }

    static public void ShieldBreak()
    {
        if (skills[5].state > 1)
        {
            foreach (EnemyDefault enemy in EnemyOnScreen) enemy.Hp = enemy.Hp - (skills[5].state - 1);
            Debug.LogFormat("Mass damage: {0}", skills[5].state - 1);
        }
        if ((skills[7].state > 1) & (Random.Range(0f, 1f) < 0.1 * (skills[7].state - 1))) 
        { 
            shield = max_shield;
            Debug.LogFormat("Shild restored! Chance: {0}%", (10 * (skills[7].state - 1)));

        }

    }
}
