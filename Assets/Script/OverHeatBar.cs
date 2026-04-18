using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OverHeatBar : MonoBehaviour
{
    public Slider slider;
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
    }
}
