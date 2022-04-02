using System;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public int NumNightsSurvived = 0;

    public static GameController Instance;

    public GameState GameState = GameState.NIGHT;

    public float TimeUntilDawn = 120f;

    private float m_NightStartedTime = 0;
    private float m_DayTime = 0;

    public int MissingHumansToKill = 1;

    private PlayerStats m_PlayerStats;

    public static event Action<int> OnHumanKilled;
    public static event Action<int> OnHumanKillCounterUpdate;

    public static event Action OnDayStarted;
    public static event Action OnNightStarted;

    private void Awake()
    {
        Instance = this;
        Storage.SetNightsSurvived(0);
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
                break;
        }    
    }
    

    public void StartNight()
    {
        if(m_PlayerStats.HumansToKill >= 1)
        {
            if(UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f)
            {
                m_PlayerStats.HumansToKill += 1;
            }
        }
        else
        {
            m_PlayerStats.HumansToKill = 1;
        }

        m_NightStartedTime = Time.time;
        TimeUntilDawn = m_PlayerStats.NightDuration;
        m_DayTime = Time.time + m_PlayerStats.NightDuration;
        MissingHumansToKill = m_PlayerStats.HumansToKill;
        OnHumanKillCounterUpdate?.Invoke(MissingHumansToKill);
        GameState = GameState.NIGHT;
        OnNightStarted?.Invoke();
    }

    public void StartDay()
    {
        NumNightsSurvived += 1;
        Storage.SetNightsSurvived(NumNightsSurvived);
        PlayerPrefs.SetInt("NightsSurvived", NumNightsSurvived);
        GameState = GameState.DAY;
        OnDayStarted?.Invoke();
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

    public void SetKillingState()
    {
        GameState = GameState.KILLING;
    }

    public void ClearKillingState()
    {
        GameState = GameState.NIGHT;
    }

    public void FireOnHumanKilled()
    {
        MissingHumansToKill -= 1;
        if (MissingHumansToKill < 0)
        {
            MissingHumansToKill = 0;
        }

        OnHumanKilled?.Invoke(MissingHumansToKill);
        OnHumanKillCounterUpdate?.Invoke(MissingHumansToKill);
    }

    public void IncreaseSpeeed()
    {
        m_PlayerStats.Speed += m_PlayerStats.Speed * 0.03f;
        StartNight();
    }

    public void IncreateNightDuration()
    {
        m_PlayerStats.NightDuration += m_PlayerStats.NightDuration * 0.05f;
        StartNight();
    }

    public void IncreaseStrenght()
    {
        m_PlayerStats.Strength += 1;
        StartNight();
    }
}
