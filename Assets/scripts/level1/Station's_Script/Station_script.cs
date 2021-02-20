using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Station_script : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public GameObject Station;
    public GameObject Enter;

    void Awake()
    {
        rb2d =GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Station.transform.position.y - Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).y < -10)
        {
            Station.transform.GetChild(1).gameObject.SetActive(true);
            Station.SetActive(false);
        }
    }

    private void OnEnable()
    {
        rb2d.velocity = Vector2.down*20;
    }

    public void ShipEnter()
    {
        Enter.SetActive(false);
        Debug.Log("Ship enter!");
        PlayerPrefs.SetInt("Ship2_prog", PlayerPrefs.GetInt("Ship2_prog")+1);
    }
}
