using UnityEngine;
using TMPro;
using System;

public class InGameUI : MonoBehaviour
{
    public TMP_Text TxtHumansToKill;
    public TMP_Text TxtTimeUntilDawn;

    public static InGameUI Instance;
    public GameObject UIHomeStatsPrefab;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        GameController.OnHumanKillCounterUpdate += GameController_OnHumanKillCounterUpdate; ;
    }

    private void OnDisable()
    {
        GameController.OnHumanKillCounterUpdate -= GameController_OnHumanKillCounterUpdate;
    }

    private void GameController_OnHumanKillCounterUpdate(int _toKill)
    {
        TxtHumansToKill.text = "Humans to kill tonight: " + _toKill.ToString();
    }


    private void Update()
    {
        TxtTimeUntilDawn.text = "Time until dawn: " + FormatTime(GameController.Instance.TimeUntilDawn);
    }

    private string FormatTime(float _seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(_seconds);
        return time.ToString(@"mm\:ss");

    }

    public void SpawnHomeUI(Bulding _spawnmer)
    {
        GameObject ui = GameObject.Instantiate(UIHomeStatsPrefab);
        ui.transform.SetParent(transform);
        ui.GetComponent<UIHomeStats>().Init(_spawnmer);
    }
}
