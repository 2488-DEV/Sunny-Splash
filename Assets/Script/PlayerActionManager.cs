using System;
using System.Collections;
using UnityEngine;

// --- 1. ประกาศประเภท Action ไว้ตรงนี้ (Enum) ---
public enum ActionType
{
    None,
    Dig,
    PlantSeed,
    Water,
    Harvest,
    PickUpItem,
    DropItem,
    RefillWater // ตัวที่เราเพิ่มเพื่อดูดน้ำกวัก!
}

// --- 2. ตัวจัดการ Action (Class) ---
public class PlayerActionManager : MonoBehaviour
{
    public static PlayerActionManager Instance { get; private set; }

    public ActionType CurrentAction { get; private set; } = ActionType.None;
    public bool IsPerformingAction => CurrentAction != ActionType.None;
    public float ActionProgress { get; private set; } = 0f;

    [Header("Action Durations (Seconds)")]
    public float digDuration = 1.5f;
    public float plantSeedDuration = 1.0f;
    public float waterDuration = 1.0f;
    public float harvestDuration = 1.2f;
    public float refillWaterDuration = 1.2f;

    public event Action<ActionType> OnActionStarted;
    public event Action<ActionType> OnActionCompleted;
    public event Action<ActionType> OnActionCancelled;

    private Animator animator;
    private Coroutine activeCoroutine;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); return; }
        Instance = this;
        animator = GetComponent<Animator>();
    }

    public bool TryStartAction(ActionType action, Action onComplete = null)
    {
        if (IsPerformingAction || action == ActionType.None) return false;

        float duration = GetDuration(action);
        CurrentAction = action;
        ActionProgress = 0f;

        // ส่ง Trigger ไปที่ Animator อัตโนมัติ (ชื่อต้องตรงกับใน Enum นะนาย)
        if (animator != null)
        {
            animator.SetTrigger(action.ToString());
        }

        OnActionStarted?.Invoke(action);
        activeCoroutine = StartCoroutine(PerformAction(duration, action, onComplete));
        return true;
    }

    public void CancelCurrentAction()
    {
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
            activeCoroutine = null;
            ActionType cancelledAction = CurrentAction;
            ResetState();
            OnActionCancelled?.Invoke(cancelledAction);
            Debug.Log("Action " + cancelledAction + " ถูกยกเลิกกวัก!");
        }
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

        activeCoroutine = null;
        onComplete?.Invoke();
        OnActionCompleted?.Invoke(action);
        ResetState();
    }

    private void ResetState()
    {
        CurrentAction = ActionType.None;
        ActionProgress = 0f;
    }

    private float GetDuration(ActionType action)
    {
        switch (action)
        {
            case ActionType.Dig: return digDuration;
            case ActionType.PlantSeed: return plantSeedDuration;
            case ActionType.Water: return waterDuration;
            case ActionType.Harvest: return harvestDuration;
            case ActionType.RefillWater: return refillWaterDuration;
            default: return 0.5f;
        }
    }
}