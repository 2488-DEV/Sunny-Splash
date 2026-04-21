using UnityEngine;
using UnityEngine.UI;

public class ActionBarUI : MonoBehaviour
{
    [Header("UI References")]
    public Slider progressBar;
    public GameObject uiContainer;
    private Canvas playerCanvas;
    void Start()
    {
        if (uiContainer != null)
        {
            playerCanvas = uiContainer.GetComponentInParent<Canvas>();
        }
        // 1. ติดตาม Event เมื่อเริ่มและจบ Action
        PlayerActionManager.Instance.OnActionStarted += HandleActionStarted;
        PlayerActionManager.Instance.OnActionCompleted += HandleActionFinished;
        PlayerActionManager.Instance.OnActionCancelled += HandleActionFinished;

        // เริ่มต้นให้ปิด UI ไว้ก่อน
        HideUI();
    }

    void Update()
    {
        // 2. อัปเดต Progress Bar ตลอดเวลาที่กำลังทำ Action
        if (PlayerActionManager.Instance.IsPerformingAction)
        {
            // ActionProgress ใน Manager คือ 0-1 อยู่แล้ว
            // เราแค่เอามาคูณกับ maxValue ของ Slider
            progressBar.value = PlayerActionManager.Instance.ActionProgress;
        }
    }

    private void HandleActionStarted(ActionType action)
    {
        // 3. ตั้งค่า Slider ตามความต้องการ
        // เนื่องจาก ActionProgress ใน Manager เป็น 0 ถึง 1 
        // แนะนำให้ตั้ง maxValue เป็น 1 เพื่อความง่ายครับ
        progressBar.maxValue = 1f;
        progressBar.value = 0f;

        ShowUI();
    }

    private void HandleActionFinished(ActionType action)
    {
        HideUI();
    }

    private void ShowUI()
    {
        if (playerCanvas != null) playerCanvas.enabled = true;
        // หรือถ้าอยากใช้ SetActive ก็ได้ครับ
        // uiContainer.SetActive(true);
    }

    private void HideUI()
    {
        if (playerCanvas != null) playerCanvas.enabled = false;
        // uiContainer.SetActive(false);
    }

    private void OnDestroy()
    {
        // อย่าลืมยกเลิกการติดตามเมื่อสคริปต์ถูกทำลาย เพื่อป้องกัน Memory Leak
        if (PlayerActionManager.Instance != null)
        {
            PlayerActionManager.Instance.OnActionStarted -= HandleActionStarted;
            PlayerActionManager.Instance.OnActionCompleted -= HandleActionFinished;
            PlayerActionManager.Instance.OnActionCancelled -= HandleActionFinished;
        }
    }
}