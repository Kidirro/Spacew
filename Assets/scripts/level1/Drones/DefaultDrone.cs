using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultDrone : MonoBehaviour
{
    [Header("Большой радиус")]
    [Range(0, 20)]
    public int R;
    [Header("Малый радиус")]
    [Range(0, 20)]
    public int r;
    private Vector2 vect_follow;

    private GameObject player;

    public virtual void Start()
    {
        vect_follow = new Vector2((int)Random.Range(-1, 1) * Random.Range(r, R), (int)Random.Range(-1, 1) * Random.Range(r, R));
    }

    public virtual void Update()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player");
        else
        {
            if (player.transform.position.x > Camera.main.ViewportToWorldPoint(new Vector2(0.8f, 0)).x & vect_follow.x > 0) vect_follow = new Vector2(-Random.Range(r, R), (int)Random.Range(-1, 1) * Random.Range(r, R));
            if (player.transform.position.x < Camera.main.ViewportToWorldPoint(new Vector2(0.2f, 0)).x & vect_follow.x < 0) vect_follow = new Vector2(Random.Range(r, R), (int)Random.Range(-1, 1) * Random.Range(r, R));
            if (player.transform.position.y > Camera.main.ViewportToWorldPoint(new Vector2(0, 0.8f)).y & vect_follow.y > 0) vect_follow = new Vector2((int)Random.Range(-1, 1) * Random.Range(r, R), -Random.Range(r, R));
            if (player.transform.position.y < Camera.main.ViewportToWorldPoint(new Vector2(0, 0.2f)).y & vect_follow.y < 0) vect_follow = new Vector2((int)Random.Range(-1, 1) * Random.Range(r, R), Random.Range(r, R));
            if (vect_follow.x == 0) vect_follow.x = Random.Range(r, R);
            if (vect_follow.y == 0) vect_follow.y = Random.Range(r, R);
            transform.position = Vector2.Lerp(transform.position, new Vector2(player.transform.position.x + vect_follow.x, player.transform.position.y + vect_follow.y), 0.05f);
        }
    }
}
