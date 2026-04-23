using UnityEngine;

public class PlantScript : MonoBehaviour
{
    public bool IsInRange;
    public enum PlantState { Empty, Dead, Dehydrated, Fresh }
    public PlantState currentStage;

    private PlayerScript player;
    private StaminaBar staminaBar;
    private WaterRefillSystem waterSystem;
    private PlayerSound playerSound; // คุมเสียงผ่านตัวเป็ดอย่างเดียวกวัก

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

    void Update()
    {
        if (!IsInRange || player == null || PlayerActionManager.Instance.IsPerformingAction)
        {
            UpdateVisuals();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (staminaBar != null && staminaBar.currentStamina < 10f) return;

            if (currentStage == PlantState.Dead && player.IsShovel)
            {
                PlayerActionManager.Instance.TryStartAction(ActionType.Dig, () => {
                    HandlePlantLogic(ActionType.Dig);
                    // สั่งเสียงผ่านตัวเป็ดกวัก!
                    if (playerSound != null) playerSound.PlayActionSound("Dig");
                });
            }
            else if (currentStage == PlantState.Empty && !player.IsShovel && player.seed >= 1)
            {
                PlayerActionManager.Instance.TryStartAction(ActionType.PlantSeed, () => {
                    player.seed -= 1;
                    player.UpdateSeedCount();
                    HandlePlantLogic(ActionType.PlantSeed);
                    if (playerSound != null) playerSound.PlayActionSound("Plant");
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
                        if (playerSound != null) playerSound.PlayActionSound("Water");
                    });
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