using UnityEngine;
using System.Runtime.InteropServices;

public class WalletManager : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void ConnectWallet();
#endif

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

        // You can now:
        // - Store wallet
        // - Call NFT checker
        // - Unlock features
    }
}