using UnityEngine;

public class FoodScript : MonoBehaviour
{
    public bool IsInRange;
    private PlayerScript player;
    private ShovelScript shovel;
    private SeedScript seed;
    private GameObject highlight;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<PlayerScript>();
        }

        shovel = FindFirstObjectByType<ShovelScript>();
        seed = FindFirstObjectByType<SeedScript>();

        Transform h = transform.Find("Highlight");
        if (h != null) highlight = h.gameObject;
    }

    void OnEnable() { GameInput.OnPickUp += TryPickUpFood; }
    void OnDisable() { GameInput.OnPickUp -= TryPickUpFood; }

    void TryPickUpFood()
    {
        if (!IsInRange || (shovel != null && shovel.IsInRange) || (seed != null && seed.IsInRange)) return;
        
        if (PlayerActionManager.Instance != null && PlayerActionManager.Instance.IsPerformingAction) return;

        if (player != null)
        {
            PlayerActionManager.Instance.TryStartAction(ActionType.PickUpItem, () =>
            {
                player.playerHp += 1;
                player.egg += 1;
                player.UpdateEggCount();
                gameObject.SetActive(false);
            });
        }
    }

    void Update() 
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            IsInRange = true;
            if (highlight != null) highlight.GetComponent<Renderer>().enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            IsInRange = false;
            if (highlight != null) highlight.GetComponent<Renderer>().enabled = false;
        }
    }
}
