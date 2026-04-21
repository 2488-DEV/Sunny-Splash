using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider slider;
    public float maxValue = 100f;
    public float timer = 0f;
    private PlayerScript player; 

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<PlayerScript>();
        }

        slider.maxValue = maxValue;
        slider.value = 100;
    }

    void Update()
    {   
        // Don't drain movement stamina during actions (player is frozen)
        if (PlayerActionManager.Instance != null && PlayerActionManager.Instance.IsPerformingAction)
        {
            timer += Time.deltaTime;
            return;
        }

        if (player.IsShovel)
        {
            if ((Input.GetAxisRaw("Vertical") != 0) || (Input.GetAxisRaw("Horizontal") != 0))
            {
                slider.value -= 0.025f;
            }
        }
        else if (timer >= 1f)
        {
            slider.value += 2f;
            timer = 0f;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            slider.value -= 0.1f;
        }
        
        timer += Time.deltaTime; // นับเวลาจริง (ขึ้นกับ Time.timeScale)
    }
}
