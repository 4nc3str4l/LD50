using DG.Tweening;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{

    public static WeatherManager Instance;

    public Light SunLight;
    private Color m_OriginaSunColor;

    public void Awake()
    {
        Instance = this;
        m_OriginaSunColor = SunLight.color;
    }

    public void SetSunlightColor(Color _color, float _duration)
    {
        SunLight.DOColor(_color, _duration);
        SunLight.DOIntensity(1.2f, _duration);
    }

    public void SetSunToNormal(float _duration)
    {
        SunLight.DOColor(m_OriginaSunColor, _duration);
        SunLight.DOIntensity(0.2f, _duration);

    }

}
