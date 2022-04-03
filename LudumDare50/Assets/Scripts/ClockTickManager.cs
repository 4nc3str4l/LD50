using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockTickManager : MonoBehaviour
{
    private AudioSource m_Source;

    private void Awake()
    {
        m_Source = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        TimeController.OnLessThan1Min += TimeController_OnLessThan1Min;
        TimeController.OnLessThan30Secs += TimeController_OnLessThan30Secs;
        TimeController.OnLessThan10Secs += TimeController_OnLessThan10Secs;
        TimeController.OnMoreThan1Min += TimeController_OnMoreThan1Min;
    }


    private void OnDisable()
    {
        TimeController.OnLessThan1Min -= TimeController_OnLessThan1Min;
        TimeController.OnLessThan30Secs -= TimeController_OnLessThan30Secs;
        TimeController.OnLessThan10Secs -= TimeController_OnLessThan10Secs;
        TimeController.OnMoreThan1Min -= TimeController_OnMoreThan1Min;
    }

    private void Update()
    {
        if(GameController.Instance.GameState == GameState.DAY)
        {
            m_Source.mute = true;
        }
        else
        {
            m_Source.mute = false;
            float totalAvaliableTime = PlayerStats.Instance.NightDuration;
            float currentAvailable = GameController.Instance.TimeControl.TimeUntilDawn;
            m_Source.volume = VolumeMaster.Instance.Volume * (1 - currentAvailable / totalAvaliableTime);
        }

    }

    private void TimeController_OnMoreThan1Min()
    {
        m_Source.Stop();
        m_Source.Play();
    }

    private void TimeController_OnLessThan10Secs()
    {
        m_Source.PlayOneShot(Jukebox.Instance.Timeout);
    }

    private void TimeController_OnLessThan30Secs()
    {
        m_Source.PlayOneShot(Jukebox.Instance.TimeThreshold);
    }

    private void TimeController_OnLessThan1Min()
    {
        m_Source.PlayOneShot(Jukebox.Instance.TimeThreshold);
    }

}
