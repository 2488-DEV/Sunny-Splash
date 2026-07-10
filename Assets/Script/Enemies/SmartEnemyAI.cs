using System.Collections;
using UnityEngine;

public class SmartEnemyAI : MonoBehaviour
{
    // กำหนดสถานะ (States) ของ AI ตามเดฟล็อก
    public enum State { Idle, Pursuing, Attacking }
    [Header("AI State")]
    public State currentState = State.Idle;

    [Header("Movement & Ranges")]
    public float speed = 3.5f;
    public float targetRange = 6f;   // รัศมีเริ่มไล่ล่า
    public float attackRange = 1.2f; // รัศมีเข้าโจมตี
    public float tileSize = 1f;      // ขนาดของ Grid ไทล์ในเกม

    [Header("Layer Setup")]
    public LayerMask obstacleLayer;  // เลือก Layer กำแพงใน Inspector

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 targetDestination;
    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // ค้นหาวัตถุที่ใส่ Tag ว่า Player
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        targetDestination = transform.position;

        // สั่งให้ระบบคำนวณไทล์ดักทางทำงานทุกๆ 0.2 วินาที (ไม่รันทุกเฟรมเพื่อประหยัด RAM)
        StartCoroutine(TileLOSLogicLoop());
    }

    void FixedUpdate()
    {
        if (player == null) return;

        switch (currentState)
        {
            case State.Idle:
                rb.linearVelocity = Vector2.zero; // หยุดนิ่ง (Unity 2023+ ใช้ linearVelocity แทน velocity)
                
                // ถ้าผู้เล่นเดินเข้ามาในระยะตรวจจับ ให้เริ่มไล่ล่า
                if (Vector2.Distance(transform.position, player.position) < targetRange)
                {
                    currentState = State.Pursuing;
                }
                break;

            case State.Pursuing:
                // เคลื่อนที่ไปหาจุดหมาย (ตัวผู้เล่น หรือ ไทล์ที่คำนวณได้)
                Vector2 direction = (targetDestination - (Vector2)transform.position).normalized;
                rb.linearVelocity = direction * speed;

                float distanceToPlayer = Vector2.Distance(transform.position, player.position);

                // เงื่อนไข: เข้าใกล้ระยะโจมตี + สายตาเคลียร์มองเห็นผู้เล่นโดยตรง -> โจมตี
                if (distanceToPlayer <= attackRange && HasLineOfSight(player.position))
                {
                    currentState = State.Attacking;
                }
                // เงื่อนไข: ถ้าเดินไปถึงจุดหมายล่าสุดแล้วแต่ยังไม่เจอตัวผู้เล่นสักที -> เลิกตาม กลับไป Idle
                else if (Vector2.Distance(transform.position, targetDestination) < 0.3f && !HasLineOfSight(player.position))
                {
                    currentState = State.Idle;
                }
                break;

            case State.Attacking:
                rb.linearVelocity = Vector2.zero;
                if (!isAttacking)
                {
                    StartCoroutine(PerformAttackRoutine());
                }
                break;
        }
    }

    // ฟังก์ชันยิง Raycast 2D เพื่อตรวจสอบสิ่งกีดขวาง (Line of Sight)
    bool HasLineOfSight(Vector2 targetPos)
    {
        Vector2 direction = targetPos - (Vector2)transform.position;
        float distance = direction.magnitude;

        // ยิงเลเซอร์สแกนหาเฉพาะวัตถุที่อยู่ในกลุ่ม obstacleLayer
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, distance, obstacleLayer);

        // ถ้าชนสิ่งกีดขวาง แปลว่าไม่มีสายตา (LOS False) แต่ถ้าไม่ชนอะไรเลย แปลว่ามองเห็นเคลียร์ (LOS True)
        return hit.collider == null;
    }

    // ลูปคำนวณหาไทล์ดักทางเมื่อผู้เล่นเดินหลบมุมตึก
    IEnumerator TileLOSLogicLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f); // หน่วงเวลา 0.2 วินาทีตามคลิป

            if (player == null || currentState != State.Pursuing) continue;

            // 1. ถ้าศัตรูยังมองเห็นตัวผู้เล่นตรงๆ -> วิ่งล็อกเป้าไปที่ตัวผู้เล่นเลย
            if (HasLineOfSight(player.position))
            {
                targetDestination = player.position;
                continue;
            }

            // 2. ถ้าหลุดสายตา (LOS False): ค้นหาไทล์ดักทางรอบๆ ตัวผู้เล่น
            Vector2 bestTile = targetDestination;
            bool foundSmartPath = false;

            // จำลอง Grid 3x3 รอบตัวผู้เล่น
            Vector2[] checkOffsets = new Vector2[]
            {
                new Vector2(0,0),   new Vector2(1,0),  new Vector2(-1,0), 
                new Vector2(0,1),   new Vector2(0,-1), new Vector2(1,1), 
                new Vector2(-1,1),  new Vector2(1,-1), new Vector2(-1,-1)
            };

            foreach (Vector2 offset in checkOffsets)
            {
                Vector2 tilePos = (Vector2)player.position + (offset * tileSize);

                // เช็คเงื่อนไขตามคลิป: ศัตรูต้องมองเห็นไทล์นั้นเคลียร์
                if (HasLineOfSight(tilePos))
                {
                    // และไทล์นั้นก็ต้องมีเส้นสายตามองเห็นตัวผู้เล่นด้วย (ยิง Raycast จำลองจากไทล์ไปหาผู้เล่น)
                    Vector2 tileToPlayerDir = (Vector2)player.position - tilePos;
                    RaycastHit2D tileHit = Physics2D.Raycast(tilePos, tileToPlayerDir.normalized, tileToPlayerDir.magnitude, obstacleLayer);

                    if (tileHit.collider == null) // ไทล์นี้มองเห็นผู้เล่นเหมือนกัน!
                    {
                        bestTile = tilePos;
                        foundSmartPath = true;
                        break; // เจอจุดตัดมุมดักทางที่เคลียร์ที่สุดแล้ว ให้หยุดวนลูปทันที
                    }
                }
            }

            if (foundSmartPath)
            {
                targetDestination = bestTile; // อัปเดตจุดหมายให้ศัตรูเดินไปดักทางที่หัวมุม
            }
        }
    }

    // ฟังก์ชันการโจมตี
    IEnumerator PerformAttackRoutine()
    {
        isAttacking = true;
        Debug.Log("ศัตรูใช้ท่าพุ่งชน / โจมตีผู้เล่น!");

        // ตรงนี้สามารถใส่โค้ดเปิดเปิดกล่อง Hitbox หรือสั่งเล่นอนิเมชันโจมตีได้
        yield return new WaitForSeconds(1.0f); // คูลดาวน์สถานะโจมตี 1 วินาที

        isAttacking = false;
        currentState = State.Pursuing; // โจมตีเสร็จกลับไปสถานะไล่ล่าต่อ
    }

    // วาดเส้นสีในหน้า Scene เพื่อให้ง่ายต่อการดีบั๊กเช็คสายตาของศัตรู
    void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            Gizmos.color = HasLineOfSight(player.position) ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position, player.position);
        }
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, targetRange);
    }
    // แปะฟังก์ชันนี้ไว้ข้างในคลาส SmartEnemyAI
    // เรียกใช้เมื่อมีเสียงเกิดขึ้น
    public void ListenToSound(Vector2 soundSourcePosition)
    {
        // ถ้าไม่ได้กำลังโจมตี ให้หันไปตรวจสอบเสียง
        if (currentState != State.Attacking)
        {
            targetDestination = soundSourcePosition;
            currentState = State.Pursuing;
        }
    }
}
