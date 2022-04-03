using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public Material OriginalMaterial;
    public Material FuryMaterial;

    private Light m_Light;

    private Vector3 m_OriginalPosition;

    public float TimeOffset = 0;

    private void OnEnable()
    {
        m_OriginalPosition = transform.localPosition;
        m_Light = GetComponent<Light>();
        m_Light.enabled = false;
        FuriousHands.OnFuryStart += FuriousHands_OnFuryStart;
        FuriousHands.OnFuryEnd += FuriousHands_OnFuryEnd;
    }

    private void OnDisable()
    {
        FuriousHands.OnFuryStart -= FuriousHands_OnFuryStart;
        FuriousHands.OnFuryEnd -= FuriousHands_OnFuryEnd;
    }

    private void Update()
    {
        transform.localPosition = m_OriginalPosition + new Vector3(Mathf.Sin(Time.time * 2 + TimeOffset) * 0.2f, Mathf.Sin(Time.time * 2 + TimeOffset) * 0.2f, 0);
    }

    private void FuriousHands_OnFuryStart()
    {
        gameObject.GetComponent<MeshRenderer>().material = FuryMaterial;
        m_Light.enabled = true;
    }

    private void FuriousHands_OnFuryEnd()
    {
        gameObject.GetComponent<MeshRenderer>().material = OriginalMaterial;
        m_Light.enabled = false;
    }
}
