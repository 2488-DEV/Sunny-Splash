using UnityEngine;

public class ClonePlotScript : MonoBehaviour
{
    
    public GameObject plot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        for (int y = 0; y <= 12; y += 3)
        {
            for (int i = 0; i <= 4; i++)
            {
                GameObject clone = Instantiate(plot);
                clone.transform.gameObject.SetActive(true);
                PlantScript script = clone.GetComponent<PlantScript>();

                if (script != null)
                {
                    script.currentStage = (PlantScript.PlantState)Random.Range(0,3); 
                }

                clone.transform.position = new Vector3(plot.transform.position.x + i * 3 , plot.transform.position.y + y , 0f);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
