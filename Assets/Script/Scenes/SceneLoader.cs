using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    // ตัวแปรส่วนกลางสำหรับจำชื่อฉากที่จะไป
    public static string NextSceneName;

    // ฟังก์ชันส่วนกลางที่ทุกปุ่มในเกมจะมาเรียกใช้
    public static void Load(string sceneName)
    {
        // 1. บันทึกชื่อฉากปลายทางเก็บไว้ในความจำ
        NextSceneName = sceneName;
        
        // 2. สั่งให้เกมเปิดฉากโหลดตรงกลางทันที (ต้องชื่อตรงกับใน Build Settings)
        SceneManager.LoadScene("Loading");
    }
}
