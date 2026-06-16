using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Button[] levelButtons; // ลากปุ่ม Level 1, 2, 3 มาใส่ตามลำดับกวัก

    void Start()
    {
        // ดึงข้อมูลว่าถึงด่านไหนแล้ว (ถ้ายังไม่เคยเล่นจะให้ค่าเริ่มต้นเป็น 1)
        int reachedLevel = PlayerPrefs.GetInt("ReachedLevel", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            // ถ้าลำดับของด่าน (i+1) มากกว่าด่านที่ปลดล็อก ให้ปิดปุ่มกวัก
            if (i + 1 > reachedLevel)
            {
                levelButtons[i].interactable = false;
                // นายอาจจะแถมโค้ดเปลี่ยนสีปุ่มให้ดูมืดๆ หรือใส่ไอคอนกุญแจตรงนี้ได้กวัก
            }
            else
            {
                levelButtons[i].interactable = true;
            }
        }
    }

    // ฟังก์ชันสำหรับ Reset ข้อมูล (เอาไว้เทสตอนพัฒนาเกมกวัก)
    public void ResetLevels()
    {
        PlayerPrefs.DeleteAll();
    }
}