using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class ship_menu_swipe : MonoBehaviour, IPointerDownHandler, IPointerUpHandler// required interface when using the OnPointerDown method.
{
    private bool click_hold = false;
    private float end_pos;
    private float timer;
    private float moving_timer;
    private int change;
    private Vector2 end_vector;
    private Vector2 start_vector;

    void Update()
    {   
        if (click_hold)
        {
            transform.position = new Vector2(Input.mousePosition.x, transform.position.y);
            timer += Time.deltaTime;
        }
        if (end_vector != start_vector)
        {
            transform.position = Vector2.Lerp(start_vector, end_vector, EasingLeaner(moving_timer));
            moving_timer += Time.deltaTime;
        }
        if (end_vector == new Vector2(transform.position.x, transform.position.y))
        {
            if (end_vector.x == Camera.main.pixelWidth + 100)
            {
                start_vector = new Vector2(-100, transform.position.y);
                end_vector = new Vector2(Camera.main.pixelWidth / 2, transform.position.y);
                GameManager.chosen_ship = change;
                moving_timer = 0;
            }
            else if (end_vector.x == -100)
            {
                start_vector = new Vector2(Camera.main.pixelWidth + 100, transform.position.y);
                end_vector = new Vector2(Camera.main.pixelWidth / 2, transform.position.y);
                GameManager.chosen_ship = change;
                moving_timer = 0;
            }
            else
            {
                start_vector = Vector2.zero;
                end_vector = Vector2.zero;
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        click_hold = true;
        timer = 0;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        click_hold = false;
        end_pos = transform.position.x - Camera.main.pixelWidth / 2;
        if (end_pos > Camera.main.pixelWidth / 7) //Утягиваем вправо
        {
            if (GameManager.chosen_ship - 1 >= 0)
            {
                change = GameManager.chosen_ship - 1;
            }
            else
            {
                change = GameManager.number_ship - 1;
            }
            start_vector = transform.position;
            end_vector = new Vector2(Camera.main.pixelWidth + 100, transform.position.y);
            moving_timer = 0;
        }
        else if (end_pos <-Camera.main.pixelWidth / 7) //Утягиваем влево
        {  
            if (GameManager.chosen_ship + 1 < GameManager.number_ship)
            {
                change = 1 + GameManager.chosen_ship;
            }
            else
            {
                change = 0;
            }
            start_vector = transform.position;
            end_vector = new Vector2(- 100, transform.position.y);
            moving_timer = 0;
        }
        else
        {
            transform.position = new Vector2(Camera.main.pixelWidth / 2, transform.position.y);
        }
    }

    float EasingLeaner(float x)
    {
        return x * Mathf.Abs(end_pos)/timer*0.004f;
    }
}