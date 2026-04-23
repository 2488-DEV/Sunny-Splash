using UnityEngine;

public class WaterColler : MonoBehaviour
{
    public OverHeatBar overHeatBar;
    public PlayerMovement playerMovement;
    public PlayerScript playerScript;

    private float timer = 0f;
    private bool isPlayerInside = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = true;
            playerMovement.isInWater = true;

            // --- ลบส่วนที่เติมน้ำ +30 ออกแล้วกวัก ---
            // ตอนนี้เดินเข้าบ่อจะไม่มีน้ำเพิ่มเองแล้ว ต้องกดดูดเอาเท่านั้น
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = false;
            playerMovement.isInWater = false;
        }
    }

    void Update()
    {
        if (isPlayerInside)
        {
            timer += Time.deltaTime;

            if (timer >= 1f)
            {
                // ลดความร้อนเมื่อแช่น้ำ (Cooling System)
                if (overHeatBar != null && overHeatBar.slider != null)
                {
                    overHeatBar.slider.value -= 1f;
                }
                timer = 0f;
            }
        }
    }
}