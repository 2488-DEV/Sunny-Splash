using UnityEngine;
using UnityEngine.Audio;

public class MainMenuControl : MonoBehaviour
{
    [Header("Panels")]
    public GameObject settingPanel;
    public GameObject informationPanel;
    public GameObject howToPlayPanel;
    public GameObject levelSelectPanel;

    [Header("Audio")]
    public AudioMixer myMixer;
    public ButtonSound buttonSoundScript;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (buttonSoundScript != null) buttonSoundScript.PlayClick();

            // 1. ถ้าหน้า Setting เปิดอยู่แล้ว -> ให้ปิดทุกอย่าง (รวมถึงตัวเอง)
            if (settingPanel.activeSelf)
            {
                CloseAllPanels();
            }
            // 2. ถ้าหน้าอื่น (Level/Info/HowTo) เปิดอยู่ หรือไม่มีอะไรเปิดเลย -> ให้ปิดหน้าอื่นแล้วเปิด Setting ทันที
            else
            {
                CloseAllPanels(); // เคลียร์หน้าอื่นทิ้งก่อน
                OpenSetting();    // เปิด Setting ขึ้นมา
            }
        }
    }

    public void OpenLevelSelect()
    {
        if (buttonSoundScript != null) buttonSoundScript.PlayClick();
        CloseAllPanels();
        if (levelSelectPanel != null) levelSelectPanel.SetActive(true);
    }

    public void OpenSetting()
    {
        if (settingPanel != null) settingPanel.SetActive(true);
    }

    public void CloseAllPanels()
    {
        if (settingPanel != null) settingPanel.SetActive(false);
        if (informationPanel != null) informationPanel.SetActive(false);
        if (howToPlayPanel != null) howToPlayPanel.SetActive(false);
        if (levelSelectPanel != null) levelSelectPanel.SetActive(false);
    }

    public void SetVolume(float sliderValue)
    {
        if (sliderValue <= 0) sliderValue = 0.0001f;
        myMixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
    }
}