using UnityEngine;

public class ShadowColler : MonoBehaviour
{
    public OverHeatBar overHeatBar;

    public bool isPlayerInside = false;

    public float timer =0f;

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
                overHeatBar.timer =0f;
                timer = 0f;
            }
        }
    }
}
