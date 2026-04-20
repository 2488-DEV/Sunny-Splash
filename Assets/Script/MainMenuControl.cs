using UnityEngine;
using UnityEngine.Audio;

public class MainMenuControl : MonoBehaviour
{
    public GameObject settingPanel;
    public GameObject informationPanel;
    public GameObject howToPlayPanel;
    public AudioMixer myMixer;

    // เชื่อมกับสคริปต์ ButtonSound โดยตรง
    public ButtonSound buttonSoundScript;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // เรียกใช้ฟังก์ชัน PlayClick ที่คุณเขียนไว้ในอีกสคริปต์
            if (buttonSoundScript != null)
            {
                buttonSoundScript.PlayClick();
            }

            if (settingPanel.activeSelf)
            {
                CloseSetting();
            }
            else
            {
                CloseAllPanels();
                OpenSetting();
            }
        }
    }

    public void OpenSetting()
    {
        settingPanel.SetActive(true);
    }

    public void CloseSetting()
    {
        settingPanel.SetActive(false);
    }

    public void CloseAllPanels()
    {
        if (settingPanel != null) settingPanel.SetActive(false);
        if (informationPanel != null) informationPanel.SetActive(false);
        if (howToPlayPanel != null) howToPlayPanel.SetActive(false);
    }

    public void SetVolume(float sliderValue)
    {
        if (sliderValue <= 0) sliderValue = 0.0001f;
        myMixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
    }
}