using UnityEngine;
using TMPro;
using System;
using DG.Tweening;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public TMP_Text TxtHumansToKill;
    public TMP_Text TxtTimeUntilDawn;
    public TMP_Text TxtSoulsToCollect;
    public TMP_Text TxtGoBack;

    public GameObject DynamicObjectContainer;


    public Image SoulImage;

    public static InGameUI Instance;
    public GameObject UIHomeStatsPrefab;

    public ProgressBar ProgTimeUntilDawn;

    private Vector3 m_InitialUISoulScale;
    private Vector3 m_InitialImgSoulScale;

    public GameObject UITravelingSoulPrefab;

    private void Awake()
    {
        Instance = this;
        m_InitialUISoulScale = TxtSoulsToCollect.transform.localScale;
        m_InitialImgSoulScale = SoulImage.transform.localScale;
    }

    private void OnEnable()
    {
        GameController.OnHumanKillCounterUpdate += GameController_OnHumanKillCounterUpdate;
        Bulding.OnBuldingKilled += Bulding_OnBuldingKilled;
    }

    private void OnDisable()
    {
        GameController.OnHumanKillCounterUpdate -= GameController_OnHumanKillCounterUpdate;
        Bulding.OnBuldingKilled -= Bulding_OnBuldingKilled;
    }

    private void Bulding_OnBuldingKilled(Bulding _target, int numSouls)
    {
        for(int i = 0; i < numSouls; ++i)
        {
            GameObject soulVisuals = GameObject.Instantiate(UITravelingSoulPrefab);
            soulVisuals.transform.SetParent(transform);
            soulVisuals.GetComponent<UITravelingSoul>().Init(_target, 2f, SoulImage.transform);
        }
    }


    private void GameController_OnHumanKillCounterUpdate(int _toKill)
    {
        TxtSoulsToCollect.text = _toKill.ToString();
        TxtSoulsToCollect.transform.DOShakeScale(0.5f).OnComplete(() =>
        {
            TxtSoulsToCollect.transform.localScale = m_InitialUISoulScale;
            SoulImage.transform.DOShakeScale(1.5f).OnComplete(() =>
            {
                SoulImage.transform.localScale = m_InitialImgSoulScale;
            });
        });
    }

    private void Update()
    {
        TxtTimeUntilDawn.text = "Time until dawn: " + FormatTime(GameController.Instance.TimeUntilDawn);
        ProgTimeUntilDawn.SetProgress(GameController.Instance.TimeUntilDawn/PlayerStats.Instance.NightDuration);
        TxtGoBack.enabled = Portal.Instance.CanEnterPortal() && GameController.Instance.GameState == GameState.NIGHT;
    }

    private string FormatTime(float _seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(_seconds);
        return time.ToString(@"mm\:ss");
    }

    public void SpawnHomeUI(Bulding _spawnmer)
    {
        GameObject ui = GameObject.Instantiate(UIHomeStatsPrefab);
        ui.transform.SetParent(DynamicObjectContainer.transform);
        ui.GetComponent<UIHomeStats>().Init(_spawnmer);
    }
}
