using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections; // ต้องมีตัวนี้เพิ่มมาเพื่อให้ใช้ Coroutine ได้

public class PauseManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public Slider bgmSlider;
    private bool isPaused = false;

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("BGMVolume", 0.75f);
        if (bgmSlider != null)
        {
            bgmSlider.value = savedVolume;
            bgmSlider.onValueChanged.AddListener(SetVolume);
        }
        AudioListener.volume = savedVolume;
    }

    void Update()
    {
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

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    // ฟังก์ชันสำหรับปุ่มกด (เรียก Coroutine อีกที)
    public void GoToMainMenu()
    {
        StartCoroutine(WaitAndLoad());
    }

    IEnumerator WaitAndLoad()
    {
        // 1. คืนค่าเวลาเป็นปกติก่อน (ถ้าไม่คืนค่า เวลาจะเป็น 0 และ WaitForSeconds จะไม่ทำงาน)
        Time.timeScale = 1f;

        // 2. รอ 1 วินาที (ใช้ WaitForSecondsRealtime จะชัวร์กว่าในกรณีที่เกมหยุดเวลาอยู่)
        yield return new WaitForSecondsRealtime(1f);

        // 3. ย้าย Scene
        SceneManager.LoadScene("MainMenu");
    }
}