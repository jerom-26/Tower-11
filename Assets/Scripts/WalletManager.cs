using UnityEngine;
using System.Runtime.InteropServices;

public class WalletManager : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void ConnectWallet();
#endif

    public void ConnectWalletButton()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        ConnectWallet();
#else
        Debug.Log("Wallet works only in WebGL build.");
#endif
    }
}