using UnityEngine;

public class TextFloating : MonoBehaviour
{
    public float DestroyTime = 3f;
    public Vector3 Offset = new Vector3(0,1,1);
    public Vector3 RandomIntensity = new Vector3(0.5f,0,0);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("QUACK");
        Destroy(gameObject,DestroyTime);
        transform.localPosition += Offset;
        transform.localPosition += new Vector3(Random.Range(-RandomIntensity.x,RandomIntensity.x),Random.Range(-RandomIntensity.y,RandomIntensity.y),Random.Range(-RandomIntensity.z,RandomIntensity.z));
    }
}