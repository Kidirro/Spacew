using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class tree_rotating : MonoBehaviour
{

    private List<GameObject> Lines;
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
                skill.transform.GetChild(0).gameObject.SetActive(false);
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
                    skill.GetComponent<Image>().color = Color.red;
                    skill.transform.GetChild(0).gameObject.SetActive(true);
                    //Менять цвет. (Заблокировано)
                }
                else
                {
                    if (skill.state == 0)
                    {
                        skill.state = 1;
                    }
                    skill.GetComponent<Image>().color = Color.yellow;
                    skill.transform.GetChild(0).gameObject.SetActive(false);
                    //Менять цвет. (разрешено для покупки)
                }
            }
        }
    }

    private void Awake()
    {
        Lines = new List<GameObject>();
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
        Tree_click(GameManager.skills[id].tree);
        Chosen = id;
        Skill_desk.text = GameManager.skills[id].Desc + "\n Изучено:" +Mathf.Clamp(GameManager.skills[id].state-1,0,100) + " из " + GameManager.skills[id].max_grade;
        if (Lines.Count>0)
        {
            DeleteLine();
        }

        if (GameManager.skills[id].Need_Skill.Length > 0)
        {
            for (int i = 0; i < GameManager.skills[id].Need_Skill.Length; i++)
            {
                GameObject line_obj =new GameObject("Line");
                LineRenderer line = line_obj.AddComponent<LineRenderer>();
                line.positionCount = 2;
                line.SetPosition(0, Camera.main.ScreenToWorldPoint(GameManager.skills[id].gameObject.transform.position));
                line.SetPosition(1, Camera.main.ScreenToWorldPoint(GameManager.skills[GameManager.skills[id].Need_Skill[i]].gameObject.transform.position));
                line.SetPosition(0, new Vector3(line.GetPosition(0).x, line.GetPosition(0).y, 1));
                line.SetPosition(1, new Vector3(line.GetPosition(1).x, line.GetPosition(1).y, 1));
                line.sortingOrder = 1;

                line.material = new Material(Shader.Find("Sprites/Default"));
                line.material.color = (GameManager.skills[GameManager.skills[id].Need_Skill[i]].state > 1) ? Color.green: Color.red;
                Lines.Add(line_obj);
            }
        }
    }

    public void Exit()
    {
        menu.SetActive(false);
        GameManager.gameOver = false;
        Time.timeScale = 1f;
        DeleteLine();  
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
            DeleteLine();
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
        DeleteLine();
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
            int id2 = int.Parse(ids_str.Substring(point, ids_str.Length - 1).Trim(';'));
            if (id1 != ids[current_segment])
            {
                Change_Current_Segment(FindIndex(ids, id1));
            }
            int right = (int)Mathf.Sign(id2 - id1);
            id2 = Mathf.Abs(id2);
            int change_id = FindIndex(ids, id2);
            if (change_id != -1)
            {
                int swap_int_main = (current_segment + right + ids.Length) % ids.Length;
                int swap_int = ids[swap_int_main];


                ids[swap_int_main] = ids[change_id];
                ids[change_id] = swap_int;
                var swap_go = Skill_segments[swap_int_main];
                Skill_segments[swap_int_main] = Skill_segments[change_id];
                Skill_segments[change_id] = swap_go;
                Rotate_Segments();
            }


        }
    }

    private void DeleteLine()
    {
        for (int i = 0; i < Lines.Count; i++)
        {
            Destroy(Lines[i].gameObject);
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
