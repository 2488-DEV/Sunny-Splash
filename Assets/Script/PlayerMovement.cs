using JetBrains.Annotations;
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

    public float speed = 5f;        // ความเร็วตัวละคร
    public float sprint = 3f;

    public bool isInShadow = false;
    public bool isInWater = false;
    public bool WaterWalking = false;

    private PlayerActionManager actionManager;

    void Start()
    {
        actionManager = GetComponent<PlayerActionManager>();
    }

    void Update()
    {
        // Block all movement and input during actions
        if (actionManager != null && actionManager.IsPerformingAction)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("IsRunning", false);
            return;
        }

        // รับ input จากปุ่ม WASD หรือ Arrow keys
        moveInput.x = Input.GetAxisRaw("Horizontal"); //รับค่าแกน X
        moveInput.y = Input.GetAxisRaw("Vertical"); //รับค่าแกน Y

        // ป้องกันเดินเร็วขึ้นตอนเฉียง
        moveInput = moveInput.normalized;
        
        if (Input.GetKey(KeyCode.LeftShift)) //วิ่ง
        {
            rb.linearVelocity = moveInput * speed* sprint;
            staminaBar.slider.value -= 0.1f;
        }

        else //เดิน
        {
            rb.linearVelocity = moveInput * speed;
        }

        if (moveInput != Vector2.zero) //animation วิ่ง
        {
            animator.SetBool("IsRunning",true);
            WaterWalking = true;
        }

        else //animation เดิน
        {
            animator.SetBool("IsRunning",false);
            WaterWalking = false;
        }

        if (moveInput.x != 0) //flip
        {
        spriteRenderer.flipX = moveInput.x < 0;
        }

        if (isInWater)
        {
            animator.SetBool("IsSwim", isInWater);
            animator.SetBool("IsSwiming", WaterWalking);
            rb.linearVelocity = moveInput * (speed * 0.25f);

            if (Input.GetKey(KeyCode.LeftShift)) //วิ่ง
            {
                rb.linearVelocity = moveInput * speed* sprint *0.4f;
            }
        }

        else
        {
            animator.SetBool("IsSwim", false);
            animator.SetBool("IsSwiming", false);
        }
    }
}