using UnityEngine;

public class ShadowColler : MonoBehaviour
{
    // ลบตัวแปรที่ไม่ได้ใช้ออกเพื่อให้ Inspector สะอาดครับ
    public bool isPlayerInside = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var movement = collision.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                isPlayerInside = true;
                movement.isInShadow = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var movement = collision.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                isPlayerInside = false;
                movement.isInShadow = false;
            }
        }
    }

    // --- ส่วนที่อัปเกรด: กันบั๊กตอนวัตถุถูกทำลายหรือปิดการใช้งาน ---
    private void OnDisable()
    {
        // ถ้าอยู่ดีๆ เมฆหายไป หรือบ่อน้ำถูกปิดกลางคัน ให้คืนค่า Player ด้วย
        if (isPlayerInside)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                var movement = player.GetComponent<PlayerMovement>();
                if (movement != null) movement.isInShadow = false;
            }
            isPlayerInside = false;
        }
    }
}