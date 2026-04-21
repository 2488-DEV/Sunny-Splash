using UnityEngine;

public class PlantScript : MonoBehaviour
{   
    public bool IsInRange;
    public enum PlantState { Empty, Dead, Dehydrated, Fresh }
    public PlantState currentStage;

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
            // Block if already performing an action
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
                        player.water_gauge -= 10;
                        HandlePlantLogic();
                        player.UpdateWater();
                    });
                }
            }
            else if (currentStage == PlantState.Fresh)
            {
                if (!player.IsShovel)
                {
                    PlayerActionManager.Instance.TryStartAction(ActionType.Harvest, () =>
                    {
                        HandlePlantLogic();
                    });
                }
            }
            
        }
        UpdateVisuals();
    }

    void HandlePlantLogic()
    {
        if (currentStage == PlantState.Dead)
            currentStage = PlantState.Empty;
        else if (currentStage == PlantState.Empty)
            currentStage = PlantState.Dehydrated;
        else if (currentStage == PlantState.Dehydrated)
            currentStage = PlantState.Fresh;
        staminaBar.slider.value -= 10;
    }

    void UpdateVisuals()
    {
        transform.Find("DeadPlant").gameObject.SetActive(currentStage == PlantState.Dead);
        transform.Find("DehydratedPlant").gameObject.SetActive(currentStage == PlantState.Dehydrated);
        transform.Find("FreshPlant").gameObject.SetActive(currentStage == PlantState.Fresh);
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IsInRange = true;
            Debug.Log("Enter");
        }

    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player"))
        {
            IsInRange = false;
            Debug.Log("Exit");
        }
    }
}
