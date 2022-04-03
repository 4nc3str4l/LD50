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

        Jukebox.Instance.PlaySound(Jukebox.Instance.PortalAbsorbing, 0.6f);
        if (NumNightsSurvived == 0)
        {
            Jukebox.Instance.PlaySoundDelayed(Jukebox.Instance.GoGetMySouls, 0.6f, 0.5f);
        }
        else
        {
            if(UnityEngine.Random.Range(0.0f, 1.0f) > 0.7f)
            {
                Jukebox.Instance.PlaySoundDelayed(Jukebox.Instance.GoGetMySouls, 0.4f, UnityEngine.Random.Range(1.5f, 2.5f));
            }
        }


    }

    public void StartDay()
    {
        Bank.DepositToTransmute(Mathf.Abs(MissingSoulsToCollect));
        NumNightsSurvived += 1;
        Storage.SetNightsSurvived(NumNightsSurvived);
        PlayerPrefs.SetInt("NightsSurvived", NumNightsSurvived);
        GameState = GameState.DAY;
        OnDayStarted?.Invoke();

        Jukebox.Instance.PlayRandomGoodJob(0.6f, 1f);

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
        Jukebox.Instance.PlaySound(Jukebox.Instance.ButtonPress, 0.5f);
        m_PlayerStats.Speed += m_PlayerStats.Speed * 0.05f;
        StartNight();
    }

    public void IncreateNightDuration()
    {
        Jukebox.Instance.PlaySound(Jukebox.Instance.ButtonPress, 0.5f);
        m_PlayerStats.NightDuration += m_PlayerStats.NightDuration * 0.05f;
        StartNight();
    }

    public void IncreaseStrenght()
    {
        Jukebox.Instance.PlaySound(Jukebox.Instance.ButtonPress, 0.5f);
        m_PlayerStats.Strength += 1;
        StartNight();
    }

    public void PlayerKilled()
    {
        if(GameState == GameState.NIGHT)
        {
            Jukebox.Instance.PlaySound(Jukebox.Instance.Die, 0.6f);
            Jukebox.Instance.PlaySound(Jukebox.Instance.Crunch, 0.7f);
            GameState = GameState.EXPLAINING;
            Scheduler.Instance.ExecuteIn(() =>
            {
                GameOverLoader.Execute();
            }, 0.2f);

        }
    }

    IEnumerator AnounceDeaths(int _numHumans)
    {
        yield return new WaitForSeconds(2);
        MissingSoulsToCollect -= _numHumans;
        OnHumanKillCounterUpdate?.Invoke(MissingSoulsToCollect);
        OnHumanKilled?.Invoke(MissingSoulsToCollect);

    }
}
