using UnityEngine;

public class StairLayer_2 : MonoBehaviour
{
    public string layerUp = "Layer 3";     // ตอนขึ้น
    public string layerDown = "Layer 2";   // ตอนลง

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // ถ้า player อยู่ "สูงกว่า" trigger = ขึ้น
            if (collision.transform.position.y > transform.position.y)
            {
                SetSortingLayer(collision.gameObject, layerUp);
            }
            else // อยู่ต่ำกว่า = ลง
            {
                SetSortingLayer(collision.gameObject, layerDown);
            }
        }
    }

    void SetSortingLayer(GameObject obj, string layerName)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingLayerName = layerName;
        }
    }
}