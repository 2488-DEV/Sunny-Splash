using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;

public class WaterColler : MonoBehaviour
{
    public OverHeatBar overHeatBar;
    public PlayerMovement playerMovement;

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
