using UnityEngine;
using Unity.Cinemachine;

public class CameraZoom : MonoBehaviour
{
    public CinemachineCamera cam;

    public float zoomInSize = 3.5f;
    public float zoomOutSize = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cam.Lens.OrthographicSize = zoomInSize;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cam.Lens.OrthographicSize = zoomOutSize;
        }
    }
}