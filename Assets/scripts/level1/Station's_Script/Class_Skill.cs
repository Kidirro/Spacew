using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Class_Skill : MonoBehaviour
{
    [Tooltip("Номер скилла")]
    public int Skill_id;

    [Tooltip("Описание скилла")]
    public string Desc;

    [Tooltip("Иконка")]
    public Image Icon;

    [Tooltip("Нужные скиллы для открытия")]
    public int[] Need_Skill;

    [Tooltip("Стостояние скилла \n 0 - заблокирован \n 1 - разблокирован \n 2+ - Изучен .")]
    public int state;
    
    [Tooltip("Состояние скилла по умолчанию")]
    public int def_state;

    [Tooltip("Максимальное число прокачек")]
    [Min(1)]
    public int max_grade;

    [Tooltip("Дерево")]
    public int tree;

    public void Start()
    {
        GameObject Locked = Instantiate(Resources.Load("Pref/LockedUI", typeof(GameObject)) as GameObject);
        Locked.transform.position = transform.position;
        
    }
}
