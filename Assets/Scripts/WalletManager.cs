using UnityEngine;
using System.Runtime.InteropServices;

public class WalletManager : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void ConnectWallet();

    [DllImport("__Internal")]
    private static extern void RestoreWalletSession();
#endif

    public Web3Manager web3Manager;

    private string currentWallet = "";

    private void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        RestoreWalletSession();
#else
        if (web3Manager != null)
            web3Manager.SetDisconnected();
#endif
    }

    public void Connect()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        ConnectWallet();
#else
        Debug.Log("WebGL only");
#endif
    }

    // Called from JS
    public void OnWalletConnected(string address)
    {
        Debug.Log("Wallet Connected: " + address);
        currentWallet = address;

        if (web3Manager != null)
            web3Manager.OnWalletConnected(address);
    }

    // Called from JS
    public void OnNFTChecked(string balance)
    {
        Debug.Log("NFT balance from JS: " + balance);

        if (web3Manager != null)
            web3Manager.OnNFTChecked(balance);
    }

    // Called from JS if no valid wallet/session exists
    public void OnWalletDisconnected(string _)
    {
        Debug.Log("Wallet disconnected or no session");
        currentWallet = "";

        if (web3Manager != null)
            web3Manager.SetDisconnected();
    }
}