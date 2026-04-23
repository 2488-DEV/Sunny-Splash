using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public OverHeatBar overHeatBar;
    public bool isInDead = false;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInDead = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInDead = false;
        }
    }
}
