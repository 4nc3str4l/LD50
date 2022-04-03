using System;
using UnityEngine;


public class TimeController : MonoBehaviour
{

    public TimeLeftState Status;

    public float RemainingTime { get; private set; }
    
    private float m_DayTime = 0;

    public float TimeUntilDawn { get; private set; }

    public static event Action OnMoreThan1Min;
    public static event Action OnLessThan1Min;
    public static event Action OnLessThan30Secs;
    public static event Action OnLessThan10Secs;
    public static event Action OnTimeout;


    public void Reset(float _newTime)
    {
        Status = TimeLeftState.MORE_THAN_1_MIN;
        RemainingTime = _newTime;
        m_DayTime = Time.time + RemainingTime;
        TimeUntilDawn = m_DayTime - Time.time;
        OnMoreThan1Min?.Invoke();

    }

    // Update is called once per frame
    void Update()
    {
        if(GameController.Instance.GameState != GameState.NIGHT && GameController.Instance.GameState != GameState.KILLING)
        {
            return;
        }

        TimeUntilDawn = m_DayTime - Time.time;

        switch (Status)
        {
            case TimeLeftState.MORE_THAN_1_MIN:
                ExecuteActionWhen(() =>
                {
                    OnLessThan1Min?.Invoke();
                }, 60, TimeLeftState.LESS_THAN_1_MIN);
                break;
            case TimeLeftState.LESS_THAN_1_MIN:
                ExecuteActionWhen(() =>
                {
                    OnLessThan30Secs?.Invoke();
                }, 30, TimeLeftState.LESS_THAN_30_SECS);
                break;
            case TimeLeftState.LESS_THAN_30_SECS:
                ExecuteActionWhen(() =>
                {
                    OnLessThan10Secs?.Invoke();
                }, 10, TimeLeftState.LESS_THAN_10_SECS);
                break;
            case TimeLeftState.LESS_THAN_10_SECS:
                ExecuteActionWhen(() =>
                {
                    OnTimeout?.Invoke();
                }, 0, TimeLeftState.TIMEOUT);
                break;
            default:
                break;
        }
    }

    public void ExecuteActionWhen(Action _action, float _threshold, TimeLeftState _newState)
    {
        if(TimeUntilDawn >= _threshold)
        {
            return;
        }

        _action?.Invoke();
        Status = _newState;
    }
}
