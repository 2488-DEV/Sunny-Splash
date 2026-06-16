using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider slider;

    [Header("Stamina Settings")]
    public float currentStamina = 100f;
    public float maxValue = 100f;

    [Header("Regen Settings")]
    public float regenRate = 15f;    // สปีดตอนยืนนิ่ง
    public float walkRegenMultiplier = 0.4f; // เดินไปรีไป จะได้ความเร็วแค่ 40% ของปกติ
    public float idleRegenDelay = 1f;

    [Header("Smoothness")]
    public float smoothSpeed = 8f;

    private float timer = 0f;
    private PlayerScript player;
    public PlayerMovement playerMovement;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.GetComponent<PlayerScript>();

        currentStamina = maxValue;
        if (slider != null)
        {
            slider.maxValue = maxValue;
            slider.value = currentStamina;
        }
    }

    void Update()
    {
        if (PlayerActionManager.Instance != null && PlayerActionManager.Instance.IsPerformingAction)
        {
            timer = 0f;
            UpdateUI();
            return;
        }

        // --- 1. เช็คเงื่อนไขการ "ใช้" Stamina ---
        if (playerMovement != null && playerMovement.isPlayerRunning && IsMoving())
        {
            currentStamina -= 15f * Time.deltaTime;
            timer = 0f;
        }
        else if (player != null && player.IsShovel && IsMoving())
        {
            currentStamina -= 5f * Time.deltaTime;
            timer = 0f;
        }
        // --- 2. เช็คเงื่อนไขการ "ฟื้นฟู" (Regen) ---
        else
        {
            // ถ้าไม่ได้วิ่ง และ ไม่ได้ถือจอบขุด (แปลว่ายืนนิ่ง หรือ เดินตัวเปล่า)
            timer += Time.deltaTime;

            if (timer >= idleRegenDelay)
            {
                float finalRegen = regenRate;

                // ถ้ากำลังเดินตัวเปล่า ให้รีช้าลงตามตัวคูณกวัก!
                if (IsMoving())
                {
                    finalRegen = regenRate * walkRegenMultiplier;
                }

                currentStamina = Mathf.Min(maxValue, currentStamina + finalRegen * Time.deltaTime);
            }
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxValue);
        UpdateUI();
    }

    bool IsMoving()
    {
        return Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0;
    }

    void UpdateUI()
    {
        if (slider != null)
        {
            slider.value = Mathf.MoveTowards(slider.value, currentStamina, smoothSpeed * 100f * Time.deltaTime);
        }
    }
}