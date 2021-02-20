using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingMenu : MonoBehaviour
{
    private Vector2 target_position;
    private Vector2 start_position;
    public Text Achivment_text;
    public Image Ship;

    public Image New_Ship;
    public int id;
    float time = 0;
    void Start()
    {
        New_Ship.gameObject.SetActive(id == PlayerPrefs.GetInt("New_Ship")); 
        time = 0;
        start_position = transform.position;
        target_position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position == new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2)) && (transform.position== new Vector3(target_position.x,target_position.y)) && !Store_UI_Script.can_mov)
        {
            Store_UI_Script.can_mov = true;
        }
        transform.position = Vector2.Lerp(start_position, target_position, EasingLeaner(time ));
        time += Time.deltaTime;

        if ((transform.position == new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2))&&(id == PlayerPrefs.GetInt("New_Ship")))
        {
            PlayerPrefs.SetInt("New_Ship", -1);
        }

        if (!GameManager.Unlocked_ship[id])
        {
            Ship.GetComponent<Image>().color = new Color32(100, 100, 100, 100);
        }
        else
        {
            Ship.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        if (id != 0)
        {
            Achivment_text.gameObject.SetActive(!GameManager.Unlocked_ship[id]);
        }
    }

    public void MoveRight()
    {
        if (target_position == new Vector2(transform.position.x, transform.position.y))
        {
            Store_UI_Script.can_mov = false;
            target_position = new Vector2(transform.position.x +Camera.main.pixelWidth , transform.position.y);
            start_position = transform.position;
            time = 0;
        }
    }

    public void MoveLeft()
    {
        if (target_position == new Vector2(transform.position.x,transform.position.y)) 
        {
            Store_UI_Script.can_mov = false;
            target_position = new Vector2(transform.position.x - Camera.main.pixelWidth, transform.position.y);
            start_position = transform.position;
            time = 0;
        }
    }

    float EasingLeaner(float x)
    {
        return Mathf.Pow(x, 2);
    }
}
