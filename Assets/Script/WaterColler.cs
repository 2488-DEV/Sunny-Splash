using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;

public class WaterColler : MonoBehaviour
{
    public OverHeatBar overHeatBar;
    private bool isPlayerInside = false;
    private float timer = 0f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }

    void Update()
    {
        if (isPlayerInside)
        {

            timer += Time.deltaTime;

            if (timer >= 1f)
            {
                overHeatBar.slider.value -= 2f; // ลดทีละ 2
                timer = 0f;
            }
        }
    }
}
