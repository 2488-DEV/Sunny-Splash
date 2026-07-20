using UnityEngine;

public class Patrol : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 3f;          // ความเร็วของศัตรู
    public float reachDistance = 0.1f; // ระยะที่ถือว่าเดินถึงจุดแล้ว

    [Header("Patrol Points")]
    public Transform[] patrolPoints;   // ลากจุดทั้ง 8 จุดมาใส่ในนี้

    private int currentPointIndex = 0; // ลำดับจุดปัจจุบันที่กำลังเดินไป
    private SmartEnemyAI ai;

    void Start()
{
    ai = GetComponent<SmartEnemyAI>();
}

    void Update()
{
    if (ai != null && ai.currentState != SmartEnemyAI.State.Idle)
        return;

    if (patrolPoints == null || patrolPoints.Length == 0)
        return;

    Transform targetPoint = patrolPoints[currentPointIndex];

    float directionX = targetPoint.position.x - transform.position.x;
    ai.FaceDirection(directionX);

    transform.position = Vector2.MoveTowards(
    transform.position,
    targetPoint.position,
    speed * Time.deltaTime);
    if (Vector2.Distance(transform.position, targetPoint.position) < reachDistance)
    {
        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
    }
}
}
