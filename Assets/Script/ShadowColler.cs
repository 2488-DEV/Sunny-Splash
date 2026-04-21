using UnityEngine;

public class ShadowColler : MonoBehaviour
{
    public OverHeatBar overHeatBar;
    public WaterColler waterColler;

    public bool isPlayerInside = false;

    public float timer =0f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = true;
            collision.GetComponent<PlayerMovement>().isInShadow = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = false;
            collision.GetComponent<PlayerMovement>().isInShadow = false;
        }
    }
}
