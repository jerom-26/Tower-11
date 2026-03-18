using UnityEngine;

public class WalletBridge : MonoBehaviour
{
    public Web3Manager web3Manager;

    public void OnWalletConnected(string walletAddress)
    {
        Debug.Log("Wallet connected: " + walletAddress);

        if (web3Manager != null)
        {
            web3Manager.OnWalletConnected(walletAddress);
        }
    }

    public void OnNFTChecked(string balance)
    {
        Debug.Log("NFT balance from JS: " + balance);

        if (web3Manager != null)
        {
            web3Manager.OnNFTChecked(balance);
        }
    }
}
