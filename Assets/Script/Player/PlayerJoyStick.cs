using UnityEngine;

public class PlayerJoyStick : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement; // ลากตัว Player มาใส่
    [SerializeField] private Joystick joystick;             // ลากตัว Joystick มาใส่

    void Update()
    {
        if (playerMovement != null && joystick != null)
        {
            // ส่งค่าจาก Joystick ไปให้ PlayerMovement
            playerMovement.moveInput = joystick.Direction;
        }
    }
}