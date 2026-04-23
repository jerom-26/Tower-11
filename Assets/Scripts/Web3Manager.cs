using UnityEngine;
using UnityEngine.UI;

public class Web3Manager : MonoBehaviour
{
    public PremiumManager premiumManager;
    public Text walletText;
    public Text nftText;

    public void SetDisconnected()
    {
        if (walletText != null)
            walletText.text = "Wallet: Not Connected";

        if (nftText != null)
            nftText.text = "NFT: Not Checked";

        if (premiumManager != null)
            premiumManager.ApplyPremium(false);
    }

    public void OnWalletConnected(string wallet)
    {
        Debug.Log("Wallet connected: " + wallet);

        if (!string.IsNullOrEmpty(wallet) && wallet.Length > 10)
        {
            string shortAddress = wallet.Substring(0, 6) + "..." + wallet.Substring(wallet.Length - 4);

            if (walletText != null)
                walletText.text = "Wallet: " + shortAddress;
        }
    }

    public void OnNFTChecked(string balance)
    {
        Debug.Log("NFT Balance: " + balance);

        int parsedBalance = 0;
        int.TryParse(balance, out parsedBalance);

        bool hasNFT = parsedBalance > 0;

        if (premiumManager != null)
            premiumManager.ApplyPremium(hasNFT);

        if (nftText != null)
            nftText.text = hasNFT ? "NFT: Detected" : "NFT: Not Found";

        Debug.Log(hasNFT ? "Premium plane unlocked" : "Player does not own NFT");
    }
}