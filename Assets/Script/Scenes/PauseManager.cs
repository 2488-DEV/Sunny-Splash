using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject settingsPanel;
    public GameObject victoryPanel;

    [Header("Audio Settings (Mixer)")]
    public AudioMixer myMixer;
    public Slider bgmSlider;
    public Slider sfxSlider; // ช่องสำหรับลาก Slider SFX ในหน้า Pause กวัก!

    [Header("Audio Settings (Source)")]
    public AudioSource bgmSource;
    private bool isPaused = false;
    private float maxBgmVolume = 0.5f;

    void Start()
    {
        // --- 1. ดึงค่าเสียงที่บันทึกไว้ข้าม Scene ---
        float savedBgm = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
        float savedSfx = PlayerPrefs.GetFloat("SFXVolume", 0.8f);

        // --- 2. สั่งให้ Mixer ในฉากนี้เปลี่ยนตามค่าที่เซฟไว้ทันที ---
        if (myMixer != null)
        {
            myMixer.SetFloat("MusicVol", Mathf.Log10(Mathf.Clamp(savedBgm, 0.0001f, 1f)) * 20);
            myMixer.SetFloat("SFXVol", Mathf.Log10(Mathf.Clamp(savedSfx, 0.0001f, 1f)) * 20);
        }

        // --- 3. ตั้งค่า UI Slider ให้ตรงกับค่าที่โหลดมา ---
        if (bgmSlider != null)
        {
            bgmSlider.value = savedBgm / maxBgmVolume;
            bgmSlider.onValueChanged.AddListener(SetVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = savedSfx;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume); // เชื่อมฟังก์ชันใหม่กวัก!
        }

        if (bgmSource != null)
        {
            bgmSource.volume = savedBgm;
        }

        AudioListener.volume = 1.0f;

        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
    }

    void Update()
    {
        if (victoryPanel != null && victoryPanel.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        settingsPanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        settingsPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    // คุมเสียงเพลง (Music)
    public void SetVolume(float volume)
    {
        float finalVolume = volume * maxBgmVolume;
        if (myMixer != null)
        {
            myMixer.SetFloat("MusicVol", Mathf.Log10(Mathf.Clamp(finalVolume, 0.0001f, 1f)) * 20);
        }
        if (bgmSource != null) bgmSource.volume = finalVolume;
        PlayerPrefs.SetFloat("BGMVolume", finalVolume);
    }

    // คุมเสียงเอฟเฟกต์ (SFX) กวัก!
    public void SetSFXVolume(float volume)
    {
        if (myMixer != null)
        {
            myMixer.SetFloat("SFXVol", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
        }
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void GoToMainMenu() { StartCoroutine(WaitAndLoad("MainMenu")); }
    public void NextLevel(string sceneName) { StartCoroutine(WaitAndLoad(sceneName)); }
    public void RestartLevel() { StartCoroutine(WaitAndLoad(SceneManager.GetActiveScene().name)); }

    IEnumerator WaitAndLoad(string sceneName)
    {
        Time.timeScale = 1f;
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(sceneName);
    }
}