using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MobileUIStage : MonoBehaviour
{
    public GameObject eggButton;
    public GameObject pickButton;
    public GameObject dropButton;
    public GameObject interactButton;
    public GameObject quackButton;
    public GameObject sprintButton;
    public GameObject joyStick;
    public PlayerScript player;
    public SettingMenuManger settingMenuManager;
    public Texture2D sprint_toggle;
    public Texture2D egg_toggle;
    public Texture2D sprint_normal;
    public Texture2D egg_normal;
    private RawImage sprintSR;
    private RawImage eggSR;

    void Start()
    {
        sprintSR = sprintButton.GetComponent<RawImage>();
        eggSR = eggButton.GetComponent<RawImage>();
    }

    void Update()
    {
        if (SettingMenuManger.isMobile)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            if (joyStick != null) joyStick.SetActive(true); 
            if (interactButton != null) interactButton.SetActive(true); 
            if (quackButton != null) quackButton.SetActive(true); 
            if (sprintButton != null) sprintButton.SetActive(true); 
            if (PlayerScript.isEgg) eggSR.texture = egg_toggle;
            if (PlayerMovement.isToggleRunning) sprintSR.texture = sprint_toggle;
            if (!PlayerScript.isEgg) eggSR.texture = egg_normal;
            if (!PlayerMovement.isToggleRunning) sprintSR.texture = sprint_normal;
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
            } else if(currentSceneName == "InGame_Lv3" || currentSceneName == "InGame_Lv4") {
                if (eggButton != null) eggButton.SetActive(true); 
                if (pickButton != null) pickButton.SetActive(true); 
                if (dropButton != null && player != null && player.IsShovel) dropButton.SetActive(true); 
                if (dropButton != null && player != null && !player.IsShovel) dropButton.SetActive(false); 
            } 
        }
        else
        {
            if (eggButton != null) eggButton.SetActive(false); 
            if (pickButton != null) pickButton.SetActive(false); 
            if (dropButton != null) dropButton.SetActive(false); 
            if (interactButton != null) interactButton.SetActive(false); 
            if (quackButton != null) quackButton.SetActive(false); 
            if (sprintButton != null) sprintButton.SetActive(false); 
            if (joyStick != null) joyStick.SetActive(false); 
        }
    }
}