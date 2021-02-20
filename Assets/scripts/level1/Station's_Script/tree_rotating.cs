using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class tree_rotating : MonoBehaviour
{

    private int current_segment=0;
    private int Segments;
    private int Segment_angle;
    public bool Using_rotate;
    [HideInInspector]
    public GameObject[] Skill_segments;
    private int[] ids;
    public Text Skill_desk;
    [HideInInspector]
    public GameObject accept_button;
    [HideInInspector]
    public GameObject menu;
    private int Chosen = -1;

    void OnEnable()
    {
        Time.timeScale = 0f;
        accept_button.SetActive(false);
        foreach (Class_Skill skill in GameManager.skills)
        {
            if (skill.state == skill.max_grade +1)
            {
                skill.GetComponent<Image>().color= new Color32(255, 255, 255, 255);
                //Тут менять цвет. (Куплено)
            }
            else
            {
                bool flag = true;
                foreach(int sk in skill.Need_Skill)
                {
                    flag = flag & (GameManager.skills[sk].state > 1);
                }
                if (!flag)
                {
                    skill.state = 0;
                    skill.GetComponent<Image>().color = new Color32(255, 255, 50, 100);
                    //Менять цвет. (Заблокировано)
                }
                else
                {
                    if (skill.state == 0)
                    {
                        skill.state = 1;
                    }
                    skill.GetComponent<Image>().color = new Color32(255, 255, 50, 255);
                    //Менять цвет. (разрешено для покупки)
                }
            }
        }
    }

    private void Awake()
    {
        if (GameManager.skills == null)
        {
            GameManager.skills = FindObjectsOfType<Class_Skill>();
            for (int i = 0; i < GameManager.skills.Length; i++)
            {
                while (GameManager.skills[i].Skill_id != i)
                {
                    Class_Skill var = GameManager.skills[GameManager.skills[i].Skill_id];
                    GameManager.skills[GameManager.skills[i].Skill_id] = GameManager.skills[i];
                    GameManager.skills[i] = var;
                }
                GameManager.skills[i].state = GameManager.skills[i].def_state;

            }
        }
        menu.SetActive(false);
        Segments = Skill_segments.Length;
        ids = new int[Segments];
        Segment_angle = 360 / Segments;
        for (int i = 0; i < Segments; i++)
        {
            ids[i] = i;
            Skill_segments[i].transform.rotation = Quaternion.Euler(0, 0, (current_segment - i) * Segment_angle);
        }
    }

    public void Chose_skill(int id)
    {
        Chosen = id;
        Skill_desk.text = GameManager.skills[id].Desc;
    }

    public void Exit()
    {
        menu.SetActive(false);
        GameManager.gameOver = false;
        Time.timeScale = 1f;
    }

    public void Accept()
    {
        if (GameManager.skills[Chosen].state >= 1 & GameManager.skills[Chosen].state - 1 < GameManager.skills[Chosen].max_grade )
        {
            GameManager.skills[Chosen].state += 1;
            Chosen = -1;
            menu.SetActive(false);
            GameManager.gameOver = false;
            Time.timeScale = 1f;
            Skill_desk.text = "Выберите навык";
        }
        else if (GameManager.skills[Chosen].state == GameManager.skills[Chosen].max_grade+1)
        {
            Skill_desk.text = "Навык уже изучен максимально. Выберите другой";
        }
        else
        {
            Skill_desk.text = "Вначале изучите все предыдущие навыки. Выберите другой";
        }
    }

    private void Update()
    {
        if (Chosen != -1)
        {
            accept_button.SetActive(true);
        }
        else
        {
            accept_button.SetActive(false);
        }
    }

    private int FindIndex(int[] array,int m)
    {
        int i=0;
        for(i=0; i< array.Length; i++)
        {
            if (m == array[i]) break;
        }
        return i;
    }

    public void Tree_click(int id)
    {
        current_segment = FindIndex(ids,id);
        Rotate_Segments();
    }

    public void Change_Current_Segment(int id)
    {
        current_segment = id;
        Rotate_Segments();

    }

    public void Split_skill(string ids_str)
    {   
        int point = ids_str.IndexOf(';');
        if (point != -1)
        {
            int id1 = int.Parse(ids_str.Substring(0, point));
            int id2 = int.Parse(ids_str.Substring(point, ids_str.Length-1).Trim(';'));
            if (id1 == ids[current_segment])
            {
                int right = (int)Mathf.Sign(id2-id1);
                int change_id= FindIndex(ids,id2);
                if (change_id != -1)
                {
                    int swap_int= ids[current_segment+right];
                    ids[current_segment + right] = ids[change_id];
                    ids[change_id] = swap_int;
                    var swap_go = Skill_segments[current_segment + right];
                    Skill_segments[current_segment + right] = Skill_segments[change_id];
                    Skill_segments[change_id] = swap_go;
                    Rotate_Segments();
                }

            }
            else
            {
                Change_Current_Segment(FindIndex(ids, id1));
            }
        }
    }

    public void Rotate_Segments()
    {
        for (int i = 0; i<Segments; i++)
        {
            Skill_segments[i].transform.rotation = Quaternion.Euler(0, 0, (current_segment - i) * Segment_angle);
        }
    }
}
