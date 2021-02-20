using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldDrone : MonoBehaviour
{
    [Header("Большой радиус")]
    [Range(0, 20)]
    public int R;
    [Header("Малый радиус")]
    [Range(0, 20)]
    public int r;
    private Vector2 vect_follow;

    private GameObject player;
    public ParticleSystem Sheild;


    // Update is called once per frame
    void Update()
    {
    }
}

