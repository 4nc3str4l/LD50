using UnityEngine;
using TMPro;
using System;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

public class InGameUI : MonoBehaviour
{
    public TMP_Text TxtHumansToKill;

    public TMP_Text TxtSoulsToCollect;

    public CanvasGroup PannelGoBack;
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
    public GameObject ProgressUntilDawnObj;
    public TMP_Text TxtTimeUntilDawn;

    private Vector3 m_InitialUISoulScale;
    private Vector3 m_InitialImgSoulScale;


    private Vector3 m_InitialUIOldSoulScale;
    private Vector3 m_InitialImgOldSoulScale;

    public GameObject UITravelingSoulPrefab;
    public GameObject UITransmutingSoulsPrefab;

    private Vector3 m_TimeUntilDawnPosition;
    private Vector3 m_TimeUntilDawnScale;

    public Vector3 m_TimeUntilDawnTargetPos;
    public Vector3 m_TimeUntilDawnTargetScale;

    public bool SkipReposition = true;

    private void Awake()
    {
        Instance = this;
        m_InitialUISoulScale = TxtSoulsToCollect.transform.localScale;
        m_InitialImgSoulScale = SoulImage.transform.localScale;

        m_InitialUIOldSoulScale = TxtOldSouls.transform.localScale;
        m_InitialImgOldSoulScale = OldSoulImage.transform.localScale;

        m_TimeUntilDawnScale = ProgressUntilDawnObj.transform.localScale;

    }

    private void OnEnable()
    {
        GameController.OnHumanKillCounterUpdate += GameController_OnHumanKillCounterUpdate;
        Bulding.OnBuldingKilled += Bulding_OnBuldingKilled;

        SoulBank.OnSoulBalanceChanged += SoulBank_OnSoulBalanceChanged;

        TimeController.OnLessThan1Min += TimeController_OnLessThan1Min;
        TimeController.OnLessThan30Secs += TimeController_OnLessThan30Secs;
        TimeController.OnLessThan10Secs += TimeController_OnLessThan10Secs;
        TimeController.OnMoreThan1Min += TimeController_OnMoreThan1Min;
    }


    private void OnDisable()
    {
        GameController.OnHumanKillCounterUpdate -= GameController_OnHumanKillCounterUpdate;
        Bulding.OnBuldingKilled -= Bulding_OnBuldingKilled;
        SoulBank.OnSoulBalanceChanged -= SoulBank_OnSoulBalanceChanged;
        TimeController.OnLessThan1Min -= TimeController_OnLessThan1Min;
        TimeController.OnLessThan30Secs -= TimeController_OnLessThan30Secs;
        TimeController.OnLessThan10Secs -= TimeController_OnLessThan10Secs;
        TimeController.OnMoreThan1Min -= TimeController_OnMoreThan1Min;

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
        TxtTimeUntilDawn.text = "Time until dawn: " + FormatTime(GameController.Instance.TimeControl.TimeUntilDawn);
        ProgTimeUntilDawn.SetProgress(GameController.Instance.TimeControl.TimeUntilDawn/PlayerStats.Instance.NightDuration);
        PannelGoBack.alpha = Portal.Instance.CanEnterPortal() && GameController.Instance.GameState == GameState.NIGHT ? 1 : 0;
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


    private void TimeController_OnMoreThan1Min()
    {
        ResetCountdownPositionAndScale();
    }

    private void ResetCountdownPositionAndScale()
    {
        if (SkipReposition)
        {
            SkipReposition = false;
            return; 
        }
        ProgressUntilDawnObj.transform.localScale = m_TimeUntilDawnScale;
    }

    private void TimeController_OnLessThan1Min()
    {
        ProgressUntilDawnObj.transform.DOShakeScale(1f).OnComplete(() =>
        {
            ResetCountdownPositionAndScale();
        });
    }

    private void TimeController_OnLessThan10Secs()
    {
        ProgressUntilDawnObj.transform.DOShakeScale(10f).OnComplete(() =>
        {
            ResetCountdownPositionAndScale();
        });
    }

    private void TimeController_OnLessThan30Secs()
    {
        ProgressUntilDawnObj.transform.DOShakeScale(3f).OnComplete(() =>
        {
            ResetCountdownPositionAndScale();
        });
    }



}
