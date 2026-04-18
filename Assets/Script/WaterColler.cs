using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;

public class WaterColler : MonoBehaviour
{
    public OverHeatBar slider;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (slider.timer >= 1f)
            {
                slider.slider.value -= 2f;
                slider.timer = 0f;
            }
        }
    }
}
