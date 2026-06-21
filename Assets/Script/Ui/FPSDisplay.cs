using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    private float deltaTime = 0.0f;
    private float currentFPS = 0.0f;

    [Header("Settings")]
    [SerializeField] private float updateInterval = 0.2f; // อัปเดตตัวเลขทุกๆ 0.2 วินาที (ช่วยให้สมูท)
    private float nextUpdateTime = 0.0f;

    void Update()
    {
        // คำนวณหาค่า Delta Time แบบสะสมความสมูท
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        // ตรวจสอบรอบเวลาการอัปเดตตัวเลขเพื่อไม่ให้ตัวเลขวิ่งไวเกินไป
        if (Time.unscaledTime >= nextUpdateTime)
        {
            currentFPS = 1.0f / deltaTime;
            nextUpdateTime = Time.unscaledTime + updateInterval;
        }
    }

    void OnGUI()
    {
        // กำหนดขนาดและสไตล์ของตัวอักษรบนหน้าจอ
        int w = Screen.width, h = Screen.height;
        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(20, 20, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 3 / 100; // ปรับขนาดตามความละเอียดหน้าจอ
        style.normal.textColor = Color.red; // เปลี่ยนสีตัวอักษรได้ตรงนี้

        // แสดงผลตัวเลข FPS
        string text = string.Format("{0:0.} FPS", currentFPS);
        GUI.Label(rect, text, style);
    }
}
