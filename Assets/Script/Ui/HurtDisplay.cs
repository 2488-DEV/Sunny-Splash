using UnityEngine;
using UnityEngine.UI;

public class HurtDisplay : MonoBehaviour
{
    public PlayerScript playerScript;
    private RawImage rawImage;

    void Start()
    {
        // ดึง Component จาก GameObject เดียวกัน
        rawImage = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.playerHp == 2)
        {
            SetAlpha(0.3f);
        }
        else if (playerScript.playerHp == 1)
        {
            SetAlpha(0.6f);
        }
    }

    public void SetAlpha(float alphaValue) 
    {
        Color newColor = rawImage.color;
        
        newColor.a = alphaValue;

        rawImage.color = newColor;
    }
}
