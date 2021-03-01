using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guide_script : MonoBehaviour
{
    public Sprite[] sheets;
    public Image main;

    private int i=0;

    private void Awake()
    {
        main.sprite = sheets[i];
    }

    private void OnEnable()
    {
        i = 0;
        main.sprite = sheets[i];
    }

    public void next()
    {
        i = (i != sheets.Length - 1) ? i+1 : 0;
        main.sprite = sheets[i];
    }

    public void prev()
    {
        i = (i != 0) ? i-1 : sheets.Length - 1;
        main.sprite = sheets[i];
    }

    public void exit()
    {
        this.gameObject.SetActive(false);
    }
}
