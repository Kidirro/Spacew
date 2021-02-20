using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class tree_moving : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector2 Start_vector_menu;
    private Vector2 Start_vector_mouse;
    private Vector2 Change_vector_mouse;
    [Header("Ссылка на маску, которая будет скрывать ненужное")]
    public Mask mask;
    [Header("На сколько камера будет отходить от краев (в экранных пикселях, не в мировых)")]
    [Range(-100, 100)]
    public float range;
    private bool click_hold = false;


    private float h;
    private float w;
    private float H;
    private float W;

    private void Awake()
    {
        h = mask.rectTransform.rect.height / 2;
        H = this.gameObject.GetComponent<RectTransform>().rect.height / 2;
        w = mask.rectTransform.rect.width / 2;
        W = this.gameObject.GetComponent<RectTransform>().rect.width / 2;
    }

    private void Update()
    {
        if (click_hold)
        { 
                transform.position = new Vector2(Mathf.Clamp(Start_vector_menu.x - (Start_vector_mouse.x - Input.mousePosition.x), mask.transform.position.x - Mathf.Abs(W-w + range), mask.transform.position.x + Mathf.Abs(W - w + range)), 
                    Mathf.Clamp(Start_vector_menu.y - (Start_vector_mouse.y - Input.mousePosition.y), mask.transform.position.y - Mathf.Abs(H - h + range), mask.transform.position.y + Mathf.Abs(H - h + range)));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Start_vector_menu = transform.position;
        Start_vector_mouse = Input.mousePosition;
        click_hold = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        click_hold = false;
    }
}
