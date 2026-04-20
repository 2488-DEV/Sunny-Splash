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
            slider.value += 1f;
            timer = 0f;
        }

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
