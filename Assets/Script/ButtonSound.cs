using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clickSfx;

    public void PlayClick()
    {
        source.PlayOneShot(clickSfx);
    }
}