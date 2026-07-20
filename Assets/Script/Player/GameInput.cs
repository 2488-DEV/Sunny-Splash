using UnityEngine;
using System;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance;
    
    public static event Action OnInteract; 
    public static event Action OnPickUp;   
    public static event Action OnDrop;    // เพิ่มอันนี้ครับ

    void Awake() 
    { 
        Instance = this; 
    }

    public void RequestInteraction() { OnInteract?.Invoke(); }
    public void RequestPickUp()      { OnPickUp?.Invoke(); }
    public void RequestDrop()        { OnDrop?.Invoke(); } // เพิ่มอันนี้ครับ

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) OnInteract?.Invoke();
        if (Input.GetKeyDown(KeyCode.F))     OnPickUp?.Invoke();
        if (Input.GetKeyDown(KeyCode.Q))     OnDrop?.Invoke(); // รองรับปุ่ม Q
    }
}