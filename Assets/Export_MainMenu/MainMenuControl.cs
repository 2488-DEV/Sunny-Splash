using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuControl : MonoBehaviour
{
    [Header("Panels")]
    public GameObject settingPanel;
    public GameObject informationPanel;
    public GameObject howToPlayPanel;
    public GameObject levelSelectPanel;

    [Header("Audio Mixer")]
    public AudioMixer myMixer; // อย่าลืมลาก MainMixer มาใส่ใน Inspector นะกวัก!

    [Header("BGM Settings")]
    public AudioSource bgmSource;
    public Slider bgmSlider;
    private float maxBgmVolume = 0.5f; // ล็อกเพดานเสียงเพลงไว้ไม่ให้ดังเกินไป

    [Header("SFX Settings")]
    public Slider sfxSlider; // ลาก SFX Slider มาใส่ช่องนี้กวัก
    public ButtonSound buttonSoundScript;

    void Start()
    {
        // --- 1. จัดการระบบ BGM ---
        // โหลดค่าเก่า ถ้าไม่มีให้เริ่มที่ 0.5 (ซึ่งจะถูกเอาไปคำนวณต่อ)
        float savedBgm = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
        if (bgmSlider != null)
        {
            // ปรับตำแหน่ง Slider ให้ตรงกับค่าที่โหลดมา
            bgmSlider.value = savedBgm / maxBgmVolume;
            bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        }
        // สั่งให้ Mixer และ AudioSource ทำงานตามค่าที่โหลดมาทันที
        SetBGMVolume(bgmSlider != null ? bgmSlider.value : 1f);

        // --- 2. จัดการระบบ SFX ---
        // โหลดค่า SFX เก่า ถ้าไม่มีให้เริ่มที่ 0.8 กวัก
        float savedSfx = PlayerPrefs.GetFloat("SFXVolume", 0.8f);
        if (sfxSlider != null)
        {
            sfxSlider.value = savedSfx;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
        // สั่งให้ Mixer ปรับระดับ SFX ตามค่าที่โหลดมา
        SetSFXVolume(sfxSlider != null ? sfxSlider.value : 0.8f);

        // ปิดหน้าต่าง UI ทั้งหมดตอนเริ่ม
        CloseAllPanels();

        // บังคับเสียงรวมของระบบให้ดัง 1 เสมอ (เราจะไปคุมที่ Mixer แทน)
        AudioListener.volume = 1.0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (buttonSoundScript != null) buttonSoundScript.PlayClick();
            if (settingPanel != null && settingPanel.activeSelf)
                CloseAllPanels();
            else
                OpenSetting();
        }
    }

    // --- ฟังก์ชันคุม BGM (MusicVol) ---
    public void SetBGMVolume(float sliderValue)
    {
        float finalVolume = sliderValue * maxBgmVolume;

        if (myMixer != null)
        {
            // เปลี่ยนค่า 0-1 เป็นหน่วยเดซิเบล (-80 ถึง 0 dB)
            float dB = Mathf.Log10(Mathf.Clamp(finalVolume, 0.0001f, 1f)) * 20;
            myMixer.SetFloat("MusicVol", dB);
        }

        if (bgmSource != null)
        {
            bgmSource.volume = finalVolume;
        }

        PlayerPrefs.SetFloat("BGMVolume", finalVolume);
    }

    // --- ฟังก์ชันคุม SFX (SFXVol) กวัก! ---
    public void SetSFXVolume(float sliderValue)
    {
        if (myMixer != null)
        {
            // เปลี่ยนค่า 0-1 เป็นหน่วยเดซิเบล
            float dB = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20;
            myMixer.SetFloat("SFXVol", dB);
        }

        // บันทึกค่า SFX ไว้ใช้ในฉากอื่นๆ กวัก
        PlayerPrefs.SetFloat("SFXVolume", sliderValue);
    }

    // --- ระบบจัดการหน้าจอ (UI Management) ---
    public void OpenLevelSelect()
    {
        if (buttonSoundScript != null) buttonSoundScript.PlayClick();
        CloseAllPanels();
        if (levelSelectPanel != null) levelSelectPanel.SetActive(true);
    }

    public void OpenSetting()
    {
        CloseAllPanels();
        if (settingPanel != null) settingPanel.SetActive(true);
    }

    public void CloseAllPanels()
    {
        if (settingPanel != null) settingPanel.SetActive(false);
        if (informationPanel != null) informationPanel.SetActive(false);
        if (howToPlayPanel != null) howToPlayPanel.SetActive(false);
        if (levelSelectPanel != null) levelSelectPanel.SetActive(false);
    }
}