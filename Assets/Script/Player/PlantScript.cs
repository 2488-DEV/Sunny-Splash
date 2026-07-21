using UnityEngine;

public class PlantScript : MonoBehaviour
{
    public bool IsInRange;
    public enum PlantState { Empty, Dead, Dehydrated, Fresh }
    public PlantState currentStage;

    private PlayerScript player;
    private StaminaBar staminaBar;
    private WaterRefillSystem waterSystem;
    private PlayerSound playerSound;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<PlayerScript>();
            waterSystem = playerObj.GetComponent<WaterRefillSystem>();
            playerSound = playerObj.GetComponent<PlayerSound>();
        }
        staminaBar = FindFirstObjectByType<StaminaBar>();
    }

    void OnEnable() { GameInput.OnInteract += TryInteract; }
    void OnDisable() { GameInput.OnInteract -= TryInteract; }

    void TryInteract()
    {
        // ถ้าไม่อยู่ในระยะ หรือ กำลังทำ Action อื่นอยู่ ให้ข้ามไปเลย
        if (!IsInRange || player == null || PlayerActionManager.Instance.IsPerformingAction) return;

        if (staminaBar != null && staminaBar.currentStamina < 10f) return;

        // 1. ขุดซากกองเถ้า
        if (currentStage == PlantState.Dead && player.IsShovel)
        {
            PlayerActionManager.Instance.TryStartAction(ActionType.Dig, () => {
                HandlePlantLogic(ActionType.Dig);
                if (playerSound != null) playerSound.PlayActionSound("Dig");
            });
        }
        // 2. ปลูกเมล็ด
        else if (currentStage == PlantState.Empty && !player.IsShovel && player.seed >= 1)
        {
            PlayerActionManager.Instance.TryStartAction(ActionType.PlantSeed, () => {
                player.UseSeed();
                HandlePlantLogic(ActionType.PlantSeed);
                if (playerSound != null) playerSound.PlayActionSound("Plant");
            });
        }
        // 3. รดน้ำ
        else if (currentStage == PlantState.Dehydrated && !player.IsShovel)
        {
            if (waterSystem != null && waterSystem.currentWater >= 33f)
            {
                PlayerActionManager.Instance.TryStartAction(ActionType.Water, () => {
                    waterSystem.UseWaterForPlanting();
                    HandlePlantLogic(ActionType.Water);
                    player.DecreaseTree();
                    if (playerSound != null) playerSound.PlayActionSound("Water");
                });
            }
        }
    }

    void Update()
    {
        if (!IsInRange || player == null || PlayerActionManager.Instance.IsPerformingAction)
        {
            UpdateVisuals();
            return;
        }

        UpdateVisuals();
    }

    void HandlePlantLogic(ActionType action)
    {
        switch (action)
        {
            case ActionType.Dig:
                currentStage = PlantState.Empty;
                break;
            case ActionType.PlantSeed:
                currentStage = PlantState.Dehydrated;
                break;
            case ActionType.Water:
                currentStage = PlantState.Fresh;
                break;
        }

        if (staminaBar != null)
        {
            staminaBar.currentStamina -= 10f;
        }
    }

    // --- Visuals เหมือนเดิมเป๊ะ ไม่พังแน่นอนกวัก ---
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