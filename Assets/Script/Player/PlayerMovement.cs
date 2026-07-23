using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Joystick joystick;
    public Vector2 moveInput;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public OverHeatBar overHeatBar;
    public SunSystem sunSystem;
    public StaminaBar staminaBar;
    public VNDialogue dialogueManager;

    public float speed = 5f;
    public float sprint = 3f;

    public bool isInShadow = false;
    public bool isInWater = false;
    public bool WaterWalking = false;
    public bool isPlayerRunning = false;
    public static bool isToggleRunning = false;

    private PlayerActionManager actionManager;

    void Start()
    {
        actionManager = GetComponent<PlayerActionManager>();
    }

    public void ToggleSprint()
    {
        isToggleRunning = !isToggleRunning; // สลับค่า (True เป็น False / False เป็น True)
        Debug.Log("Sprint Mode: " + isToggleRunning);
    }
    void Update()
    {
        if (dialogueManager.isDialogue) 
        {
            animator.SetBool("IsRunning", false);
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (actionManager != null && actionManager.IsPerformingAction)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("IsRunning", false);
            isPlayerRunning = false;
            return;
        }

        Vector2 joystickInput = Vector2.zero;
        // สมมติว่าเจมส์มีตัวแปรชื่อ joystick ให้เรียกใช้แบบนี้ (ถ้าชื่ออื่นให้เปลี่ยนครับ)
        if (joystick != null) 
        {
            joystickInput = joystick.Direction;
        }

        if (joystickInput.magnitude > 0.1f)
        {
            moveInput = joystickInput;
        }
        else
        {
            // ถ้าไม่ใช้จอยสติ๊ก ให้ใช้คีย์บอร์ดแทน
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
        }

        moveInput = moveInput.normalized;

        bool runRequested = Input.GetKey(KeyCode.LeftShift) || isToggleRunning;
        bool hasStamina = (staminaBar != null && staminaBar.currentStamina >= 1f);

        if (runRequested && hasStamina && moveInput != Vector2.zero)
        {
            rb.linearVelocity = moveInput * speed * sprint;
            isPlayerRunning = true;

            if (staminaBar != null)
            {
                staminaBar.currentStamina -= 10f * Time.deltaTime;
            }
        }
        else
        {
            rb.linearVelocity = moveInput * speed;
            isPlayerRunning = false;
        }

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

    // --- ส่วนอัปเกรด: รองรับทั้งน้ำสะอาด (WaterSource) และน้ำเสีย (ContaminatedWater) กวัก! ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ตรวจสอบ Tag ให้ตรงกับใน Unity Inspector ของนายกวัก!
        if (other.CompareTag("WaterSource") || other.CompareTag("ContaminatedWater") || other.CompareTag("Water"))
        {
            isInWater = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // เมื่อเดินออกจากน้ำทุกประเภท ให้ยกเลิกสถานะว่ายน้ำกวัก
        if (other.CompareTag("WaterSource") || other.CompareTag("ContaminatedWater") || other.CompareTag("Water"))
        {
            isInWater = false;
        }
    }
}