using UnityEngine;
using TMPro;

public class ShovelScript : MonoBehaviour
{
    public bool IsInRange;
    private PlayerScript player;
    private SeedScript seed;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<PlayerScript>();
        }
        seed = FindFirstObjectByType<SeedScript>();
    }

    void Update()
    {   
        if (player.IsShovel)
        {   
            if (player.isLeft)
            {
                transform.position = new Vector3(player.transform.position.x - 0.8f , player.transform.position.y - 0.475f , player.transform.position.z);
                transform.rotation = Quaternion.Euler(0, 0, -19f);
            }
            else if (player.isRight)
            {
                transform.position = new Vector3(player.transform.position.x + 0.8f , player.transform.position.y - 0.475f , player.transform.position.z);
                transform.rotation = Quaternion.Euler(0, 0, 100f);
            }
            
            if (Input.GetKeyDown(KeyCode.Q))
            {
                transform.rotation = Quaternion.Euler(0, 0, 130f);
                player.IsShovel = false;
                IsInRange = true;
                GetComponent<SpriteRenderer>().sortingOrder = 0;
                transform.Find("Highlight").GetComponent<Renderer>().enabled = true;
            }
        }
        if (IsInRange && Input.GetKeyDown(KeyCode.F))
        {
            // Block if already performing an action
            if (PlayerActionManager.Instance != null && PlayerActionManager.Instance.IsPerformingAction) return;

            if (player != null && !player.IsShovel)
            {
                PlayerActionManager.Instance.TryStartAction(ActionType.PickUpItem, () =>
                {
                    player.IsShovel = true;
                    GetComponent<SpriteRenderer>().sortingOrder = 2;
                    transform.Find("Highlight").GetComponent<Renderer>().enabled = false;
                });
            }
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IsInRange = true;
            Debug.Log("Enter");
            transform.Find("Highlight").GetComponent<Renderer>().enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player"))
        {
            IsInRange = false;
            Debug.Log("Exit");
            transform.Find("Highlight").GetComponent<Renderer>().enabled = false;
        }
    }
}
