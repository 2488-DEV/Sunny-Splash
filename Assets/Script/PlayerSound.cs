using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource footstepSource;
    public AudioSource actionSource;
    public PlayerMovement playerMovement;

    [Header("Volume Controls")]
    [Range(0f, 1f)] public float footstepVolume = 0.2f;
    [Range(0f, 1f)] public float actionVolume = 0.8f;
    [Range(0f, 1f)] public float dieVolume = 1.0f;

    [Header("Movement Clips")]
    public AudioClip walkSound;
    public AudioClip swimSound;
    public AudioClip enterWaterSound;
    public AudioClip exitWaterSound;

    [Header("Action Clips")]
    public AudioClip digSound;
    public AudioClip plantSound;
    public AudioClip waterSound;
    public AudioClip actionSuccess;
    public AudioClip missionComplete;
    public AudioClip dieSound;

    [Header("Pitch Settings")]
    public float walkPitch = 1.0f;
    public float runPitch = 1.6f;

    private bool wasInWater = false;

    public void PlayActionSound(string actionName)
    {
        AudioClip clipToPlay = null;

        switch (actionName)
        {
            case "Dig": clipToPlay = digSound; break;
            case "Plant": clipToPlay = plantSound; break;
            case "Water": clipToPlay = waterSound; break;
            case "Success": clipToPlay = actionSuccess; break;
            case "MissionComplete": clipToPlay = missionComplete; break;
            case "Die":
                if (dieSound != null)
                {
                    // --- เล่นครั้งเดียว แต่เล่นที่ตำแหน่งกล้องเพื่อให้ดังที่สุดกวัก! ---
                    AudioSource.PlayClipAtPoint(dieSound, Camera.main.transform.position, dieVolume);
                }
                return;
        }

        if (clipToPlay != null && actionSource != null)
        {
            actionSource.PlayOneShot(clipToPlay, actionVolume);
        }
    }

    void Update()
    {
        if (playerMovement == null || footstepSource == null) return;

        footstepSource.volume = footstepVolume;

        if (playerMovement.isInWater && !wasInWater)
        {
            footstepSource.PlayOneShot(enterWaterSound, footstepVolume);
            wasInWater = true;
        }
        else if (!playerMovement.isInWater && wasInWater)
        {
            footstepSource.PlayOneShot(exitWaterSound, footstepVolume);
            wasInWater = false;
        }

        float speed = (playerMovement.rb != null) ? playerMovement.rb.linearVelocity.magnitude : 0f;
        bool isMoving = speed > 0.1f;

        if (isMoving)
        {
            if (!footstepSource.isPlaying) footstepSource.Play();
            AudioClip targetClip = playerMovement.isInWater ? swimSound : walkSound;
            if (footstepSource.clip != targetClip)
            {
                footstepSource.clip = targetClip;
                footstepSource.Play();
            }
            footstepSource.pitch = playerMovement.isPlayerRunning ? runPitch : walkPitch;
        }
        else
        {
            if (footstepSource.isPlaying) footstepSource.Stop();
        }
    }
}