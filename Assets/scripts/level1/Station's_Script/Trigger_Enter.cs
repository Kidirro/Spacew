using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Trigger_Enter : MonoBehaviour
{
    public UnityEvent SendTriggerEnterEvent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")&&SendTriggerEnterEvent!=null)
        {
            SendTriggerEnterEvent.Invoke();
        }
    }
}
