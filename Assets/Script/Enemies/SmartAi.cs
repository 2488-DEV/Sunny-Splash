using System.Collections;
using UnityEngine;

public class SmartEnemyAIWithPatrol : MonoBehaviour
{
    // กำหนดสถานะ (States) ของ AI ตามเดฟล็อก
    public enum State { Idle, Patrolling, Pursuing, Attacking }
    [Header("AI State")]
    public State currentState = State.Idle;

    [Header("Movement & Ranges")]
    public float speed = 3.5f;
    public float patrolSpeed = 1.5f; // ความเร็วตอนเดินลาดตระเวน (มักจะเดินช้ากว่าตอนวิ่งไล่)
    public float targetRange = 6f;   // รัศมีเริ่มไล่ล่า
    public float attackRange = 1.2f; // รัศมีเข้าโจมตี
    public float tileSize = 1f;      // ขนาดของ Grid ไทล์ในเกม

    [Header("Patrol Settings")]
    public float patrolRadius = 4f;    // รัศมีสูงสุดที่จะสุ่มเดินห่างจากจุดเกิด
    public float waitAtWaypoint = 2f;  // เวลาที่จะยืนรอ (วินาที) เมื่อเดินไปถึงจุดพิกัดลาดตระเวนแล้ว
    private Vector2 homePosition;       // จุดเกิด/จุดตั้งหลักของศัตรู
    private Vector2 patrolTarget;       // จุดหมายที่กำลังจะเดินไปในเส้นทางลาดตระเวน
    private bool isWaiting = false;

    [Header("Layer Setup")]
    public LayerMask obstacleLayer;  // เลือก Layer กำแพงใน Inspector

    [Header("Wall Avoidance")]
    public float wallCheckDistance = 0.7f;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 targetDestination;
    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // บันทึกตำแหน่งจุดเกิดปัจจุบันไว้เป็นจุดศูนย์กลางของการลาดตระเวน
        homePosition = transform.position;
        patrolTarget = homePosition;

        // ค้นหาวัตถุที่ใส่ Tag ว่า Player
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        targetDestination = transform.position;

