using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class Teletype : MonoBehaviour
{
    private TMP_Text m_TextToTeletype;
    public float Speed = 0.1f;

    private string m_OriginalText;
    private string m_CurrentText;

    private WaitForSeconds m_Waiter;
    private WaitForSeconds m_CommaWaiter;
    private WaitForSeconds m_ParrafWaiter;


    private int m_Position;

    private void Awake()
    {
        m_TextToTeletype = GetComponent<TMP_Text>();
        m_CurrentText = "";
        m_OriginalText = m_TextToTeletype.text;
        m_Waiter = new WaitForSeconds(Speed);
        m_CommaWaiter = new WaitForSeconds(Speed * 2);
        m_ParrafWaiter = new WaitForSeconds(Speed * 3);
        m_Position = 0;
    }

    private void Start()
    {
        StartCoroutine(WriteText());
    }

    private IEnumerator WriteText()
    {
        while(m_Position < m_OriginalText.Length)
        {
            m_CurrentText += m_OriginalText[m_Position];
            m_TextToTeletype.text = m_CurrentText;
            if(m_OriginalText[m_Position] == ',')
            {
                yield return m_CommaWaiter;
            }
            else if (m_OriginalText[m_Position] == '\n')
            {
                yield return m_ParrafWaiter;
            }
            else
            {
                yield return m_Waiter;
            }
            ++m_Position;
        }
    }
}
