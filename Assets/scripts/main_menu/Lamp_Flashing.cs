using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lamp_Flashing : MonoBehaviour
{
    public List<GameObject> Lamps;
    void Start()
    {
        foreach (GameObject lamp in Lamps)
        {
            lamp.GetComponent<Image>().color = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255),255);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject lamp in Lamps)
        {
            lamp.GetComponent<Image>().color = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
        }
        
    }
}
