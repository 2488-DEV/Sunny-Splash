using UnityEngine;

public class PlantScript : MonoBehaviour
{   
    public bool IsInRange;
    public enum PlantState { Empty, Dead, Dehydrated, Fresh }
    public PlantState currentStage;

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
        if (IsInRange && player != null)
        {
            if (currentStage == PlantState.Dead)
            {
                if (player.IsShovel && Input.GetKeyDown(KeyCode.Space))
                {
                    HandlePlantLogic();
                }
            }
            else
            {
                if (!player.IsShovel && Input.GetKeyDown(KeyCode.Space))
                {
                    HandlePlantLogic();
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
