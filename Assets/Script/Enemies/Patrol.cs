using UnityEngine;

public class Patrol : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 3f;          // ความเร็วของศัตรู
    public float reachDistance = 0.1f; // ระยะที่ถือว่าเดินถึงจุดแล้ว

    [Header("Patrol Points")]
    public Transform[] patrolPoints;   // ลากจุดทั้ง 8 จุดมาใส่ในนี้

    private int currentPointIndex = 0; // ลำดับจุดปัจจุบันที่กำลังเดินไป

    void Update()
    {
        // 1. เช็คก่อนว่ามีจุดใน Array ไหมเพื่อกัน Error
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        // 2. หาตำแหน่งของจุดเป้าหมายปัจจุบัน
        Transform targetPoint = patrolPoints[currentPointIndex];

        // 3. สั่งให้ศัตรูเดินยิงตรงไปที่จุดเป้าหมาย
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // 4. เช็คว่าเดินไปถึงจุดเป้าหมายหรือยัง
        if (Vector2.Distance(transform.position, targetPoint.position) < reachDistance)
        {
            // เปลี่ยนไปจุดถัดไป (ถ้าถึงจุดสุดท้ายที่ 8 มันจะวนกลับมาจุดที่ 0 เอง)
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
    }
}
