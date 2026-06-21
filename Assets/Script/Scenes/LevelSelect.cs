using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelSelect : MonoBehaviour
{
    [Header("Level Buttons")]
    public Button[] levelButtons; // ลากปุ่ม Level 1, 2, 3, 4 มาใส่ตามลำดับ

    [Header("Lock Icons")]
    [Tooltip("Element 0 = กุญแจด่าน 2, Element 1 = กุญแจด่าน 3, Element 2 = กุญแจด่าน 4")]
    public GameObject[] lockIcons; // มี 3 อันสำหรับด่าน 2, 3, 4

    private const string LEVEL_PREFS_KEY = "levelReached"; // ตั้งชื่อคีย์กลางให้ตรงกันหมด

    void OnEnable()
    {
        RefreshUI();
    }

    // ฟังก์ชันสำหรับคำนวณหน้าจอ UI ใหม่
    public void RefreshUI()
    {
        // ดึงค่าการปลดล็อกด่าน (ถ้ายังไม่เคยเล่นจะคืนค่าเป็นด่าน 1 เสมอ)
        int levelReached = PlayerPrefs.GetInt(LEVEL_PREFS_KEY, 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int currentButtonLevel = i + 1; // ด่านของปุ่มนี้ (เช่น i=0 คือด่าน 1)

            if (currentButtonLevel > levelReached)
            {
                // ---- กรณีด่านยังไม่ปลดล็อก ----
                levelButtons[i].interactable = false;

                // เปิดสัญลักษณ์กุญแจ (ด่าน 2 อ้างอิง index ที่ 0 ของอาร์เรย์กุญแจ)
                int lockIndex = i - 1;
                if (lockIndex >= 0 && lockIndex < lockIcons.Length && lockIcons[lockIndex] != null)
                {
                    lockIcons[lockIndex].SetActive(true);
                }
            }
            else
            {
                // ---- กรณีด่านปลดล็อกแล้ว ----
                levelButtons[i].interactable = true;

                // ซ่อนสัญลักษณ์กุญแจ
                int lockIndex = i - 1;
                if (lockIndex >= 0 && lockIndex < lockIcons.Length && lockIcons[lockIndex] != null)
                {
                    lockIcons[lockIndex].SetActive(false);
                }
            }
        }
    }

    // ฟังก์ชันสั่งโหลดฉากผ่านระบบฉากโหลดตรงกลาง (ไม่ต้องดีเลย์ 1 วิแล้ว เพราะฉากโหลดทำหน้าที่นั้นแทน)
    public void SelectLevel(string sceneName)
    {
        SceneLoader.Load(sceneName);
    }

    // --- ฟังก์ชัน Cheat สำหรับปุ่มล่องหน เอาไว้ทดสอบเกม ---
    public void CheatToggleLevels()
    {
        int currentLevelReached = PlayerPrefs.GetInt(LEVEL_PREFS_KEY, 1);
        int maxLevels = levelButtons.Length; 
        int nextLevel = currentLevelReached + 1;

        if (nextLevel > maxLevels)  
        {
            PlayerPrefs.SetInt(LEVEL_PREFS_KEY, 1);
            Debug.Log("Cheat: รีเซ็ตกลับไปล็อกเหลือแค่ด่าน 1!");
        }
        else
        {
            PlayerPrefs.SetInt(LEVEL_PREFS_KEY, nextLevel);
            Debug.Log("Cheat: ปลดล็อกถึงด่าน " + nextLevel + " แล้ว!");
        }

        PlayerPrefs.Save();
        RefreshUI(); // อัปเดตหน้าจอทันที
    }

    // --- ฟังก์ชันเอาไว้กดล้างเซ็ตติ้งตอนทดสอบเกมใน Unity Editor ---
    [ContextMenu("Reset Level Progression")]
    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey(LEVEL_PREFS_KEY);
        PlayerPrefs.Save();
        Debug.Log("ล้างข้อมูลการเล่นเรียบร้อย!");
        RefreshUI();
    }
}
