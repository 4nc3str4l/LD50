using UnityEngine;
using TMPro;

public class UIHomeStats : MonoBehaviour
{
    public TMP_Text TxtDoorStrength;

    private Bulding m_TrackingBulding;

    private Vector3 m_Offset = new Vector3(-30, 56, 0);
    public ProgressBar BuildingHealth;
    public ProgressBar AlarmEnabled;


    public void Init(Bulding _buildingToTrack)
    {
        m_TrackingBulding = _buildingToTrack;
        AlarmEnabled.SetProgress(_buildingToTrack.AlarmSys.GetProgress());
    }

    private void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(m_TrackingBulding.UIPos.transform.position);
           
        // Optimize this with events
        TxtDoorStrength.text = m_TrackingBulding.DefensePoints + "/" + m_TrackingBulding.StartDefensePoints;
        BuildingHealth.SetProgress(m_TrackingBulding.DefensePoints / (float)m_TrackingBulding.StartDefensePoints);

        if(!m_TrackingBulding.AlarmSys.IsEnabled)
        {
            AlarmEnabled.Hide();
        }
        else
        {
            AlarmEnabled.Show();
            AlarmEnabled.SetProgress(m_TrackingBulding.AlarmSys.GetProgress());
        }

    }


}
