using UnityEngine;
using UnityEngine.SceneManagement;


public class MobileUIStage : MonoBehaviour
{
    public GameObject eggButton;
    public GameObject pickButton;
    public GameObject dropButton;
    public PlayerScript player;

    void Start()
    {
        
    }

    void Update()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "InGame_Lv1")
        {
            if (eggButton != null) eggButton.SetActive(false); 
            if (pickButton != null) pickButton.SetActive(false); 
            if (dropButton != null) dropButton.SetActive(false); 
        }
        else if(currentSceneName == "InGame_Lv2") {
            if (eggButton != null) eggButton.SetActive(false); 
            if (pickButton != null) pickButton.SetActive(true); 
            if (dropButton != null && player != null && player.IsShovel) dropButton.SetActive(true); 
            if (dropButton != null && player != null && !player.IsShovel) dropButton.SetActive(false); 
        } else if(currentSceneName == "InGame_Lv3") {
            if (eggButton != null) eggButton.SetActive(true); 
            if (pickButton != null) pickButton.SetActive(true); 
            if (dropButton != null && player != null && player.IsShovel) dropButton.SetActive(true); 
            if (dropButton != null && player != null && !player.IsShovel) dropButton.SetActive(false); 
        }
    }
}