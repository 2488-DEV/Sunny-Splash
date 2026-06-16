using UnityEngine;

public class ForCheat : MonoBehaviour
{
    public GameObject[] TestToggleLevel; 
    
    // ตัวแปรเก็บสถานะ (เริ่มจาก 0)
    private int currentState = 0;

    void Start()
    {
        // เริ่มเกมมาให้เปิดหรือปิดตามค่าเริ่มต้น
        UpdateToggleTestBotton();
    }

    public void ToggleTestBotton()
    {
        if (currentState == 0)
        {
            currentState = 1;
        }
        else
        {
            currentState = 0;
        }

        // อัปเดตการเปิด-ปิด GameObject
        UpdateToggleTestBotton();
    }
    void UpdateToggleTestBotton()
    {
        // ถ้าเป็น 1 จะได้ true (เปิด) ถ้าเป็น 0 จะได้ false (ปิด)
        bool isActive = (currentState == 1);

        foreach (GameObject obj in TestToggleLevel)
        {
            if (obj != null)
            {
                obj.SetActive(isActive);
            }

    }
    }
}
