using UnityEngine;

public class TextFloating : MonoBehaviour
{
    public float DestroyTime = 3f;
    public Vector3 Offset = new Vector3(0,1,1);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject,DestroyTime);
        transform.localPosition += Offset;
    }
}
