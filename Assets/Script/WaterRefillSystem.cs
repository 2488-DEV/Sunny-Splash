using UnityEngine;
using UnityEngine.UI;

public class WaterRefillSystem : MonoBehaviour
{
    [Header("Water Settings")]
    public float currentWater = 0f;
    public float maxWater = 100f;
    public Slider waterBar;

    [Header("Smoothness Settings")]
    public float smoothSpeed = 5f;

    private bool canRefill = false;

    void Start()
    {
        if (waterBar != null)
        {
            waterBar.maxValue = maxWater;
            waterBar.value = currentWater;
        }
    }

    void Update()
    {
        // ทำให้หลอดลื่นไหล (Smooth Slider)
        if (waterBar != null)
        {
            waterBar.value = Mathf.MoveTowards(waterBar.value, currentWater, smoothSpeed * 100f * Time.deltaTime);
        }

        // ระบบเติมน้ำด้วย Spacebar (Refill)
        if (canRefill && Input.GetKeyDown(KeyCode.Space) && !PlayerActionManager.Instance.IsPerformingAction)
        {
            StartRefilling();
        }
    }

    void StartRefilling()
    {
        if (currentWater >= maxWater) return;

        PlayerActionManager.Instance.TryStartAction(ActionType.RefillWater, () =>
        {
            currentWater = maxWater; // เติมน้ำเต็ม
            Debug.Log("เติมน้ำฉ่ำๆ เรียบร้อย! กวัก!");
        });
    }

    // --- ฟังก์ชันอัปเกรด: รดน้ำ 3 ทีหมดแบบเป๊ะๆ ---
    public void UseWaterForPlanting()
    {
        // หักออกทีละ 33.4 เพื่อให้รดครบ 3 ครั้งแล้วน้ำ "ติดลบ" หรือ "เป็น 0" แน่นอน
        // (33.4 * 3 = 100.2) จะไม่มีเศษน้ำเหลือไปรดต้นที่ 4 กวัก!
        currentWater = Mathf.Max(0, currentWater - 33.4f);

        Debug.Log("รดน้ำไปแล้ว! น้ำคงเหลือจริง: " + currentWater);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WaterSource")) canRefill = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("WaterSource")) canRefill = false;
    }
}