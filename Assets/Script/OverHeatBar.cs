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

        // --- ระบบดับร้อนอัปเกรดใหม่ แยกประเภทน้ำกวัก! ---
        if (playerMovement.isInWater)
        {
            // เช็คว่าตำแหน่งที่เป็ดอยู่นั้นคือ Tag อะไรกวัก
            Collider2D waterCheck = Physics2D.OverlapPoint(player.transform.position);

            if (waterCheck != null && waterCheck.CompareTag("ContaminatedWater"))
            {
                // ถ้าน้ำปนเปื้อน: ลดความร้อนแค่ครึ่งเดียว (หรือตั้งค่าใหม่ตามใจนายกวัก)
                slider.value -= (waterCoolingSpeed * 0.5f) * Time.deltaTime;
            }
            else
            {
                // ถ้าน้ำปกติ: ลดความร้อนเต็มสปีด
                slider.value -= waterCoolingSpeed * Time.deltaTime;
            }
        }
        else if (playerMovement.isInShadow)
        {
            slider.value -= shadowCoolingSpeed * Time.deltaTime;
        }
        else
        {
            // ส่วนเพิ่มความร้อนกลางแดด (ที่นายบอกว่าใช้ได้แล้วกวัก!)
            float currentHeatSpeed = 0f;

            if (isInBurningZone)
            {
                currentHeatSpeed = burningHeatingSpeed * sunSystem.sunIntensity;
            }
            else if (sunSystem.isSunActive)
            {
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