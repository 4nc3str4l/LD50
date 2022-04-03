using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jukebox : MonoBehaviour
{

    public static Jukebox Instance;
    public AudioSource TargetSource;

    public AudioClip GoGetMySouls;

    public AudioClip HouseHit1;
    public AudioClip HouseHit2;

    public AudioClip Attack1;
    public AudioClip Attack2;
    public AudioClip Attack3;
    public AudioClip Attack4;

    public AudioClip DoorBreak;

    public AudioClip Scream;
    public AudioClip ManScream;
    public AudioClip OShit;
    public AudioClip HelloThere;
    public AudioClip OhNoo;
    public AudioClip ThatsMaSoul;
    public AudioClip GirlNoo;
    public AudioClip Ahh;

    public AudioClip SoulGoing;
    public AudioClip SoulAscending;
    public AudioClip SoulAscending2;

    public AudioClip PortalEnter;
    public AudioClip PortalAbsorbing;

    public AudioClip SoulTransmutationStart;
    public AudioClip SoulTransmutationEnd;

    public AudioClip CantActivate;
    public AudioClip ButtonPress;
    public AudioClip Sprint;

    public AudioClip GoodJob;
    public AudioClip GoodJobSoulMine;

    public AudioClip YourSoulIsMine;

    public AudioClip HeyYou;
    public AudioClip ComeHere;

    public AudioClip Die;
    public AudioClip Crunch;

    public AudioClip Alarm;


    private List<AudioClip> m_PeapoleReactions = new List<AudioClip>();

    private List<AudioClip> m_Attacks = new List<AudioClip>();

    private void Awake()
    {
        Instance = this;
        m_PeapoleReactions.Add(Scream);
        m_PeapoleReactions.Add(ManScream);
        m_PeapoleReactions.Add(OShit);
        m_PeapoleReactions.Add(HelloThere);
        m_PeapoleReactions.Add(OhNoo);
        m_PeapoleReactions.Add(ThatsMaSoul);
        m_PeapoleReactions.Add(GirlNoo);
        m_PeapoleReactions.Add(Ahh);

        m_Attacks.Add(Attack1);
        m_Attacks.Add(Attack2);
        m_Attacks.Add(Attack3);
        m_Attacks.Add(Attack4);
    }


    public void PlayRandomDoorHit(float _volumne)
    {
        if(Random.Range(0.0f, 1.0f) > 0.5f)
        {
            PlaySound(HouseHit1, _volumne);
        }
        else
        {
            PlaySound(HouseHit2, _volumne);
        }
    }

    public void PlayRandomGoodJob(float _volumne, float _delay)
    {
        if (Random.Range(0.0f, 1.0f) > 0.5f)
        {
            PlaySoundDelayed(GoodJob, _volumne, _delay);
        }
        else
        {
            PlaySoundDelayed(GoodJob, _volumne, _delay);
        }
    }

    public void PlayRandomPeapoleReaction(float _volume, float _delay)
    {
        PlaySoundDelayed(m_PeapoleReactions[Random.Range(0, m_PeapoleReactions.Count)], _volume, _delay);
    }

    public void PlayRandomAttack(float _volume, float _delay)
    {
        PlaySoundDelayed(m_Attacks[Random.Range(0, m_Attacks.Count)], _volume, _delay);
    }

    public void PlaySound(AudioClip _clip, float _volumne)
    {
        TargetSource.pitch = Random.Range(0.90f, 1.1f);
        TargetSource.PlayOneShot(_clip, _volumne * VolumeMaster.Instance.Volume);
    }

    public void PlaySoundDelayed(AudioClip _clip, float _volumne, float _delay)
    {
        StartCoroutine(WaitAndPlay(_clip, _volumne, _delay));
    }

    IEnumerator WaitAndPlay(AudioClip _clip, float _volumne, float _delay)
    {
        yield return new WaitForSeconds(_delay);
        PlaySound(_clip, _volumne);
    }

}
