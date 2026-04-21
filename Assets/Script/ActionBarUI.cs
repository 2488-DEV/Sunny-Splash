using UnityEngine;
using UnityEngine.UI;

public class ActionBarUI : MonoBehaviour
{
    [Header("UI References")]
    public Slider progressBar;
    public GameObject uiContainer; // The UI parent object to show/hide and move

    [Header("Screen Space Settings")]
    public Transform playerTransform; // Track this object
    public Vector3 worldOffset = new Vector3(0, -1f, 0); // Distance below player in world units

    [Header("Optional - Fill Color")]
    public Image fillImage;
    public Color fillColor = new Color32(100, 200, 100, 255); // Green

    private Camera mainCam;
    private RectTransform rectTransform;

    void Start()
    {
        mainCam = Camera.main;

        if (uiContainer != null)
        {
            rectTransform = uiContainer.GetComponent<RectTransform>();
            uiContainer.SetActive(false);
        }

        // Set fill color if assigned
        if (fillImage != null)
        {
            fillImage.color = fillColor;
        }

        // Set slider range
        if (progressBar != null)
        {
            progressBar.minValue = 0f;
            progressBar.maxValue = 1f;
            progressBar.value = 0f;
        }

        // Auto-find player if missing
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }
    }

    void OnEnable()
    {
        // Subscribe to action manager events
        if (PlayerActionManager.Instance != null)
        {
            PlayerActionManager.Instance.OnActionStarted += HandleActionStarted;
            PlayerActionManager.Instance.OnActionCompleted += HandleActionEnded;
            PlayerActionManager.Instance.OnActionCancelled += HandleActionEnded;
        }
    }

    void OnDisable()
    {
        // Clean up subscriptions
        if (PlayerActionManager.Instance != null)
        {
            PlayerActionManager.Instance.OnActionStarted -= HandleActionStarted;
            PlayerActionManager.Instance.OnActionCompleted -= HandleActionEnded;
            PlayerActionManager.Instance.OnActionCancelled -= HandleActionEnded;
        }
    }

    private void HandleActionStarted(ActionType action)
    {
        if (progressBar != null)
        {
            progressBar.value = 0f;
        }

        if (uiContainer != null)
        {
            uiContainer.SetActive(true);
        }
    }

    private void HandleActionEnded(ActionType action)
    {
        if (uiContainer != null)
        {
            uiContainer.SetActive(false);
        }
    }

    void Update()
    {
        // Fill the bar based on action progress (0 to 1)
        if (PlayerActionManager.Instance != null && PlayerActionManager.Instance.IsPerformingAction)
        {
            if (progressBar != null)
            {
                progressBar.value = PlayerActionManager.Instance.ActionProgress;
            }
        }
    }

    void LateUpdate()
    {
        // Continuously position the UI element over the player on the screen
        if (uiContainer != null && uiContainer.activeSelf && playerTransform != null && mainCam != null && rectTransform != null)
        {
            // Convert player's world position (plus offset) into screen space
            Vector3 screenPos = mainCam.WorldToScreenPoint(playerTransform.position + worldOffset);
            rectTransform.position = screenPos;
        }
    }
}
