using UnityEngine;

public class SimpleLayerTrigger : MonoBehaviour
{
    public string layerWhenEnter = "Layer3"; // ตอนเข้า (ขึ้น)
    public string layerWhenExit = "Layer2";  // ตอนออก (ลง)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SetLayer(collision.gameObject, layerWhenEnter);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SetLayer(collision.gameObject, layerWhenExit);
        }
    }

    void SetLayer(GameObject obj, string layerName)
    {
        SpriteRenderer[] srs = obj.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in srs)
        {
            sr.sortingLayerName = layerName;
        }
    }
}