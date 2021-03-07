using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class ship_menu_swipe : MonoBehaviour// required interface when using the OnPointerDown method.
{
    private bool Ship_changing = false;
    private float moving_timer;
    private int change;
    private Vector2 end_vector;
    private Vector2 start_vector;

    void Update()
    {   
        /*if (click_hold)
        {
            transform.position = new Vector2(Input.mousePosition.x, transform.position.y);
            timer += Time.deltaTime;
        }*/
        if (end_vector != start_vector)
        {
            transform.position = Vector2.Lerp(start_vector, end_vector, EasingLeaner(moving_timer));
            moving_timer += Time.deltaTime;
        }
        if (Mathf.Abs(end_vector.x - transform.position.x)<10)
        {
            if (end_vector.x == Camera.main.pixelWidth*8 / 7)
            {
                start_vector = new Vector2(-Camera.main.pixelWidth / 7, transform.position.y);
                end_vector = new Vector2(Camera.main.pixelWidth / 2, transform.position.y);
                GameManager.chosen_ship = change;
                moving_timer = 0;
            }
            else if (end_vector.x == -Camera.main.pixelWidth/7)
            {
                start_vector = new Vector2(Camera.main.pixelWidth * 8 / 7, transform.position.y);
                end_vector = new Vector2(Camera.main.pixelWidth / 2, transform.position.y);
                GameManager.chosen_ship = change;
                moving_timer = 0;
            }
            else
            {
                start_vector = Vector2.zero;
                end_vector = Vector2.zero;
                Ship_changing = false;
            }
        }
    }

    public void Click(bool left)
    {
        if (!Ship_changing)
        {
            start_vector = transform.position;
            end_vector = (left) ? new Vector2(-Camera.main.pixelWidth / 7, transform.position.y) : new Vector2(Camera.main.pixelWidth * 8 / 7, transform.position.y);
            moving_timer = 0;
            if (!left) change = (GameManager.chosen_ship != 0) ? GameManager.chosen_ship - 1 : GameManager.number_ship - 1;
            else change = (GameManager.chosen_ship != GameManager.number_ship - 1) ? GameManager.chosen_ship + 1 : 0;
            Ship_changing = true;
        }
    }

    float EasingLeaner(float x)
    {
        return x*Mathf.Pow(moving_timer*10,2);
    }
}