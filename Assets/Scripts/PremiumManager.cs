using UnityEngine;

public class PremiumManager : MonoBehaviour
{
    public SpriteRenderer planeSpriteRenderer;
    public Sprite normalPlaneSprite;
    public Sprite premiumPlaneSprite;

    public void ApplyPremium(bool hasNFT)
    {
        if (planeSpriteRenderer == null)
        {
            Debug.LogWarning("Plane SpriteRenderer not assigned.");
            return;
        }

        if (hasNFT && premiumPlaneSprite != null)
        {
            planeSpriteRenderer.sprite = premiumPlaneSprite;
            Debug.Log("Player owns NFT. Premium plane enabled.");
        }
        else if (normalPlaneSprite != null)
        {
            planeSpriteRenderer.sprite = normalPlaneSprite;
            Debug.Log("Player does not own NFT. Normal plane enabled.");
        }
    }
}