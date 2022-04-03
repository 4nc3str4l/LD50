using System;
using System.Collections;
using UnityEngine;

public class Scheduler : MonoBehaviour
{

    public static Scheduler Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void ExecuteIn(Action _action, float _delay)
    {
        StartCoroutine(WaitAndExecute(_delay, _action));
    }

    IEnumerator WaitAndExecute(float _delay, Action _action)
    {
        yield return new WaitForSeconds(_delay);
        _action?.Invoke();
    }
}