using UnityEngine;

public class SeedScript : MonoBehaviour
{
    public bool IsInRange;
    private PlayerScript player;
    private ShovelScript shovel;
    private GameObject highlight;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<PlayerScript>();
        }

        shovel = FindFirstObjectByType<ShovelScript>();

        // เก็บอ้างอิง Highlight ไว้จะได้ไม่โหลดบ่อยกวัก
        Transform h = transform.Find("Highlight");
        if (h != null) highlight = h.gameObject;
    }

    void Update()
    {
        // เช็คว่าอยู่ในระยะ และพลั่วไม่ได้ถูกใช้งานอยู่ (กันปุ่มซ้อน)
        if (IsInRange && (shovel == null || !shovel.IsInRange))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (PlayerActionManager.Instance != null && PlayerActionManager.Instance.IsPerformingAction) return;

                if (player != null)
                {
                    PlayerActionManager.Instance.TryStartAction(ActionType.PickUpItem, () =>
                    {
                        player.seed += 1;
                        player.UpdateSeedCount(); // อัปเดตตัวเลขบนจอทันทีกวัก!
                        gameObject.SetActive(false);
                    });
                }
            }
        }
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