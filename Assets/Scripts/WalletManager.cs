using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WalletManager : MonoBehaviour
{
    private static WalletManager instance;

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void ConnectWallet();

    [DllImport("__Internal")]
    private static extern void RestoreWalletSession();
#endif

    public Web3Manager web3Manager;
    public Text connectButtonText;

    private string currentWallet = "";

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        RestoreWalletSession();
#else
        if (web3Manager != null)
            web3Manager.SetDisconnected();
#endif
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        web3Manager = FindObjectOfType<Web3Manager>();

        if (web3Manager != null && !string.IsNullOrEmpty(currentWallet))
        {
            web3Manager.OnWalletConnected(currentWallet);
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        RestoreWalletSession();
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

    public void OnWalletConnected(string address)
    {
        Debug.Log("Wallet Connected: " + address);
        currentWallet = address;
        if (web3Manager != null)
            web3Manager.OnWalletConnected(address);
        if (connectButtonText != null)
            connectButtonText.text = "CONNECTED";
    }

    public void OnNFTChecked(string balance)
    {
        Debug.Log("NFT balance from JS: " + balance);
        if (web3Manager != null)
            web3Manager.OnNFTChecked(balance);
    }

    public void OnWalletDisconnected(string _)
    {
        Debug.Log("Wallet disconnected or no session");
        currentWallet = "";
        if (web3Manager != null)
            web3Manager.SetDisconnected();
        if (connectButtonText != null)
            connectButtonText.text = "CONNECT WALLET";
    }
}