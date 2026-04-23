using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OverHeatBar : MonoBehaviour
{
    public WaterColler waterColler;
    public PlayerMovement playerMovement;
    public SunSystem sunSystem;
    public BurningZone burningZone;
    public DeadZone deadZone;

    public Slider slider;
    public Image fillImage;
    public GameObject player;

    public float maxValue = 100f;

    [Header("Settings")]
    public float waterCoolingSpeed = 15f;  // ความเร็วตอนแช่น้ำ (แรงสะใจ)
    public float shadowCoolingSpeed = 8f;  // ความเร็วตอนเข้าร่ม (เย็นสบาย)
    public float sunHeatingSpeed = 5f;     // ความเร็วตอนโดนแดดปกติ
    public float burningHeatingSpeed = 25f; // ความเร็วตอนโซนร้อน (อันตราย!)
    public float deadBurnigSpeed = 50f; //ตายแน่ไอ้อ้วนควยโง่ อ่านหาพ่อง

    void Start()
    {
        slider.maxValue = maxValue;
        slider.value = 0;
    }

    void Update()
    {
        // เปลี่ยนมาคำนวณแบบ Real-time ทุกเฟรมเพื่อให้เกจไหลลื่น (Smooth)

        if (playerMovement.isInWater)
        {
            // แช่น้ำ: ลดฮวบๆ
            slider.value -= waterCoolingSpeed * Time.deltaTime;
        }
        else if (burningZone.inBuring == true)
        {
            // โซนร้อน: พุ่งปรี๊ด
            slider.value += burningHeatingSpeed * Time.deltaTime;
        }
        else if (playerMovement.isInShadow)
        {
            // เข้าร่ม: ลดแบบทันใจ
            slider.value -= shadowCoolingSpeed * Time.deltaTime;
        }
        else if (deadZone.isInDead)
        {
            // โซนชิปหาย ตายหยังเขียด
            slider.value += deadBurnigSpeed * Time.deltaTime;
        }
        else if (sunSystem.isSunActive)
        {
            // เดินกลางแดดปกติ: ค่อยๆ เพิ่ม
            slider.value += sunHeatingSpeed * Time.deltaTime;
        }

        // --- รักษาระดับค่า 0-100 ---
        slider.value = Mathf.Clamp(slider.value, 0, maxValue);

        // --- ระบบเช็คความตายและเปลี่ยนสี ---
        if (slider.value >= 100f)
        {
            Debug.Log("YOU ARE DEAD");
            player.SetActive(false);
        }
        else if (slider.value >= 85f) // ปรับให้เตือนแดงเร็วขึ้นนิดนึง
        {
            fillImage.color = Color.red;
        }
        else
        {
            fillImage.color = new Color32(255, 113, 0, 255);
        }
    }
}