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
            StartCoroutine(Flashlight(lamp, Random.Range(1f,4f), Random.Range(2f, 3f)));
        }
    }

    private void OnEnable()
    {
        foreach (GameObject lamp in Lamps)
        {
            StartCoroutine(Flashlight(lamp, Random.Range(1f, 4f), Random.Range(2f, 3f)));
        }
    }

    private IEnumerator Flashlight(GameObject lamp, float work_time,float sleep_time) {
        int color_id = Random.Range(0, 2);
        lamp.GetComponent<Image>().color = Color.blue;


        lamp.SetActive(Random.Range(0, 2) == 1);
        while (true)
        {
            if (lamp.activeSelf)
            {
                lamp.SetActive(false);
                yield return new WaitForSecondsRealtime(sleep_time);
            }
            else
            {
                lamp.SetActive(true);
                yield return new WaitForSecondsRealtime(work_time);
            }
        }

    }
}
