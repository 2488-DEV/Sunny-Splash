using UnityEngine;

public class ShovelScript : MonoBehaviour
{
    public bool IsInRange;
    private PlayerScript player;
    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<PlayerScript>();
        }
    }

    void Update()
    {   
        if (IsInRange && !player.IsShovel)
        {
            transform.Find("Highlight").GetComponent<Renderer>().enabled = true;
        }
        else
        {
            transform.Find("Highlight").GetComponent<Renderer>().enabled = false;
        }
        if (player.IsShovel)
        {
            transform.position = new Vector3(player.transform.position.x , player.transform.position.y , player.transform.position.z);
        }
        if (IsInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (player != null)
            {
                player.IsShovel = true;
                GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
             

        }
        if (player.IsShovel && Input.GetKeyDown(KeyCode.Q))
        {
            player.IsShovel = false;
            GetComponent<SpriteRenderer>().sortingOrder = -1;
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
