using UnityEngine;
using UnityEngine.UI;

public class LocalizedButtonSprite : MonoBehaviour
{
    public Image targetImage;
    public Button targetButton;

    public void SetNormalSprite(Sprite sprite)
    {
        if (targetImage != null)
            targetImage.sprite = sprite;
    }

    public void SetPressedSprite(Sprite sprite)
    {
        if (targetButton == null)
            return;

        SpriteState state = targetButton.spriteState;
        state.pressedSprite = sprite;
        targetButton.spriteState = state;
    }

    public void SetHighlightedSprite(Sprite sprite)
    {
        if (targetButton == null)
            return;

        SpriteState state = targetButton.spriteState;
        state.highlightedSprite = sprite;
        targetButton.spriteState = state;
    }

    public void SetSelectedSprite(Sprite sprite)
    {
        if (targetButton == null)
            return;

        SpriteState state = targetButton.spriteState;
        state.selectedSprite = sprite;
        targetButton.spriteState = state;
    }

    public void SetDisabledSprite(Sprite sprite)
    {
        if (targetButton == null)
            return;

        SpriteState state = targetButton.spriteState;
        state.disabledSprite = sprite;
        targetButton.spriteState = state;
    }
}