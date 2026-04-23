using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 moveInput;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public OverHeatBar overHeatBar;
    public SunSystem sunSystem;
    public StaminaBar staminaBar;

    public float speed = 5f;
    public float sprint = 3f;

    public bool isInShadow = false;
    public bool isInWater = false;
    public bool WaterWalking = false;
    public bool isPlayerRunning = false;

    private PlayerActionManager actionManager;

    void Start()
    {
        actionManager = GetComponent<PlayerActionManager>();
    }

    void Update()
    {
        if (actionManager != null && actionManager.IsPerformingAction)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("IsRunning", false);
            isPlayerRunning = false; // หยุดวิ่งทันทีเมื่อทำ Action
            return;
        }

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput = moveInput.normalized;

        // --- ส่วนที่ปรับปรุง: เช็ควิ่งและ Stamina ---
        // เปลี่ยนมาเช็ค currentStamina แทน slider.value เพื่อความเป๊ะกวัก!
        bool hasStamina = (staminaBar != null && staminaBar.currentStamina >= 1f);

        if (Input.GetKey(KeyCode.LeftShift) && hasStamina && moveInput != Vector2.zero)
        {
            rb.linearVelocity = moveInput * speed * sprint;
            isPlayerRunning = true;
            // เราลบบรรทัดหักค่าตรงนี้ออก เพราะเราจะไปหักใน StaminaBar.cs แทนเพื่อให้หลอดนิ่มกวัก!
        }
        else
        {
            rb.linearVelocity = moveInput * speed;
            isPlayerRunning = false;
        }

        // Animation Logic
        if (moveInput != Vector2.zero)
        {
            animator.SetBool("IsRunning", true);
            WaterWalking = true;
        }
        else
        {
            animator.SetBool("IsRunning", false);
            WaterWalking = false;
        }

        if (moveInput.x != 0)
        {
            spriteRenderer.flipX = moveInput.x < 0;
        }

        // Water Logic
        if (isInWater)
        {
            animator.SetBool("IsSwim", true);
            animator.SetBool("IsSwiming", WaterWalking);

            float waterSpeedMult = isPlayerRunning ? 0.4f : 0.25f;
            rb.linearVelocity = moveInput * (speed * (isPlayerRunning ? sprint : 1f)) * waterSpeedMult;
        }
        else
        {
            animator.SetBool("IsSwim", false);
            animator.SetBool("IsSwiming", false);
        }
    }
}