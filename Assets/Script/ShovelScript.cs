using UnityEngine;
using TMPro;

public class ShovelScript : MonoBehaviour
{
    public bool IsInRange;
    private PlayerScript player;
    public TextMeshProUGUI interact;
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
            transform.position = new Vector3(player.transform.position.x , player.transform.position.y , player.transform.position.z);
            if (Input.GetKeyDown(KeyCode.Q))
            {
                player.IsShovel = false;
                IsInRange = true;
                GetComponent<SpriteRenderer>().sortingOrder = -1;
                transform.Find("Highlight").GetComponent<Renderer>().enabled = true;
                interact.enabled = true;
            }
        }
        if (IsInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (player != null)
            {
                player.IsShovel = true;
                
                GetComponent<SpriteRenderer>().sortingOrder = 2;
                transform.Find("Highlight").GetComponent<Renderer>().enabled = false;
                if (seed.IsInRange)
                {
                    interact.enabled = false;
                    Debug.Log("NoSeed");
                }
                else
                {
                    interact.enabled = true;
                    Debug.Log("Seed");
                }
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
            interact.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player"))
        {
            IsInRange = false;
            Debug.Log("Exit");
            transform.Find("Highlight").GetComponent<Renderer>().enabled = false;
            interact.enabled = false;
        }
    }
}
