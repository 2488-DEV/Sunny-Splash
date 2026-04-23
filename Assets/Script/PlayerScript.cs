using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    public bool IsShovel;

    [Header("Status Settings")]
    public int seed;
    public TextMeshProUGUI seedCount; // ลาก Text ที่เขียนว่า Seed : มาใส่กวัก

    public int tree; // จำนวนต้นไม้ที่ต้องปลูก/รดน้ำในด่านนี้
    public TextMeshProUGUI treeCount; // ลาก Text ที่เขียนว่า Remaining : มาใส่กวัก

    [Header("Victory Settings")]
    public GameObject victoryPanel;

    [Header("Movement State")]
    public bool isLeft;
    public bool isRight;

    private WaterRefillSystem waterSystem;

    void Start()
    {
        waterSystem = GetComponent<WaterRefillSystem>();

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }

        // รีเฟรชค่าเริ่มต้นทั้งหมดกวัก!
        RefreshAllUI();
    }

    void Update()
    {
        float move = Input.GetAxisRaw("Horizontal");
        if (move != 0)
        {
            isLeft = (move == -1);
            isRight = (move == 1);
        }
    }

    // --- ระบบเมล็ด (Seed) ---
    public void UpdateSeedCount()
    {
        if (seedCount != null)
            seedCount.text = "Seed : " + seed; // โชว์จำนวนเมล็ดที่ถืออยู่กวัก
    }

    public void UseSeed()
    {
        if (seed > 0)
        {
            seed--;
            UpdateSeedCount();
        }
    }

    // --- ระบบต้นไม้ (Tree) ---
    public void DecreaseTree()
    {
        if (tree > 0)
        {
            tree -= 1;
            UpdateTreeCount();

            // เงื่อนไขชัยชนะ: เมื่อปลูก/รดน้ำครบกวัก!
            if (tree <= 0)
            {
                WinGame();
            }
        }
    }

    public void UpdateTreeCount()
    {
        if (treeCount != null)
            treeCount.text = "Remaining : " + tree; // โชว์จำนวนที่เหลือให้ปลูกกวัก
    }

    public void RefreshAllUI()
    {
        UpdateSeedCount();
        UpdateTreeCount();
    }

    // --- ระบบจบเกม ---
    void WinGame()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);

            // ปลดล็อกเมาส์ให้กดปุ่ม Restart/Next ในหน้า Victory กวัก
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            // หยุดทุกอย่าง: แดดไม่เผา เป็ดไม่ขยับกวัก!
            Time.timeScale = 0f;
        }
    }
}