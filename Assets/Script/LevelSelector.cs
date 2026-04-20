using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections; // จำเป็นต้องมีเพื่อใช้ IEnumerator

public class LevelSelector : MonoBehaviour
{
    [Header("Level Buttons")]
    public Button[] levelButtons;

    [Header("Lock Icons")]
    public GameObject[] lockIcons;

    private bool isLoading = false; // ป้องกันการกดซ้ำ

    void OnEnable()
    {
        isLoading = false; // รีเซ็ตสถานะเมื่อเปิดหน้าต่างขึ้นมา
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > levelReached)
            {
                levelButtons[i].interactable = false;
                if (i > 0 && (i - 1) < lockIcons.Length)
                {
                    lockIcons[i - 1].SetActive(true);
                }
            }
            else
            {
                levelButtons[i].interactable = true;
                if (i > 0 && (i - 1) < lockIcons.Length)
                {
                    lockIcons[i - 1].SetActive(false);
                }
            }
        }
    }

    // ฟังก์ชันหลักที่ปุ่มจะเรียกใช้
    public void LoadLevel(string sceneName)
    {
        if (!isLoading) // ถ้ายังไม่ได้อยู่ในระหว่างโหลด
        {
            StartCoroutine(DelayLoad(sceneName));
        }
    }

    IEnumerator DelayLoad(string sceneName)
    {
        isLoading = true; // ล็อกไม่ให้กดปุ่มอื่นได้อีก

        // รอ 1 วินาที
        yield return new WaitForSeconds(1f);

        // ย้ายฉากไปยังชื่อที่ตั้งไว้
        SceneManager.LoadScene(sceneName);
    }
}