using System;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController Instance;

    public GameState GameState = GameState.NIGHT;

    public float TimeUntilDawn = 120f;

    private float m_NightStartedTime = 0;
    private float m_DayTime = 0;

    public int MissingHumansToKill = 1;

    private PlayerStats m_PlayerStats;

    public static event Action<int> OnHumanKilled;
    public static event Action<int> OnHumanKillCounterUpdate;

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        m_PlayerStats = GameObject.FindObjectOfType<PlayerStats>();
        StartNight();
    }

    private void Update()
    {
        switch (GameState)
        {
            case GameState.DAY:
                UpdateDay();
                break;
            case GameState.NIGHT:
                UpdateNight();
                break;
            case GameState.EXPLAINING:
                UpdateExplaining();
                break;

            default:
                Debug.Log("Unknown Game State");
                break;
        }    
    }
    

    public void StartNight()
    {
        m_NightStartedTime = Time.time;
        TimeUntilDawn = m_PlayerStats.NightDuration;
        m_DayTime = Time.time + m_PlayerStats.NightDuration;
        MissingHumansToKill = m_PlayerStats.HumansToKill;
        OnHumanKillCounterUpdate?.Invoke(MissingHumansToKill);
        GameState = GameState.NIGHT;
    }

    public void StartDay()
    {
        GameState = GameState.DAY;
    }

    public void StartExplaining()
    {
        GameState = GameState.EXPLAINING;
    }

    private void UpdateDay()
    {

    }

    private void UpdateNight()
    {
        TimeUntilDawn = m_DayTime - Time.time;
    }

    private void UpdateExplaining()
    {

    }

}
