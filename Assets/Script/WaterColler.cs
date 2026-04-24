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
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = false;
            playerMovement.isInWater = false;
            timer = 0f;
        }
    }

    void Update()
    {
        if (isPlayerInside)
        {
            timer += Time.deltaTime;

            if (timer >= 1f)
            {
                // ไม่ว่าจะเป็นน้ำ Tag ไหน (WaterSource หรือ ContaminatedWater)
                // ก็จะลดความร้อน 1.0f เท่ากันหมดกวัก!
                if (overHeatBar != null)
                {
                    overHeatBar.slider.value -= 1f;
                    Debug.Log("แช่น้ำดับร้อน... เย็นเท่ากันทุกบ่อกวัก!");
                }
                timer = 0f;
            }
        }
    }
}