using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Desctription : MonoBehaviour
{
    public Text Title;
    private string[] names = { "AM \"Scout\"", "AM \"Crusader\"", "AM \"Alchemist\"", "USG \"Ishimura\"" };

    public Text Decs;
    private string[] decs_str = { "Самый первый корабль исследователей \n Оснащен базовыми орудиями дальнего боя.", "Мощный корабль Альянса \n Оснащен мощным щитом, позволяющим выдерживать большое количество урона. \n Вес корабля не позволяет ему поставить оружие сначала.", "Мощный оружейный корабль Альянса \n Оснащен мощной залповой установкой, которая позволяет наносить много урона по площади.","Мощный корабль, позволяющий создавать дронов прямо в полете."};
    private string[] unlocking = { "Описание разблокировки первого корабля", "Проведите все исследование над щитами", "Приземлитесь в дружественную станцию","Создайте 30 дронов" };

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

        if (GameManager.chosen_ship == PlayerPrefs.GetInt("New_Ship")) PlayerPrefs.SetInt("New_Ship", -1);
    }

    public void Exit()
    {
        this.gameObject.SetActive(false);
    }
}
