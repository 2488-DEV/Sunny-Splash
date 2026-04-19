using UnityEngine;

public class ClonePlot : MonoBehaviour
{

    public GameObject plot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i <= 4; i++)
        {
            GameObject clone = Instantiate(plot);
            clone.transform.gameObject.SetActive(true);
            PlantScript script = clone.GetComponent<PlantScript>();

            if (script != null)
            {
                // 2. ส่งค่าเลข 1 (หรือค่าที่สุ่ม) เข้าไปในตัวแปร currentStage ของสคริปต์นั้น
                script.currentStage = (PlantScript.PlantState)Random.Range(1,3); 
            }

            clone.transform.position = new Vector3(plot.transform.position.x + i * 3 , plot.transform.position.y , 0f);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
