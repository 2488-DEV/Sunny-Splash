using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource footstepSource;
    public AudioSource actionSource;
    public AudioSource QuackSource;
    public PlayerMovement playerMovement;

    [Header("Volume Controls")]
    [Range(0f, 1f)] public float footstepVolume = 0.2f;
    [Range(0f, 1f)] public float actionVolume = 0.8f;
    [Range(0f, 1f)] public float dieVolume = 1.0f;
    [Range(0f, 1f)] public float quackVolume = 0.8f;

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
    public AudioClip QuackSound;

    [Header("Pitch Settings")]
    public float walkPitch = 1.0f;
    public float runPitch = 1.6f;

    [Header("Quack Text")]
    public GameObject FloatingText;
    bool isQuack = false;
    private float timer = 0f;

    // ===== [ส่วนที่ 1: เพิ่มตัวแปรสำหรับระบบเรียกศัตรู] =====
    [Header("Enemy Alert Settings")]
    [Tooltip("ระยะทางที่ศัตรูจะได้ยินเสียงร้อง")]
    public float quackSoundRadius = 15f; 
    [Tooltip("เลือก Layer ของศัตรู (เช่น Enemy)")]
    public LayerMask enemyLayer; 
    // ==================================================

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

            case "MissionComplete":
                if (missionComplete != null)
                {
                    AudioSource.PlayClipAtPoint(missionComplete, Camera.main.transform.position, 1.0f);
                }
                return; 

            case "Die":
                if (dieSound != null)
                {
                    AudioSource.PlayClipAtPoint(dieSound, Camera.main.transform.position, dieVolume);
                }
                return;
        }

        if (clipToPlay != null && actionSource != null)
        {
            actionSource.PlayOneShot(clipToPlay, actionVolume);
        }
    }   

    void OnEnable() { GameInput.OnQuack += TryQuack; }
    void OnDisable() { GameInput.OnQuack -= TryQuack; }

    void TryQuack()
    {
        timer = 0f; // รีเซ็ตตัวจับเวลาเมื่อกด R
        isQuack = true;
        transform.localScale = new Vector3(transform.localScale.x,0.7f,transform.localScale.z);
        if (QuackSound != null)
        {   
            
            ShowFloatingText();
            if (QuackSource != null)
            {
                QuackSource.PlayOneShot(QuackSound, quackVolume);
            }
            else if (actionSource != null)
            {
                actionSource.PlayOneShot(QuackSound, quackVolume);
            }
            // ===== [ส่วนที่ 2: เพิ่มคำสั่งเรียกศัตรูเมื่อกด R] =====
            AlertNearbyEnemies();
            // ===================================================
        }
    }

    void Update()
    {   
        if (isQuack)
        {
            timer += Time.deltaTime;

            if (timer >= 0.2f)
            {
                transform.localScale = new Vector3(
                    transform.localScale.x,
                    1f,
                    transform.localScale.z
                );

                isQuack = false;
            }
        }

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

    void ShowFloatingText()
    {
        if (isQuack != false){
            Instantiate(FloatingText, transform.position, Quaternion.identity, transform);
        }
    }

    // ===== [ส่วนที่ 3: เพิ่มฟังก์ชันค้นหาและสั่งการศัตรู] =====
    void AlertNearbyEnemies()
{
    // เปลี่ยนเป็น Physics2D.OverlapCircle และใช้ Collider2D
    Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, quackSoundRadius, enemyLayer);
    
    Debug.Log("เจอวัตถุในเลเยอร์ศัตรู (2D) ทั้งหมด: " + hitColliders.Length + " ตัว");

    foreach (var enemyCollider in hitColliders)
    {
        SmartEnemyAI enemy = enemyCollider.GetComponent<SmartEnemyAI>();
        
        if (enemy != null)
        {
            Debug.Log("ส่งสัญญาณเสียงไปให้ศัตรูชื่อ: " + enemyCollider.name);
            enemy.ListenToSound(transform.position); 
        }
        else
        {
            Debug.LogWarning("เจอวัตถุ " + enemyCollider.name + " แต่ในตัวไม่มีสคริปต์ SmartEnemyAI!");
        }
    }
}

    // เปิดให้แสดงวงกลมรัศมีเสียงในหน้า Scene View ของ Unity (ช่วยให้ปรับแต่งง่ายขึ้น)
    void OnDrawGizmosSelected()
{
    Gizmos.color = Color.yellow;
    // วาดเส้นวงกลมแบบแบนราบสไตล์ 2D
    Matrix4x4 oldMatrix = Gizmos.matrix;
    Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, new Vector3(1, 1, 0));
    Gizmos.DrawWireSphere(Vector3.zero, quackSoundRadius);
    Gizmos.matrix = oldMatrix;
}
    // ===================================================

}
