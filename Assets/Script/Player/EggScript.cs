using UnityEngine;

public class EggScript : MonoBehaviour
{
    private SmartEnemyAI enemy;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemy = FindFirstObjectByType<SmartEnemyAI>();
            enemy.slowTimer += 15f;
            Destroy(gameObject);
        }
    }
}
