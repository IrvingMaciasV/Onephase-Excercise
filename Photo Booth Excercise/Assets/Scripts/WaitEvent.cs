using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaitEvent : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] UnityEvent events;

    private void OnEnable()
    {
        StartCoroutine(InvokeEvents());
    }

    IEnumerator InvokeEvents()
    {
        yield return new WaitForSeconds(duration);

        events.Invoke();
    }
}
