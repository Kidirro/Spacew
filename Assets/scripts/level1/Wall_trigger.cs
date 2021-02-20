using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_trigger : MonoBehaviour
{
    public bool tr;

    void Start()
    {
        GameManager.wall_trigger = false;
    }

    

    private void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 point = Camera.main.ScreenToWorldPoint(new Vector2(mousePos.x, mousePos.y));
         transform.position = point;
        tr = GameManager.wall_trigger;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    { 
        GameManager.wall_trigger = true;       
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
            GameManager.wall_trigger = false;

    }
}
