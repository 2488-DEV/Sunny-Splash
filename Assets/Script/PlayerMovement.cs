using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 moveInput;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public OverHeatBar overHeatBar;
    public SunSystem sunSystem;

    public float speed = 5f;        // ความเร็วตัวละคร
    public float sprint = 3f;

    public bool isInShadow = false;
    public bool isInWater = false;

    void Update()
    {
        // รับ input จากปุ่ม WASD หรือ Arrow keys
        moveInput.x = Input.GetAxisRaw("Horizontal"); //รับค่าแกน X
        moveInput.y = Input.GetAxisRaw("Vertical"); //รับค่าแกน Y

        // ป้องกันเดินเร็วขึ้นตอนเฉียง
        moveInput = moveInput.normalized;
        
        if (Input.GetKey(KeyCode.LeftShift)) //วิ่ง
        {
            rb.linearVelocity = moveInput * speed* sprint;
        }

        else //เดิน
        {
            rb.linearVelocity = moveInput * speed;
        }

        if (moveInput != Vector2.zero) //animation วิ่ง
        {
            animator.SetBool("IsRunning",true);
        }

        else //animation เดิน
        {
            animator.SetBool("IsRunning",false);
        }

        if (moveInput.x != 0) //flip
        {
        spriteRenderer.flipX = moveInput.x < 0;
        }

        animator.SetBool("IsSwim", isInWater); //animation ว่ายน้ำ

        if (isInWater)
        {
            rb.linearVelocity = moveInput * (speed * 0.75f);
        }
    }
}