using UnityEngine;

public class EggLockedArea : MonoBehaviour
{
    // เปลี่ยนมาใช้ LayerMask สำหรับระบบ 2D
    [SerializeField] private LayerMask targetLayer;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 1. แปลงตำแหน่งเมาส์บนจอ ให้เป็นพิกัดในโลกเกม 2D (World Space)
            Vector2 mousePos2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 2. ยิง Raycast 2D ตรงตำแหน่งเมาส์จิ้มแบบล็อกเลเยอร์
            // (ระบบ 2D จะเช็กจุดที่เมาส์จิ้มลงไปตรงๆ เลย)
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, Mathf.Infinity, targetLayer);

            // 3. เช็กว่าเจออะไรไหม
            if (hit.collider != null)
            {
                // พ่น Log ออกมาดูว่าชนวัตถุชื่ออะไร Tag อะไร
                Debug.Log($"[2D Hit] ชนวัตถุชื่อ: {hit.collider.name} | Tag: {hit.collider.tag}");

                if (hit.collider.CompareTag("Enemy"))
                {
                    Debug.Log("[SUCCESS] 2D Player เจอ Enemy Area แล้ว!");
                    // ยัด Action ตรงนี้เลย
                }
            }
            else
            {
                Debug.Log("[Failed 2D] เมาส์จิ้มลงไปในความว่างเปล่า ไม่โดน Collider 2D ตัวไหนใน Layer ที่กำหนดเลย");
            }
        }
    }
}