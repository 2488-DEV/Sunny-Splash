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
        if (waterBar != null)
        {
            waterBar.value = Mathf.MoveTowards(waterBar.value, currentWater, smoothSpeed * 100f * Time.deltaTime);
        }

        // ระบบเติมน้ำ: เช็ค canRefill (ต้องยืนในบ่อ WaterSource เท่านั้นถึงจะตักได้กวัก!)
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
            currentWater = maxWater;
            Debug.Log("เติมน้ำสะอาดเรียบร้อย! กวัก!");
        });
    }

    public void UseWaterForPlanting()
    {
        currentWater = Mathf.Max(0, currentWater - 33.4f);
        Debug.Log("รดน้ำไปแล้ว! น้ำคงเหลือ: " + currentWater);
    }

    // --- ส่วนแก้ไขเพื่อลบ Error สีแดงกวัก! ---
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // เช็คเฉพาะ Tag ที่เรามีใน Tag Manager เท่านั้นกวัก
        if (collision.CompareTag("WaterSource"))
        {
            canRefill = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("WaterSource"))
        {
            canRefill = false;
        }
    }
}