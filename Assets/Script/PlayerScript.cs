using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    public bool IsShovel;

    [Header("Status Settings")]
    public int water_gauge = 100;
    public Slider waterBar;

    public int seed;
    public TextMeshProUGUI seedCount;

    public int tree;
    public TextMeshProUGUI treeCount;

    [Header("Victory Settings")]
    public GameObject victoryPanel; // ลาก VictoryPanel มาใส่ในช่องนี้ที่ Inspector

    [Header("Movement State")]
    public bool isLeft;
    public bool isRight;
    public float timer = 0f;

    void Start()
    {
        // ตรวจสอบว่าลืมเปิดทิ้งไว้หรือไม่ ถ้าลืมให้สั่งปิดก่อนเริ่มเกม
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
            victoryPanel.SetActive(true); // สั่งเปิดหน้าต่าง Victory

            // ปลดล็อคเมาส์ให้กดปุ่มได้ (ถ้าเกมมีการล็อคเมาส์ไว้)
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            // หยุดเวลาในเกมเพื่อให้ผู้เล่นขยับตัวไม่ได้ (ทางเลือก)
            // Time.timeScale = 0f; 
        }
    }

    public void RefreshAllUI()
    {
        UpdateWater();
        UpdateSeedCount();
        UpdateTreeCount();
    }

    public void UpdateWater()
    {
        if (waterBar != null) waterBar.value = water_gauge;
    }

    public void UpdateSeedCount()
    {
        if (seedCount != null) seedCount.text = "Seed : " + seed;
    }

    public void UpdateTreeCount()
    {
        if (treeCount != null) treeCount.text = "Remaining : " + tree;
    }
}