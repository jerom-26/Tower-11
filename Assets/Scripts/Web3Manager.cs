using UnityEngine;

public class Web3Manager : MonoBehaviour
{
    public PremiumManager premiumManager;

    public void OnWalletConnected(string wallet)
    {
        Debug.Log("Wallet connected: " + wallet);
    }

    public void OnNFTChecked(string balance)
    {
        Debug.Log("NFT Balance: " + balance);

        int b = 0;
        int.TryParse(balance, out b);

        bool hasNFT = b > 0;

        if (premiumManager != null)
        {
            premiumManager.ApplyPremium(hasNFT);
        }

        if (hasNFT)
            Debug.Log("Premium plane unlocked");
        else
            Debug.Log("Player does not own NFT");
    }
}