using UnityEngine;

public class DestroyBlock : MonoBehaviour
{
    public GameObject Block;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(Block);
        }
    }
}
