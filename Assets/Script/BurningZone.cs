using UnityEngine;

public class BurningZone : MonoBehaviour
{
    public OverHeatBar overHeatBar;
    public bool inBuring = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inBuring = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inBuring = false;
        }
    }
}
