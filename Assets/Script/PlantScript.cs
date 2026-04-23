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
    private WaterRefillSystem waterSystem;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<PlayerScript>();
            waterSystem = playerObj.GetComponent<WaterRefillSystem>();
        }
        staminaBar = FindFirstObjectByType<StaminaBar>();
    }

    void Update()
    {
        if (!IsInRange || player == null || PlayerActionManager.Instance.IsPerformingAction)
        {
            UpdateVisuals();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // --- แก้จุดเช็ค Stamina ให้เช็คจากตัวแปร currentStamina แทนกวัก ---
            if (staminaBar != null && staminaBar.currentStamina < 10f)
            {
                Debug.Log("เหนื่อยแล้วกวัก! พักแป๊บนาย");
                return;
            }

            if (currentStage == PlantState.Dead && player.IsShovel)
            {
                PlayerActionManager.Instance.TryStartAction(ActionType.Dig, () => {
                    HandlePlantLogic(ActionType.Dig);
                });
            }
            else if (currentStage == PlantState.Empty && !player.IsShovel && player.seed >= 1)
            {
                PlayerActionManager.Instance.TryStartAction(ActionType.PlantSeed, () => {
                    player.seed -= 1;
                    player.UpdateSeedCount();
                    HandlePlantLogic(ActionType.PlantSeed);
                });
            }
            else if (currentStage == PlantState.Dehydrated && !player.IsShovel)
            {
                if (waterSystem != null && waterSystem.currentWater >= 33f)
                {
                    PlayerActionManager.Instance.TryStartAction(ActionType.Water, () => {
                        waterSystem.UseWaterForPlanting();
                        HandlePlantLogic(ActionType.Water);
                        player.DecreaseTree();
                    });
                }
                else
                {
                    Debug.Log("น้ำไม่พอรดแล้วนาย! ไปเติมน้ำด่วนกวัก!");
                }
            }
        }
        UpdateVisuals();
    }

    void HandlePlantLogic(ActionType action)
    {
        switch (action)
        {
            case ActionType.Dig:
                currentStage = PlantState.Empty;
                source.PlayOneShot(digSfx);
                break;
            case ActionType.PlantSeed:
                currentStage = PlantState.Dehydrated;
                source.PlayOneShot(plantSfx);
                break;
            case ActionType.Water:
                currentStage = PlantState.Fresh;
                source.PlayOneShot(wateringSfx);
                break;
        }

        // --- จุดสำคัญ: เปลี่ยนจากหัก slider.value มาเป็นหัก currentStamina ---
        // วิธีนี้จะทำให้หลอดค่อยๆ วิ่งลดลงอย่างนิ่มนวล ไม่วาร์ปกวัก!
        if (staminaBar != null)
        {
            staminaBar.currentStamina -= 10f;
        }
    }

    void UpdateVisuals()
    {
        SetPlantActive("DeadPlant", currentStage == PlantState.Dead);
        SetPlantActive("DehydratedPlant (lower)", currentStage == PlantState.Dehydrated);
        SetPlantActive("DehydratedPlant (upper)", currentStage == PlantState.Dehydrated);
        SetPlantActive("FreshPlant (lower)", currentStage == PlantState.Fresh);
        SetPlantActive("FreshPlant (upper)", currentStage == PlantState.Fresh);
        SetPlantActive("Hitbox", currentStage == PlantState.Fresh);
    }

    void SetPlantActive(string name, bool isActive)
    {
        Transform t = transform.Find(name);
        if (t != null) t.gameObject.SetActive(isActive);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) IsInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) IsInRange = false;
    }
}