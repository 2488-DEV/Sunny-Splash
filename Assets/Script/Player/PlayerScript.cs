using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections; // จำเป็นต้องมีเพื่อใช้ Coroutine กวัก!

public class PlayerScript : MonoBehaviour
{
    public bool IsShovel;

    [Header("Level Settings")]
    [Tooltip("ใส่เลขด่านปัจจุบัน เช่น ด่าน 1 ใส่เลข 1 กวัก")]
    public int currentLevelIndex;

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
    private bool isWaitingForVictory = false; // ป้องกันการเรียก Coroutine ซ้ำกวัก

    void Start()
    {
        Time.timeScale = 1f;
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

    public void UpdateSeedCount() { if (seedCount != null) seedCount.text = "Seed : " + seed; }
    public void UseSeed() { if (seed > 0) { seed--; UpdateSeedCount(); } }

    public void DecreaseTree()
    {
        if (tree > 0)
        {
            tree -= 1;
            UpdateTreeCount();

            if (tree <= 0)
            {
                // ถ้าเป็นด่าน 3 และยังไม่ได้เริ่มรอ ให้เริ่มรอ 15 วิกวัก!
                if (currentLevelIndex == 3 && !isWaitingForVictory)
                {
                    StartCoroutine(WaitBeforeWin(15f));
                }
                else if (currentLevelIndex != 3)
                {
                    WinGame();
                }
            }
        }
    }

    public void UpdateTreeCount() { if (treeCount != null) treeCount.text = "Remaining : " + tree; }
    public void RefreshAllUI() { UpdateSeedCount(); UpdateTreeCount(); }

    // --- ฟังก์ชันพิเศษสำหรับด่านสุดท้ายกวัก ---
    IEnumerator WaitBeforeWin(float seconds)
    {
        isWaitingForVictory = true;
        Debug.Log("รดน้ำครบแล้ว! อีก " + seconds + " วินาทีจะจบเกมกวัก...");

        // (Option) ถ้านายมี SunSystem ในเป็ด นายอาจจะสั่งปิดเพื่อให้เป็ดอมตะช่วงนี้กวัก
        // GetComponent<SunSystem>().enabled = false; 

        yield return new WaitForSeconds(seconds);
        WinGame();
    }

    void WinGame()
    {
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);
        if (levelReached <= currentLevelIndex)
        {
            PlayerPrefs.SetInt("levelReached", currentLevelIndex + 1);
            PlayerPrefs.Save();
            Debug.Log("ปลดล็อกด่านถัดไปเรียบร้อยกวัก!");
        }

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }
    }
}