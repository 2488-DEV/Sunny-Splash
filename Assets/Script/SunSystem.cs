using UnityEngine;
using UnityEngine.UI;

public class SunSystem : MonoBehaviour
{
    [Header("Status")]
    public bool isSunActive = true;
    [Range(0, 1)]
    public float sunIntensity = 1f; // 1 = ร้อนจัด, 0 = เมฆบังมิด

    [Header("Interval Settings")]
    public float sunMin = 15f;      // เพิ่มเวลาแดดออกให้ไม่น่ารำคาญเกินไป
    public float sunMax = 25f;
    public float cloudMin = 8f;     // เพิ่มเวลาเมฆบังให้น้องเป็ดได้พัก
    public float cloudMax = 15f;

    [Header("Transition")]
    public float transitionSpeed = 1.5f; // ความเร็วตอนเมฆเคลื่อนมาบัง
    public Image darkOverlay;

    private float timer = 0f;
    private float nextSwitchTime;

    void Start()
    {
        SetNextTime();
        sunIntensity = isSunActive ? 1f : 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= nextSwitchTime)
        {
            isSunActive = !isSunActive;
            timer = 0f;
            SetNextTime();
        }

        // --- ระบบคำนวณ Intensity (ความเข้มแดด) ---
        float targetIntensity = isSunActive ? 1f : 0f;
        sunIntensity = Mathf.Lerp(sunIntensity, targetIntensity, Time.deltaTime * transitionSpeed);

        // ปรับความมืดจอตาม Intensity
        if (darkOverlay != null)
        {
            // ถ้า Intensity = 1 (แดดจ้า) Alpha จะเป็น 0 (ใส)
            // ถ้า Intensity = 0 (เมฆบัง) Alpha จะเป็น 0.5 (มืด)
            float targetAlpha = Mathf.Lerp(0.5f, 0f, sunIntensity);
            Color c = darkOverlay.color;
            c.a = targetAlpha;
            darkOverlay.color = c;
        }
    }

    void SetNextTime()
    {
        nextSwitchTime = isSunActive ? Random.Range(sunMin, sunMax) : Random.Range(cloudMin, cloudMax);
        Debug.Log($"ถัดไปคือ: {(isSunActive ? "แดดออก" : "เมฆบัง")} ในอีก {nextSwitchTime:F1} วินาที");
    }
}