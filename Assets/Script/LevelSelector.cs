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
}