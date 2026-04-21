using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OverHeatBar : MonoBehaviour
{   
    public WaterColler waterColler;
    public PlayerMovement playerMovement;
    public SunSystem sunSystem;

    public Slider slider;
    public Image fillImage;
    public GameObject player;

    public float maxValue = 100f;
    public float timer = 0f;
    
    void Start()
    {
        slider.maxValue = maxValue;
        slider.value = 0;
    }

    void Update()
    {

        timer += Time.deltaTime; // นับเวลาจริง (ขึ้นกับ Time.timeScale)

        if (timer >= 1f)
        {
        // ลำดับความสำคัญ
            if (playerMovement.isInWater)
            {
            slider.value -= 3f; // น้ำลดเร็ว
            }
            else if (playerMovement.isInShadow)
            {
            slider.value -= 1f; // เงาลดช้า
            }
            else if (sunSystem.isSunActive)
            {
            slider.value += 1f; // โดนแดดเพิ่ม
            }

        timer = 0f;
        }
        
        slider.value = Mathf.Clamp(slider.value, 0, 100);

        if (slider.value >= 100f)
        {
            Debug.Log("YOU ARE DEAD");
            player.SetActive(false);
        }
        
        else if (slider.value >= 90f)
        {
            fillImage.color = Color.red;
        }

        else
        {
            fillImage.color = new Color32(255,113,0,255);
        }
    }
}