        // สั่งให้ระบบคำนวณไทล์ดักทางทำงานทุกๆ 0.2 วินาที
        StartCoroutine(TileLOSLogicLoop());
    }

    void FixedUpdate()
    {
        if (player == null) return;

        // ส่วนเช็คตรวจจับผู้เล่นข้ามสถานะ: ถ้าเจอผู้เล่นเมื่อไหร่ ให้ตัดเข้าสู่การไล่ล่าทันที (ยกเว้นตอนกำลังโจมตี)
        if (currentState != State.Pursuing && currentState != State.Attacking)
        {
            if (Vector2.Distance(transform.position, player.position) < targetRange && HasLineOfSight(player.position))
            {
                currentState = State.Pursuing;
                isWaiting = false; // ยกเลิกการยืนรอลาดตระเวนทันที
                StopCoroutine(PatrolWaitRoutine());
            }
        }

        // ระบบ State Machine
        switch (currentState)
        {
            case State.Idle:
                rb.linearVelocity = Vector2.zero; // หยุดนิ่ง
                
                // ถ้าไม่ได้กำลังยืนรอคูลดาวน์ ให้สุ่มจุดเดินลาดตระเวนใหม่
                if (!isWaiting)
                {
                    currentState = State.Patrolling;
                }
                break;

            case State.Patrolling:
                // คำนวณทิศทางเดินไปยังจุดสุ่มลาดตระเวน
                Vector2 patrolDir = (patrolTarget - (Vector2)transform.position).normalized;
                patrolDir = AvoidWall(patrolDir);
                rb.linearVelocity = patrolDir * patrolSpeed;

                // ถ้าเดินไปใกล้ถึงจุดสุ่มนั้นแล้ว ให้สั่งหยุดรอสักพักก่อนสุ่มใหม่
                if (Vector2.Distance(transform.position, patrolTarget) < 0.2f)
                {
                    StartCoroutine(PatrolWaitRoutine());
                }
                break;

            case State.Pursuing:
                // เคลื่อนที่ไปหาจุดหมายไล่ล่า (ตัวผู้เล่น หรือ ไทล์ที่คำนวณได้)
                Vector2 direction = (targetDestination - (Vector2)transform.position).normalized;
                direction = AvoidWall(direction);
                rb.linearVelocity = direction * speed;

                float distanceToPlayer = Vector2.Distance(transform.position, player.position);

                // เงื่อนไข: เข้าใกล้ระยะโจมตี + สายตาเคลียร์มองเห็นผู้เล่นโดยตรง -> โจมตี
                if (distanceToPlayer <= attackRange && HasLineOfSight(player.position))
                {
                    currentState = State.Attacking;
                }
                // เงื่อนไข: ถ้าเดินไปถึงจุดหมายล่าสุดแล้วแต่ยังไม่เจอตัวผู้เล่นสักที -> เลิกตาม กลับไปจุดเริ่มต้น (Idle)
                else if (Vector2.Distance(transform.position, targetDestination) < 0.3f && !HasLineOfSight(player.position))
                {
                    currentState = State.Idle;
                    patrolTarget = homePosition; // เดินกลับมาตั้งหลักที่จุดเกิด
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

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, distance, obstacleLayer);
        return hit.collider == null;
    }

    // ระบบหน่วงเวลาตอนเดินไปถึงจุดลาดตระเวนแล้วยืนเอ๋อพักนึง ก่อนสุ่มจุดถัดไป
    IEnumerator PatrolWaitRoutine()
    {
        isWaiting = true;
        currentState = State.Idle;
        
        yield return new WaitForSeconds(waitAtWaypoint);

        // สุ่มหาพิกัดใหม่รอบๆ จุดเกิด (Home Position)
        Vector2 randomDirection = Random.insideUnitCircle.normalized * Random.Range(1f, patrolRadius);
        Vector2 potentialTarget = homePosition + randomDirection;

        // เช็คก่อนว่าจุดที่จะสุ่มเดินไป มีกำแพงขวางอยู่ไหม ถ้าไม่มีค่อยเดินไป (ป้อนกัน AI เดินติดกำแพงตอนเดินเล่น)
        if (HasLineOfSight(potentialTarget))
        {
            patrolTarget = potentialTarget;
        }
        else
        {
            patrolTarget = homePosition; // ถ้าจุดสุ่มติดกำแพง ให้เดินกลับมาจุดเกิดแทน
        }

        isWaiting = false;
    }

    // ลูปคำนวณหาไทล์ดักทางเมื่อผู้เล่นเดินหลบมุมตึก
    IEnumerator TileLOSLogicLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

            if (player == null || currentState != State.Pursuing) continue;

            if (HasLineOfSight(player.position))
            {
                targetDestination = player.position;
                continue;
            }

            Vector2 bestTile = targetDestination;
            bool foundSmartPath = false;

            Vector2[] checkOffsets = new Vector2[]
            {
                new Vector2(0,0),   new Vector2(1,0),  new Vector2(-1,0), 
                new Vector2(0,1),   new Vector2(0,-1), new Vector2(1,1), 
                new Vector2(-1,1),  new Vector2(1,-1), new Vector2(-1,-1)
            };

            foreach (Vector2 offset in checkOffsets)
            {
                Vector2 tilePos = (Vector2)player.position + (offset * tileSize);

                if (HasLineOfSight(tilePos))
                {
                    Vector2 tileToPlayerDir = (Vector2)player.position - tilePos;
                    RaycastHit2D tileHit = Physics2D.Raycast(tilePos, tileToPlayerDir.normalized, tileToPlayerDir.magnitude, obstacleLayer);

                    if (tileHit.collider == null)
                    {
                        bestTile = tilePos;
                        foundSmartPath = true;
                        break;
                    }
                }
            }

            if (foundSmartPath)
            {
                targetDestination = bestTile;
            }
        }
    }

    // ฟังก์ชันการโจมตี
    IEnumerator PerformAttackRoutine()
    {
        isAttacking = true;
        Debug.Log("ศัตรูใช้ท่าพุ่งชน / โจมตีผู้เล่น!");

        yield return new WaitForSeconds(1.0f);

        isAttacking = false;
        currentState = State.Pursuing;
    }

    // วาดขอบเขตวงรัศมีในหน้า Scene
    void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            Gizmos.color = HasLineOfSight(player.position) ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position, player.position);
        }
        
        // วงกลมสีเหลือง = ระยะสายตาเริ่มไล่ล่า
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, targetRange);

        // วงกลมสีฟ้า = ขอบเขตพื้นที่ที่ศัตรูจะเดินลาดตระเวนรอบจุดเกิด
        Gizmos.color = Color.cyan;
        if (Application.isPlaying)
            Gizmos.DrawWireSphere(homePosition, patrolRadius);
        else
            Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }

    Vector2 AvoidWall(Vector2 moveDir)
    {
        // เช็คข้างหน้า
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            moveDir,
            wallCheckDistance,
            obstacleLayer);

        if (!hit.collider)
            return moveDir;

        // หมุนซ้าย 90
        Vector2 left = new Vector2(-moveDir.y, moveDir.x);

        // หมุนขวา 90
        Vector2 right = new Vector2(moveDir.y, -moveDir.x);

        bool leftBlocked = Physics2D.Raycast(
            transform.position,
            left,
            wallCheckDistance,
            obstacleLayer);

        bool rightBlocked = Physics2D.Raycast(
            transform.position,
            right,
            wallCheckDistance,
            obstacleLayer);

        if (!leftBlocked)
            return left;

        if (!rightBlocked)
            return right;

        // ถ้าตันหมด
        return -moveDir;
    }
}
