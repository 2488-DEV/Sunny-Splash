using UnityEngine;

public class EggScript : MonoBehaviour
{
    private SmartEnemyAI enemy;
    private Rigidbody2D rb;

    [SerializeField] private float speed = 20f;
    
    private Vector3 targetPoint; // ใช้ตัวแปรพิกัดนิ่งตัวเดียวเลย
    private bool hasTargetPoint = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.angularVelocity = -500f; // ให้ไข่หมุนติ้วๆ
        }
    }

    // โหมด A: ปรับใหม่! รับ Transform เข้ามา แต่แปลงเป็นพิกัด Vector3 ณ วินาทีนั้นทันที
    public void SetTargetPoint(Transform target)
    {
        if (target != null)
        {
            targetPoint = target.position; // <--- ล็อกพิกัด ณ วินาทีที่กด! ศัตรูขยับหลังจากนี้ ไข่ไม่สนใจแล้ว
            targetPoint.z = 0;
            hasTargetPoint = true;
        }
    }

    // โหมด B: ล็อกพิกัดเมาส์นิ่งๆ (เหมือนเดิม)
    public void SetTargetPoint(Vector3 point)
    {
        targetPoint = point;
        targetPoint.z = 0;
        hasTargetPoint = true;
    }

    void FixedUpdate()
    {
        if (rb == null) return;
    
        // เหลือโหมดเดียวคือวิ่งไปที่พิกัดนิ่ง (ไม่ว่าจะมาจากพิกัดเมาส์ หรือพิกัดศัตรูตอนกด)
        if (hasTargetPoint)
        {
            Vector2 direction = (Vector2)targetPoint - rb.position;
            float distance = direction.magnitude;
    
            // บินถึงพิกัดเป้าหมาย (ที่ล็อกไว้) แล้วค่อยระเบิด
            if (distance <= 0.2f)
            {
                Explode();
                return;
            }
    
            direction.Normalize();
            rb.velocity = direction * speed;
        }
    }

    void Explode()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemy = collision.GetComponent<SmartEnemyAI>();
            if (enemy != null)
            {
                enemy.slowTimer += 15f;
            }
            Destroy(gameObject);
        }
        
        if (collision.CompareTag("Border"))
        {
            Destroy(gameObject);
        }
    }
}