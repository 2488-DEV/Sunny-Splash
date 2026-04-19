using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;        // ความเร็วตัวละคร
    public Rigidbody2D rb;
    public Vector2 moveInput;
    public float sprint = 3f;

    void Update()
    {
        // รับ input จากปุ่ม WASD หรือ Arrow keys
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // ป้องกันเดินเร็วขึ้นตอนเฉียง
        moveInput = moveInput.normalized;
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            rb.linearVelocity = moveInput * speed* sprint;
        }
        else
        {
            rb.linearVelocity = moveInput * speed;
        }
    }
}