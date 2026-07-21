using UnityEngine;
using System;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance;
    
    public static event Action OnInteract; 
    public static event Action OnPickUp;   
    public static event Action OnDrop;
    public static event Action OnQuack;
    public static event Action OnEgg;

    void Awake() 
    { 
        Instance = this; 
    }

    public void RequestInteraction() { OnInteract?.Invoke(); }
    public void RequestPickUp()      { OnPickUp?.Invoke(); }
    public void RequestDrop()        { OnDrop?.Invoke(); }
    public void RequestQuack()      { OnQuack?.Invoke(); }
    public void RequestEgg()      { OnEgg?.Invoke(); }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) OnInteract?.Invoke();
        if (Input.GetKeyDown(KeyCode.F))     OnPickUp?.Invoke();
        if (Input.GetKeyDown(KeyCode.Q))     OnDrop?.Invoke();
        if (Input.GetKeyDown(KeyCode.R))     OnQuack?.Invoke();
        if (Input.GetKeyDown(KeyCode.E))     OnEgg?.Invoke();
    }
}