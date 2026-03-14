using UnityEngine;

public class WalletBridge : MonoBehaviour
{
    public NFTService nftService;

    public void OnWalletConnected(string walletAddress)
    {
        Debug.Log("Wallet connected: " + walletAddress);

        if (nftService != null)
        {
            StartCoroutine(CheckNFT(walletAddress));
        }
    }

    System.Collections.IEnumerator CheckNFT(string wallet)
    {
        var task = nftService.CheckNFT(wallet);

        while (!task.IsCompleted)
            yield return null;
    }
}