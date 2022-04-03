using UnityEngine;
using TMPro;
using System;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

public class InGameUI : MonoBehaviour
{
    public TMP_Text TxtHumansToKill;
    public TMP_Text TxtTimeUntilDawn;

    public TMP_Text TxtSoulsToCollect;
    public TMP_Text TxtGoBack;

    public TMP_Text TxtOldSouls;

    public GameObject DynamicObjectContainer;

    public TMP_Text TxtNumAccSouls;

    public Image SoulImage;
    public Image OldSoulImage;
    public Image OldSoulImageAcc;

    public static InGameUI Instance;
    public GameObject UIHomeStatsPrefab;

    public ProgressBar ProgTimeUntilDawn;

    private Vector3 m_InitialUISoulScale;
    private Vector3 m_InitialImgSoulScale;


    private Vector3 m_InitialUIOldSoulScale;
    private Vector3 m_InitialImgOldSoulScale;

    public GameObject UITravelingSoulPrefab;
    public GameObject UITransmutingSoulsPrefab;

    private void Awake()
    {
        Instance = this;
        m_InitialUISoulScale = TxtSoulsToCollect.transform.localScale;
        m_InitialImgSoulScale = SoulImage.transform.localScale;

        m_InitialUIOldSoulScale = TxtOldSouls.transform.localScale;
        m_InitialImgOldSoulScale = OldSoulImage.transform.localScale;
    }

    private void OnEnable()
    {
        GameController.OnHumanKillCounterUpdate += GameController_OnHumanKillCounterUpdate;
        Bulding.OnBuldingKilled += Bulding_OnBuldingKilled;

        SoulBank.OnSoulBalanceChanged += SoulBank_OnSoulBalanceChanged;
    }

    private void OnDisable()
    {
        GameController.OnHumanKillCounterUpdate -= GameController_OnHumanKillCounterUpdate;
        Bulding.OnBuldingKilled -= Bulding_OnBuldingKilled;
        SoulBank.OnSoulBalanceChanged -= SoulBank_OnSoulBalanceChanged;
    }

    private void Bulding_OnBuldingKilled(Bulding _target, int numSouls)
    {
        for(int i = 0; i < numSouls; ++i)
        {
            Scheduler.Instance.ExecuteIn(() =>
            {
                GameObject soulVisuals = GameObject.Instantiate(UITravelingSoulPrefab);
                soulVisuals.transform.SetParent(transform);
                soulVisuals.GetComponent<UITravelingSoul>().Init(_target, 2f, SoulImage.transform);
            }, i * 0.15f);

        }
    }

    private void GameController_OnHumanKillCounterUpdate(int _numSouls)
    {
        TxtSoulsToCollect.text = _numSouls.ToString();
        TxtSoulsToCollect.transform.DOShakeScale(0.5f).OnComplete(() =>
        {
            TxtSoulsToCollect.transform.localScale = m_InitialUISoulScale;
            SoulImage.transform.DOShakeScale(1.5f).OnComplete(() =>
            {
                SoulImage.transform.localScale = m_InitialImgSoulScale;
            });
        });
    }

    private void SoulBank_OnSoulBalanceChanged(int _toKill)
    {
        TxtOldSouls.text = _toKill.ToString();
        TxtOldSouls.transform.DOShakeScale(0.1f).OnComplete(() =>
        {
            TxtOldSouls.transform.localScale = m_InitialUIOldSoulScale;
            OldSoulImage.transform.DOShakeScale(0.3f).OnComplete(() =>
            {
                OldSoulImage.transform.localScale = m_InitialImgOldSoulScale;
            });
        });
    }

    private void Update()
    {
        TxtTimeUntilDawn.text = "Time until dawn: " + FormatTime(GameController.Instance.TimeUntilDawn);
        ProgTimeUntilDawn.SetProgress(GameController.Instance.TimeUntilDawn/PlayerStats.Instance.NightDuration);
        TxtGoBack.enabled = Portal.Instance.CanEnterPortal() && GameController.Instance.GameState == GameState.NIGHT;
        TxtOldSouls.text = GameController.Instance.Bank.Balance.ToString();

        if(GameController.Instance.MissingSoulsToCollect < 0)
        {
            ActivateIfNotActive(TxtNumAccSouls);
            ActivateIfNotActive(OldSoulImageAcc);
            TxtNumAccSouls.text = "(+" + Mathf.Abs(GameController.Instance.MissingSoulsToCollect).ToString() + ")";
        }
        else
        {
            DeactivateIfNotActive(TxtNumAccSouls);
            DeactivateIfNotActive(OldSoulImageAcc);
        }
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

    public void PerformSoulTransmutation(Action _onComplete, float _delay)
    {
        StartCoroutine(DoTransmutation(_delay, _onComplete));
    }

    IEnumerator DoTransmutation(float _delay, Action _onComplete)
    {
        yield return new WaitForSeconds(_delay);
        GameObject soulVisuals = GameObject.Instantiate(UITransmutingSoulsPrefab);
        soulVisuals.transform.SetParent(transform);
        soulVisuals.GetComponent<UITransmutationSoul>().Init(SoulImage.transform, 2, OldSoulImage.transform, _onComplete);

    }
    private void DeactivateIfNotActive(MonoBehaviour _cmp)
    {
        if (_cmp.gameObject.activeSelf)
        {
            _cmp.gameObject.SetActive(false);
        }
    }

    public void ActivateIfNotActive(MonoBehaviour _cmp)
    {
        if (!_cmp.gameObject.activeSelf)
        {
            _cmp.gameObject.SetActive(true);
        }
    }

}
