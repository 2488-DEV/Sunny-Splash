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

            // --- ส่วนที่อัปเดต: บังคับให้ Canvas อยู่หน้าสุดเสมอ ---
            if (playerCanvas != null)
            {
                // เปลี่ยนโหมดเป็น WorldSpace (ถ้ายังไม่ได้ตั้งใน Inspector)
                playerCanvas.renderMode = RenderMode.WorldSpace;

                // บังคับเลเยอร์ไปที่ UI หรือเลเยอร์ที่นายสร้างใหม่ (เช่น TopUI)
                playerCanvas.sortingLayerName = "UI";

                // ตั้งค่า Order ให้สูงมากๆ เพื่อทับทุกอย่างในฉาก
                playerCanvas.sortingOrder = 100;
            }
        }

        PlayerActionManager.Instance.OnActionStarted += HandleActionStarted;
        PlayerActionManager.Instance.OnActionCompleted += HandleActionFinished;
        PlayerActionManager.Instance.OnActionCancelled += HandleActionFinished;

        HideUI();
    }

    void Update()
    {
        if (PlayerActionManager.Instance.IsPerformingAction)
        {
            progressBar.value = PlayerActionManager.Instance.ActionProgress;
        }
    }

    private void HandleActionStarted(ActionType action)
    {
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
    }

    private void HideUI()
    {
        if (playerCanvas != null) playerCanvas.enabled = false;
    }

    private void OnDestroy()
    {
        if (PlayerActionManager.Instance != null)
        {
            PlayerActionManager.Instance.OnActionStarted -= HandleActionStarted;
            PlayerActionManager.Instance.OnActionCompleted -= HandleActionFinished;
            PlayerActionManager.Instance.OnActionCancelled -= HandleActionFinished;
        }
    }
}