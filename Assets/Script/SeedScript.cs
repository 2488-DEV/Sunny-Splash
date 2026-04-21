using UnityEngine;
using TMPro;

public class SeedScript : MonoBehaviour
{
    public bool IsInRange;
    private PlayerScript player;
    private ShovelScript shovel;
    public TextMeshProUGUI interact;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<PlayerScript>();
        }
        shovel = FindFirstObjectByType<ShovelScript>();
    }

    void Update()
    {   
        if (IsInRange && !shovel.IsInRange)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (player != null)
                {
                    player.seed += 1;
                    player.UpdateSeedCount();
                    gameObject.SetActive(false);
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
            interact.enabled = false;
            transform.Find("Highlight").GetComponent<Renderer>().enabled = false;
        }
    }
}
