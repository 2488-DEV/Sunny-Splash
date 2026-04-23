using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    public bool IsShovel;

    [Header("Status Settings")]
    public int seed;
    public TextMeshProUGUI seedCount;

    public int tree;
    public TextMeshProUGUI treeCount;

    [Header("Victory Settings")]
    public GameObject victoryPanel;

    [Header("Movement State")]
    public bool isLeft;
    public bool isRight;

    private WaterRefillSystem waterSystem;

    void Start()
    {
        waterSystem = GetComponent<WaterRefillSystem>();

        // ปิด Victory Panel ไว้ก่อนเริ่มเกมกวัก
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }

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

    public void DecreaseTree()
    {
        if (tree > 0)
        {
            tree -= 1;
            UpdateTreeCount();

            // เมื่อรดน้ำจนเหลือ 0 ต้นกวัก!
            if (tree <= 0)
            {
                WinGame();
            }
        }
    }

    void WinGame()
    {
        if (victoryPanel != null)
        {
            // 1. เปิดหน้าต่างชนะกวัก
            victoryPanel.SetActive(true);

            // 2. ปลดล็อกเมาส์ให้กดปุ่มได้
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            // 3. --- แก้ไขตามสั่ง: ปลดล็อกบรรทัดนี้แล้วกวัก! ---
            // หยุดเวลาเกมทั้งหมด แดดจะหยุดเผา เป็ดจะปลอดภัยกวัก!
            Time.timeScale = 0f;
        }
    }

    public void RefreshAllUI()
    {
        UpdateSeedCount();
        UpdateTreeCount();
    }

    public void UpdateSeedCount()
    {
        if (seedCount != null) seedCount.text = "Seed : " + seed;
    }

    public void UpdateTreeCount()
    {
        // อัปเดตข้อความจำนวนต้นไม้ที่เหลือ
        if (treeCount != null) treeCount.text = "Remaining : " + tree;
    }
}