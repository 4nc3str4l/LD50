using UnityEngine;
using TMPro;

public class UIHomeStats : MonoBehaviour
{

    public TMP_Text TxtDoorStrength;
    public TMP_Text TxtHumansInside;
    public TMP_Text TxtAlam;

    private Bulding m_TrackingBulding;

    private Vector3 m_Offset = new Vector3(-30, 56, 0);

    public void Init(Bulding _buildingToTrack)
    {
        m_TrackingBulding = _buildingToTrack;
    }

    private void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(m_TrackingBulding.UIPos.transform.position);
           
        // Optimize this with events
        TxtDoorStrength.text = "Door Strength: " + m_TrackingBulding.DefensePoints;
        TxtHumansInside.text = "Humans Inside: " + m_TrackingBulding.HumansInside;
        TxtAlam.text = "Has Alarm: " + m_TrackingBulding.Alarm;
    }


}
