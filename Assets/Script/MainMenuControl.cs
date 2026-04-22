using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI; // เพิ่มตัวนี้มาเพื่อคุม Slider

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
    public Slider bgmSlider; // เพิ่มตัวนี้เพื่อตั้งค่าเริ่มต้นตอนเปิดเกม

    void Start()
    {
        // โหลดค่าจาก "BGMVolume" (ชื่อเดียวกับใน PauseManager)
        float savedVolume = PlayerPrefs.GetFloat("BGMVolume", 0.75f);

        if (bgmSlider != null) bgmSlider.value = savedVolume;

        SetVolume(savedVolume); // สั่งให้ Mixer ปรับตามค่าที่โหลดมา
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (buttonSoundScript != null) buttonSoundScript.PlayClick();

            if (settingPanel.activeSelf)
            {
                CloseAllPanels();
            }
            else
            {
                CloseAllPanels();
                OpenSetting();
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
        if (sliderValue <= 0.0001f) sliderValue = 0.0001f;

        // 1. ปรับที่ Mixer (สำหรับใน Main Menu)
        myMixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);

        // 2. ปรับที่ AudioListener (เพื่อให้ PauseManager ใน Scene อื่นๆ ทำงานตรงกัน)
        AudioListener.volume = sliderValue;

        // 3. บันทึกค่าลงเครื่องโดยใช้ชื่อ "BGMVolume" (ชื่อเดียวกับใน PauseManager)
        PlayerPrefs.SetFloat("BGMVolume", sliderValue);
    }
}