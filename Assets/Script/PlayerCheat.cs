using UnityEngine;

public class PlayerCheat : MonoBehaviour
{
    [Header("Cheat Settings")]
    [Tooltip("ใส่ความเร็วที่ต้องการให้วิ่งเร็วขึ้นตอนเปิดสูตรโกง")]
    public float cheatSpeedMultiplier = 2.5f; 

    private PlayerMovement playerMovement;
    private float originalSpeed;
    private bool isCheatActive = false; // ตัวแปรเช็คสถานะ เปิด/ปิด

    void Start()
    {
        // ค้นหาสคริปต์ PlayerMovement ในตัวละคร
        playerMovement = GetComponent<PlayerMovement>();

        if (playerMovement != null)
        {
            // บันทึกค่าความเร็วปกติเริ่มต้นเอาไว้
            originalSpeed = playerMovement.speed;
        }
        else
        {
            Debug.LogError("PlayerCheat: ไม่เจอสคริปต์ PlayerMovement ในออปเจกต์นี้กวัก!");
        }
    }

    void Update()
    {
        if (playerMovement == null) return;

        // เปลี่ยนมาใช้ GetKeyDown เพื่อตรวจจับการกดปุ่ม "ครั้งเดียว" (ไม่นับตอนกดค้าง)
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            // สลับสถานะจริง/เท็จ (ถ้า true จะกลายเป็น false / ถ้า false จะกลายเป็น true)
            isCheatActive = !isCheatActive;

            if (isCheatActive)
            {
                // เปิด Cheat: คูณความเร็ว
                playerMovement.speed = originalSpeed * cheatSpeedMultiplier;
                Debug.Log("💥 Cheat ON: เปิดโหมดวิ่งเร็วทะลุนรกกวัก!");
            }
            else
            {
                // ปิด Cheat: กลับมาความเร็วปกติ
                playerMovement.speed = originalSpeed;
                Debug.Log("🛑 Cheat OFF: กลับมาความเร็วปกติแล้วกวัก");
            }
        }
    }
}
