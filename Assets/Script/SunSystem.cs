using UnityEngine;
using UnityEngine.UI;

public class SunSystem : MonoBehaviour
{
    public bool isSunActive = true; // true = ร้อน / false = เมฆบัง

    public float sunMin = 10f;
    public float sunMax = 20f;
    public float cloudMin = 5f;
    public float cloudMax = 10f;

    public Image darkOverlay;

    private float timer = 0f;
    private float nextSwitchTime;

    void Start()
    {
        SetNextTime();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= nextSwitchTime)
        {
            isSunActive = !isSunActive; // สลับสถานะ
            timer = 0f;
            SetNextTime();
        }

        if (darkOverlay != null)
        {
            float targetAlpha = isSunActive ? 0f : 0.5f;
            Color c = darkOverlay.color;
            c.a = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime * 2f);
            darkOverlay.color = c;
        }
    }

    void SetNextTime()
    {
        if (isSunActive)
        {
            nextSwitchTime = Random.Range(sunMin, sunMax);
        }

        else
        {
            nextSwitchTime = Random.Range(cloudMin, cloudMax);
        }
        
        Debug.Log(nextSwitchTime);
    }
}