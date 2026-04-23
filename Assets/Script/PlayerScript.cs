using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    public bool IsShovel;

    [Header("Status Settings")]
    // เราจะไม่ใช้ water_gauge ตรงๆ ในนี้แล้ว แต่จะอ้างอิงไปที่ WaterRefillSystem แทน
    public int seed;
    public TextMeshProUGUI seedCount;

    public int tree;
    public TextMeshProUGUI treeCount;

    [Header("Victory Settings")]
    public GameObject victoryPanel;

    [Header("Movement State")]
    public bool isLeft;
    public bool isRight;

    private WaterRefillSystem waterSystem; // ตัวแปรอ้างอิงระบบน้ำใหม่

    void Start()
    {
        waterSystem = GetComponent<WaterRefillSystem>();

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
            victoryPanel.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            // Time.timeScale = 0f; // ถ้าอยากให้เกมหยุดนิ่งตอนชนะ ให้ติ๊กออกครับ
        }
    }

    public void RefreshAllUI()
    {
        UpdateSeedCount();
        UpdateTreeCount();
        // ส่วนของ Water จะถูกจัดการโดยอัตโนมัติในสคริปต์ WaterRefillSystem
    }

    public void UpdateSeedCount()
    {
        if (seedCount != null) seedCount.text = "Seed : " + seed;
    }

    public void UpdateTreeCount()
    {
        if (treeCount != null) treeCount.text = "Remaining : " + tree;
    }

    // ลบ UpdateWater() เก่าออก เพราะเราใช้ระบบ Smooth ใน WaterRefillSystem แล้วครับ
}