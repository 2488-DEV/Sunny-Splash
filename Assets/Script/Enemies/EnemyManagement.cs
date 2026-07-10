using UnityEngine;

public class EnemyManagement : MonoBehaviour
{
    public float speed = 5f;
    public Transform target;
    public float stopDistance = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position,target.position) > stopDistance)
        transform.position = Vector2.MoveTowards(transform.position,target.position,speed * Time.deltaTime);        
    }
}
