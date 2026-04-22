using UnityEngine;

public class PlantScript : MonoBehaviour
{
    public bool IsInRange;
    public enum PlantState { Empty, Dead, Dehydrated, Fresh }
    public PlantState currentStage;
    public AudioSource source;
    public AudioClip digSfx;
    public AudioClip plantSfx;
    public AudioClip wateringSfx;

    private PlayerScript player;
    private StaminaBar staminaBar;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<PlayerScript>();
        }
        staminaBar = FindFirstObjectByType<StaminaBar>();
    }

    void Update()
    {
        if (IsInRange && player != null && Input.GetKeyDown(KeyCode.Space) && staminaBar.slider.value >= 10)
        {
            if (PlayerActionManager.Instance != null && PlayerActionManager.Instance.IsPerformingAction)
            {
                UpdateVisuals();
                return;
            }

            if (currentStage == PlantState.Dead)
            {
                if (player.IsShovel)
                {
                    PlayerActionManager.Instance.TryStartAction(ActionType.Dig, () =>
                    {
                        HandlePlantLogic();
                    });
                }
            }
            else if (currentStage == PlantState.Empty)
            {
                if (!player.IsShovel && player.seed >= 1)
                {
                    PlayerActionManager.Instance.TryStartAction(ActionType.PlantSeed, () =>
                    {
                        player.seed -= 1;
                        player.UpdateSeedCount();
                        HandlePlantLogic();
                    });
                }
            }
            else if (currentStage == PlantState.Dehydrated)
            {
                if (!player.IsShovel && player.water_gauge >= 10)
                {
                    PlayerActionManager.Instance.TryStartAction(ActionType.Water, () =>
                    {
                        // 1. จ่ายค่าน้ำและอัปเดต UI
                        player.water_gauge -= 10;
                        player.UpdateWater();

                        // 2. เปลี่ยนสถานะต้นไม้เป็น Fresh
                        HandlePlantLogic();

                        // 3. สั่งลดจำนวนต้นไม้ที่เหลือ (เรียกใช้ฟังก์ชันอัปเกรดใน PlayerScript)
                        player.DecreaseTree();
                    });
                }
            }
        }
        UpdateVisuals();
    }

    void HandlePlantLogic()
    {
        if (currentStage == PlantState.Dead)
        {
            currentStage = PlantState.Empty;
            source.PlayOneShot(digSfx);
        }
        else if (currentStage == PlantState.Empty)
        {
            currentStage = PlantState.Dehydrated;
            source.PlayOneShot(plantSfx);
        }
        else if (currentStage == PlantState.Dehydrated)
        {
            currentStage = PlantState.Fresh;
            source.PlayOneShot(wateringSfx);
        }
        staminaBar.slider.value -= 10;
    }

    void UpdateVisuals()
    {
        // ใส่ตัวเช็คเพื่อความปลอดภัย (กัน Error: NullReferenceException)
        Transform dead = transform.Find("DeadPlant");
        if (dead != null) dead.gameObject.SetActive(currentStage == PlantState.Dead);

        Transform dehydrated = transform.Find("DehydratedPlant");
        if (dehydrated != null) dehydrated.gameObject.SetActive(currentStage == PlantState.Dehydrated);

        Transform fresh = transform.Find("FreshPlant");
        if (fresh != null) fresh.gameObject.SetActive(currentStage == PlantState.Fresh);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) IsInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) IsInRange = false;
    }
}