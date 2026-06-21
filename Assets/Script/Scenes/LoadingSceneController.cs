using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingSceneController : MonoBehaviour
{
    [Header("UI Elements")]
    public Image LoadingBarFill; // ลากรูปแถบโหลดมาใส่ในช่องนี้
    public TMP_Text LoadingText;  // ลาก TextMeshPro ที่จะแสดงเลข % มาใส่

    void Start()
    {
        // ทันทีที่ฉากเปิดขึ้นมา ให้เริ่มแอบโหลดฉากหลังบ้าน
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        // ดึงชื่อฉากปลายทางที่ฝากไว้ใน SceneLoader ออกมาสั่งโหลดแบบเบื้องหลัง (Async)
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneLoader.NextSceneName);

        // วนลูปตราบใดที่ฉากถัดไปยังโหลดไม่เสร็จ
        while (!operation.isDone)
        {
            // operation.progress ของ Unity จะวิ่งจาก 0 ถึง 0.9 เท่านั้นเมื่อโหลดเสร็จ 
            // เราจึงต้องหารด้วย 0.9f เพื่อปัดตัวเลขให้กลายเป็น 0 ถึง 1 เต็มหลอดพอดี
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            
            if (LoadingBarFill != null)
            {
                LoadingBarFill.fillAmount = progressValue;
            }
            
            if (LoadingText != null)
            {
                float percent = progressValue * 100f;
                LoadingText.text = string.Format("{0:0}%", percent); // แสดงผลเป็นเลขจำนวนเต็ม เช่น 50%
            }

            yield return null; // รอเฟรมถัดไปแล้วเช็คใหม่
        }
    }
}
