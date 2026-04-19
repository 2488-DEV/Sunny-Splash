using UnityEngine;

public class PlantScript : MonoBehaviour
{
    public bool IsInRange;
    public enum PlantState { Empty , Dead , Dehydrated , Fresh }
    public PlantState currentStage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {   
        if (IsInRange)
        {
            Debug.Log(currentStage);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (currentStage == PlantState.Dead)
                {
                    currentStage = PlantState.Empty;
                }
                else if (currentStage == PlantState.Empty)
                {
                    currentStage = PlantState.Dehydrated;
                }
                else if (currentStage == PlantState.Dehydrated)
                {
                    currentStage = PlantState.Fresh;
                }
            }
        }

        if (currentStage == PlantState.Dead)
        {
            transform.Find("DeadPlant").gameObject.SetActive(true);
            transform.Find("DehydratedPlant").gameObject.SetActive(false);
            transform.Find("FreshPlant").gameObject.SetActive(false);
        }
        else if (currentStage == PlantState.Empty)
        {
            transform.Find("DeadPlant").gameObject.SetActive(false);
            transform.Find("DehydratedPlant").gameObject.SetActive(false);
            transform.Find("FreshPlant").gameObject.SetActive(false);
        }
        else if (currentStage == PlantState.Dehydrated)
        {
            transform.Find("DehydratedPlant").gameObject.SetActive(true);
            transform.Find("FreshPlant").gameObject.SetActive(false);
            transform.Find("DeadPlant").gameObject.SetActive(false);
        }
        else if (currentStage == PlantState.Fresh)
        {
            transform.Find("FreshPlant").gameObject.SetActive(true);
            transform.Find("DeadPlant").gameObject.SetActive(false);
            transform.Find("DehydratedPlant").gameObject.SetActive(false);
        }
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
