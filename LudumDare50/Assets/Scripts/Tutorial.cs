using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tutorial : MonoBehaviour
{

    public CanvasGroup Objective;
    public CanvasGroup Skills;
    public CanvasGroup TimeLeft;
    public CanvasGroup Souls;
    public CanvasGroup DisableTutorial;


    void Start()
    {
        if(Storage.GetTutorialEnabled())
        {
            ShowCanvasGroup(Objective);
            Scheduler.Instance.ExecuteIn(() => {
                HideCanvasGroup(Objective);
                ShowCanvasGroup(Souls);
                Scheduler.Instance.ExecuteIn(() => {
                    HideCanvasGroup(Souls);
                    ShowCanvasGroup(Skills);
                    Scheduler.Instance.ExecuteIn(() => {
                        HideCanvasGroup(Skills);
                        ShowCanvasGroup(TimeLeft);
                        Scheduler.Instance.ExecuteIn(() => {
                            HideCanvasGroup(TimeLeft);
                            ShowCanvasGroup(DisableTutorial);
                            Scheduler.Instance.ExecuteIn(() => {
                                HideCanvasGroup(DisableTutorial);
                            }, 5.0f);
                        }, 5.0f);
                    }, 5.0f);
                }, 5.0f);
            }, 5.0f);
        }
    
    }

    private void ShowCanvasGroup(CanvasGroup _group)
    {
        _group.DOFade(1, 1);
    }

    private void HideCanvasGroup(CanvasGroup _group)
    {
        _group.DOFade(0, 1);
    }
}
