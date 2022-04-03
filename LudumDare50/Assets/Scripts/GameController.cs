using System;
using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public int NumNightsSurvived = 0;

    public static GameController Instance;

    public GameState GameState = GameState.NIGHT;

    public float TimeUntilDawn = 120f;

    private float m_NightStartedTime = 0;
    private float m_DayTime = 0;

    public int MissingSoulsToCollect = 1;

    private PlayerStats m_PlayerStats;

    public static event Action<int> OnHumanKilled;
    public static event Action<int> OnHumanKillCounterUpdate;

    public static event Action OnDayStarted;
    public static event Action OnNightStarted;

    public LoadScene GameOverLoader;

    public SoulBank Bank;

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
        m_PlayerStats.SoulsToCollect += 1;
        m_NightStartedTime = Time.time;
        TimeUntilDawn = m_PlayerStats.NightDuration;
        m_DayTime = Time.time + m_PlayerStats.NightDuration;
        MissingSoulsToCollect = m_PlayerStats.SoulsToCollect;
        OnHumanKillCounterUpdate?.Invoke(MissingSoulsToCollect);
        GameState = GameState.NIGHT;
        OnNightStarted?.Invoke();

        Bank.TransmuteSouls();

    }

    public void StartDay()
    {
        Bank.DepositToTransmute(Mathf.Abs(MissingSoulsToCollect));
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

        if(TimeUntilDawn <= 0)
        {
            PlayerKilled();
        }
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

    public void FireOnHumanKilled(int _numHumans)
    {
        StartCoroutine(AnounceDeaths(_numHumans));
    }

    public void IncreaseSpeeed()
    {
        m_PlayerStats.Speed += m_PlayerStats.Speed * 0.05f;
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

    public void PlayerKilled()
    {
        GameOverLoader.Execute();
    }

    IEnumerator AnounceDeaths(int _numHumans)
    {
        yield return new WaitForSeconds(1);
        MissingSoulsToCollect -= _numHumans;
        OnHumanKilled?.Invoke(MissingSoulsToCollect);
        OnHumanKillCounterUpdate?.Invoke(MissingSoulsToCollect);

    }
}
