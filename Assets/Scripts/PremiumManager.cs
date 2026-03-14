using UnityEngine;

public class PremiumManager : MonoBehaviour
{
    public void ApplyPremium(bool hasNFT)
    {
        if (hasNFT)
        {
            Debug.Log("Player owns NFT. Premium features enabled.");
        }
        else
        {
            Debug.Log("Player does not own NFT.");
        }
    }
}