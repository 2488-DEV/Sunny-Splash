using UnityEngine;

public class SeedScript : MonoBehaviour
{
    public bool IsInRange;
    private PlayerScript player;
    private ShovelScript shovel;
    private FoodScript food;
    private GameObject highlight;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<PlayerScript>();
        }

        shovel = FindFirstObjectByType<ShovelScript>();
        food = FindFirstObjectByType<FoodScript>();

        // เก็บอ้างอิง Highlight ไว้จะได้ไม่โหลดบ่อยกวัก
        Transform h = transform.Find("Highlight");
        if (h != null) highlight = h.gameObject;
    }

    void OnEnable() { GameInput.OnPickUp += TryPickUp; }
    void OnDisable() { GameInput.OnPickUp -= TryPickUp; }

    void TryPickUp()
    {
        // เช็คระยะและเงื่อนไขเดิมของเจมส์
        if (!IsInRange || (shovel != null && shovel.IsInRange)) return;
        if (PlayerActionManager.Instance != null && PlayerActionManager.Instance.IsPerformingAction) return;

        // ... (โค้ดเก็บของเดิมของเจมส์)
        PlayerActionManager.Instance.TryStartAction(ActionType.PickUpItem, () =>
        {
            player.seed += 1;
            player.UpdateSeedCount();
            gameObject.SetActive(false);
        });
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