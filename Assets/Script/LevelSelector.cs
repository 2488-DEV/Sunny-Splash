using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelSelector : MonoBehaviour
{
    [Header("Level Buttons")]
    public Button[] levelButtons;

    [Header("Lock Icons")]
    public GameObject[] lockIcons; // Element 0 คือกุญแจด่าน 2, Element 1 คือกุญแจด่าน 3

    private bool isLoading = false;

    void OnEnable()
    {
        isLoading = false;

        // ดึงค่าการปลดล็อก (ถ้ายังไม่เคยเล่นจะคืนค่า 1 กวัก)
        // **สำคัญ:** ชื่อตัวแปร "levelReached" ต้องตรงกับที่เขียนใน PlayerScript นะนาย!
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            // ด่านปัจจุบัน (เช่น i=0 คือด่าน 1)
            int currentButtonLevel = i + 1;

            if (currentButtonLevel > levelReached)
            {
                levelButtons[i].interactable = false;
                // ถ้ามีกุญแจสำหรับด่านนี้ (ด่าน 2 ใช้กุญแจใบที่ 0) ให้เปิดกุญแจกวัก
                if (i > 0 && (i - 1) < lockIcons.Length && lockIcons[i - 1] != null)
                {
                    lockIcons[i - 1].SetActive(true);
                }
            }
            else
            {
                levelButtons[i].interactable = true;
                // ถ้าปลดล็อกแล้ว ให้ซ่อนกุญแจกวัก
                if (i > 0 && (i - 1) < lockIcons.Length && lockIcons[i - 1] != null)
                {
                    lockIcons[i - 1].SetActive(false);
                }
            }
        }
    }

    public void LoadLevel(string sceneName)
    {
        if (!isLoading)
        {
            StartCoroutine(DelayLoad(sceneName));
        }
    }

    IEnumerator DelayLoad(string sceneName)
    {
        isLoading = true;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }

    // --- แถม: ฟังก์ชันเอาไว้กดล้างเซ็ตติ้งตอนทดสอบเกมกวัก! ---
    [ContextMenu("Reset Level Progression")]
    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey("levelReached");
        Debug.Log("ล้างข้อมูลการเล่นเรียบร้อยกวัก!");
    }

    // --- ฟังก์ชัน Cheat สำหรับปุ่มล่องหน เอาไว้ทดสอบเกมกวัก! ---
    public void CheatToggleLevels()
    {
    // 1. ดึงค่าด่านปัจจุบันจาก PlayerPrefs (ถ้าไม่มีให้เริ่มที่ 1)
    int currentLevelReached = PlayerPrefs.GetInt("levelReached", 1);
    
    // n คือจำนวนด่านทั้งหมดในเกม (นับจากจำนวนปุ่มที่ลากใส่ไว้)
    int maxLevels = levelButtons.Length; 

    // 2. คำนวณด่านถัดไป
    int nextLevel = currentLevelReached + 1;

    // 3. ถ้าค่าด่านถัดไปเกินจำนวนด่านทั้งหมด (กดเกิน n ครั้ง) ให้วนกลับไปล็อกเหลือแค่ด่าน 1
    if (nextLevel > maxLevels)
    {
        PlayerPrefs.SetInt("levelReached", 1);
        Debug.Log("Cheat: รีเซ็ตกลับไปล็อกเหลือแค่ด่าน 1 กวัก!");
    }
    else
    {
        // ปลดล็อกด่านถัดไปเรื่อยๆ
        PlayerPrefs.SetInt("levelReached", nextLevel);
        Debug.Log("Cheat: ปลดล็อกถึงด่าน " + nextLevel + " แล้วกวัก!");
    }

    // 4. บันทึกข้อมูลลงเครื่องทันที
    PlayerPrefs.Save();

    // 5. สั่งให้โค้ดคำนวณหน้าจอ UI ใหม่ทันทีโดยไม่ต้องเปิด-ปิดหน้าต่างใหม่
    OnEnable(); 
    }

}