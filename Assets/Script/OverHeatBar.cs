using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OverHeatBar : MonoBehaviour
{
    public WaterColler waterColler;
    public PlayerMovement playerMovement;
    public SunSystem sunSystem;
    public BurningZone burningZone;

    public Slider slider;
    public Image fillImage;
    public GameObject player;

    public GameObject deathPanel;
    private PlayerSound playerSound;

    public float maxValue = 100f;
    [HideInInspector] public bool isInBurningZone;

    [Header("Settings")]
    public float waterCoolingSpeed = 15f;
    public float shadowCoolingSpeed = 8f;
    public float sunHeatingSpeed = 5f;
    public float burningHeatingSpeed = 25f;

    void Start()
    {
        slider.maxValue = maxValue;
        slider.value = 0;
        if (player != null) playerSound = player.GetComponent<PlayerSound>();
    }

    void Update()
    {
        if (burningZone != null) isInBurningZone = burningZone.inBuring;

        // --- ระบบคำนวณความร้อนใหม่ตามความแรงแดดกวัก! ---
        if (playerMovement.isInWater)
        {
            slider.value -= waterCoolingSpeed * Time.deltaTime;
        }
        else if (playerMovement.isInShadow)
        {
            slider.value -= shadowCoolingSpeed * Time.deltaTime;
        }
        else
        {
            float currentHeatSpeed = 0f;

            if (isInBurningZone)
            {
                // ความร้อนในโซนร้อนจะคูณตามความเข้มแดด (sunIntensity) 
                // ถ้าเมฆบังมิด ค่าจะค่อยๆ กลายเป็น 0 กวัก!
                currentHeatSpeed = burningHeatingSpeed * sunSystem.sunIntensity;
            }
            else if (sunSystem.isSunActive)
            {
                // ความร้อนปกติกลางแจ้งคูณตามความเข้มแดดเช่นกัน
                currentHeatSpeed = sunHeatingSpeed * sunSystem.sunIntensity;
            }

            slider.value += currentHeatSpeed * Time.deltaTime;
        }

        slider.value = Mathf.Clamp(slider.value, 0, maxValue);

        // --- ส่วนจัดการความตาย ---
        if (slider.value >= maxValue)
        {
            if (deathPanel != null && !deathPanel.activeSelf)
            {
                if (playerSound != null) playerSound.PlayActionSound("Die");
                deathPanel.SetActive(true);
                player.SetActive(false);
            }
        }
        else if (slider.value >= 85f) fillImage.color = Color.red;
        else fillImage.color = new Color32(255, 113, 0, 255);
    }

    // --- ระบบเปลี่ยน Scene ---
    public void TryAgain()
    {
        StartCoroutine(DelayLoadScene(SceneManager.GetActiveScene().name));
    }

    public void BackToMenu()
    {
        StartCoroutine(DelayLoadScene("MainMenu"));
    }

    private IEnumerator DelayLoadScene(string sceneName)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }
}