using UnityEngine;
using UnityEngine.UI;

public class WeatherUI : MonoBehaviour
{
    [Header("UI Image Reference")]
    public Image weatherIcon;

    [Header("Weather Sprites")]
    public Sprite sunSprite;
    public Sprite cloudSunSprite;
    public Sprite burningSunSprite;

    [Header("Systems Reference")]
    public SunSystem sunSystem;
    public OverHeatBar overHeatBar;

    void Start()
    {
        if (sunSystem == null) sunSystem = Object.FindFirstObjectByType<SunSystem>();
        if (overHeatBar == null) overHeatBar = Object.FindFirstObjectByType<OverHeatBar>();
    }

    void Update()
    {
        if (weatherIcon == null) return;

        // เช็คลำดับสถานะ: โซนร้อน > แดดออก > เมฆบัง
        if (overHeatBar != null && overHeatBar.isInBurningZone)
        {
            weatherIcon.sprite = burningSunSprite;
        }
        else if (sunSystem != null && sunSystem.isSunActive)
        {
            weatherIcon.sprite = sunSprite;
        }
        else if (sunSystem != null && !sunSystem.isSunActive)
        {
            weatherIcon.sprite = cloudSunSprite;
        }
    }
}