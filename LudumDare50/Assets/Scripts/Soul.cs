using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Soul : MonoBehaviour
{

    private ParticleSystem m_ParticleSystem;
    private float m_TravelTime;
    private Orbit m_OrbitScript;

    public void Init(float _travelTime)
    {
        m_TravelTime = _travelTime;
        m_ParticleSystem = GetComponentInChildren<ParticleSystem>();
        Jukebox.Instance.PlaySound(Jukebox.Instance.SoulGoing, 0.7f);
        transform.DOMove(transform.position + Vector3.up * 15 + new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f)), _travelTime / 4).OnComplete(()=>
        {
            m_OrbitScript = gameObject.AddComponent<Orbit>();
            m_OrbitScript.centerPoint = transform.position;
            m_OrbitScript.rotSpeed = Random.Range(0.5f, 1.5f);
        });
    }

    private void OnEnable()
    {
        GameController.OnHumanKillCounterUpdate += GameController_OnHumanKillCounterUpdate;
    }

    private void OnDisable()
    {
        GameController.OnHumanKillCounterUpdate -= GameController_OnHumanKillCounterUpdate;
    }

    private void GameController_OnHumanKillCounterUpdate(int m_NumRemainingSouls)
    {
        if(m_NumRemainingSouls <= 0)
        {
            StartCoroutine(GoToBase());
        }
    }

    private IEnumerator GoToBase()
    {
        yield return new WaitForSeconds(m_TravelTime / 4 + Random.Range(0.3f, 0.6f));
        GameObject.Destroy(m_OrbitScript);
        Jukebox.Instance.PlaySound(Jukebox.Instance.SoulGoing, Random.Range(0.2f, 0.4f));

        transform.DOMove(Portal.Instance.Door.transform.position, m_TravelTime / 4 * 3).OnComplete(() =>
        {
            Portal.Instance.Shake();
            m_ParticleSystem.transform.SetParent(null);
            Jukebox.Instance.PlaySound(Jukebox.Instance.PortalAbsorbing, 0.2f);
            GameObject.Destroy(gameObject);
            GameObject.Destroy(m_ParticleSystem.gameObject, 2.0f);
        });
    }
}
