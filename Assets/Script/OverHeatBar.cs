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
    public DeadZone deadZone; // ตัวเชื่อมกับสคริปต์พื้นที่ใหม่กวัก!

    public Slider slider;
    public Image fillImage;
    public GameObject player;

    public GameObject deathPanel;
    private PlayerSound playerSound;

    public float maxValue = 100f;
    [HideInInspector] public bool isInBurningZone;
    [HideInInspector] public bool isInDeadZone; // เช็คว่าอยู่ในเขตมรณะไหมกวัก

    [Header("Settings")]
    public float waterCoolingSpeed = 15f;
    public float shadowCoolingSpeed = 8f;
    public float sunHeatingSpeed = 5f;
    public float burningHeatingSpeed = 25f;
    public float deadZoneHeatingSpeed = 40f; // ค่าความร้อนใหม่สำหรับ DeadZone กวัก!

    void Start()
    {
        slider.maxValue = maxValue;
        slider.value = 0;
        if (player != null) playerSound = player.GetComponent<PlayerSound>();
    }

    void Update()
    {
        // --- อัปเดตสถานะจากโซนต่างๆ กวัก ---
        if (burningZone != null) isInBurningZone = burningZone.inBuring;
        if (deadZone != null) isInDeadZone = deadZone.isInDead;

        // --- 1. ระบบลดความร้อน (น้ำ/ร่ม) ---
        if (playerMovement.isInWater)
        {
            Collider2D waterCheck = Physics2D.OverlapPoint(player.transform.position);
            if (waterCheck != null && waterCheck.CompareTag("ContaminatedWater"))
            {
                slider.value -= (waterCoolingSpeed * 0.5f) * Time.deltaTime;
            }
            else
            {
                slider.value -= waterCoolingSpeed * Time.deltaTime;
            }
        }
        else if (playerMovement.isInShadow)
        {
            slider.value -= shadowCoolingSpeed * Time.deltaTime;
        }
        // --- 2. ระบบเพิ่มความร้อน (เรียงลำดับความแรงกวัก!) ---
        else
        {
            float currentHeatSpeed = 0f;

            if (isInDeadZone)
            {
                // แรงที่สุด! ร้อนแบบไม่สนความแรงแดดกวัก
                currentHeatSpeed = deadZoneHeatingSpeed;
            }
            else if (isInBurningZone)
            {
                // ร้อนรองลงมาและคูณกับความแรงแดด
                currentHeatSpeed = burningHeatingSpeed * sunSystem.sunIntensity;
            }
            else if (sunSystem.isSunActive)
            {
                // ร้อนแดดปกติ
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