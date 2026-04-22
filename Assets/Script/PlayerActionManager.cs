using System;
using System.Collections;
using UnityEngine;

public enum ActionType
{
    None,
    Dig,
    PlantSeed,
    Water,
    Harvest,
    PickUpItem,
    DropItem
}

public class PlayerActionManager : MonoBehaviour
{
    public static PlayerActionManager Instance { get; private set; }

    // Current action state (read-only from outside)
    public ActionType CurrentAction { get; private set; } = ActionType.None;
    public bool IsPerformingAction => CurrentAction != ActionType.None;

    // Progress (0 to 1) of the current action, useful for UI progress bars
    public float ActionProgress { get; private set; } = 0f;

    // Duration settings (editable in Inspector)
    [Header("Action Durations (seconds)")]
    public float digDuration = 1.5f;
    public float plantSeedDuration = 1.0f;
    public float waterDuration = 1.0f;
    public float harvestDuration = 1.2f;
    public float pickUpDuration = 0.5f;
    public float dropDuration = 0.5f;

    // Events for external systems (UI, audio, animation)
    public event Action<ActionType> OnActionStarted;
    public event Action<ActionType> OnActionCompleted;
    public event Action<ActionType> OnActionCancelled;

    // Reference to animator for action animation triggers
    private Animator animator;
    private Coroutine activeCoroutine;

    void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Attempt to start an action. Returns false if already performing another action.
    /// The onComplete callback fires ONLY when the action finishes successfully (not cancelled).
    /// </summary>
    public bool TryStartAction(ActionType action, Action onComplete = null)
    {
        // Reject if already busy
        if (IsPerformingAction)
        {
            Debug.Log($"[ActionManager] Rejected {action} — already performing {CurrentAction}");
            return false;
        }

        // Reject None action
        if (action == ActionType.None)
        {
            Debug.LogWarning("[ActionManager] Cannot start ActionType.None");
            return false;
        }

        float duration = GetDuration(action);
        CurrentAction = action;
        ActionProgress = 0f;

        // Set animator parameter if available
        if (animator != null)
        {
            animator.SetInteger("ActionType", (int)action);
            animator.SetBool("IsAction", true);
        }

        Debug.Log($"[ActionManager] Started {action} (duration: {duration}s)");
        OnActionStarted?.Invoke(action);

        activeCoroutine = StartCoroutine(PerformAction(duration, action, onComplete));
        return true;
    }

    /// <summary>
    /// Cancel the current action immediately. The onComplete callback will NOT fire.
    /// </summary>
    public void CancelAction()
    {
        if (!IsPerformingAction) return;

        ActionType cancelled = CurrentAction;

        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
            activeCoroutine = null;
        }

        ResetState();

        Debug.Log($"[ActionManager] Cancelled {cancelled}");
        OnActionCancelled?.Invoke(cancelled);
    }

    private IEnumerator PerformAction(float duration, ActionType action, Action onComplete)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            ActionProgress = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }

        // Action completed successfully
        ActionProgress = 1f;
        activeCoroutine = null;

        // Fire the completion callback BEFORE resetting state
        // so the callback can still read CurrentAction if needed
        onComplete?.Invoke();

        Debug.Log($"[ActionManager] Completed {action}");
        OnActionCompleted?.Invoke(action);

        ResetState();
    }

    private void ResetState()
    {
        CurrentAction = ActionType.None;
        ActionProgress = 0f;

        if (animator != null)
        {
            animator.SetInteger("ActionType", 0);
            animator.SetBool("IsAction", false);
        }
    }

    private float GetDuration(ActionType action)
    {
        switch (action)
        {
            case ActionType.Dig:         return digDuration;
            case ActionType.PlantSeed:   return plantSeedDuration;
            case ActionType.Water:       return waterDuration;
            case ActionType.Harvest:     return harvestDuration;
            case ActionType.PickUpItem:  return pickUpDuration;
            case ActionType.DropItem:    return dropDuration;
            default:                     return 0f;
        }
    }
}
