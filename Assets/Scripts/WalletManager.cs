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
    private string lastNFTBalance = "";
    private string currentWallet = "";
    private bool isRestoring = false;

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
        FindSceneReferences();

#if UNITY_WEBGL && !UNITY_EDITOR
        RestoreOnce();
#else
        if (web3Manager != null)
            web3Manager.SetDisconnected();
#endif
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindSceneReferences();

        if (web3Manager != null)
        {
            if (!string.IsNullOrEmpty(currentWallet))
            {
                web3Manager.OnWalletConnected(currentWallet);

                if (!string.IsNullOrEmpty(lastNFTBalance))
                {
                    web3Manager.OnNFTChecked(lastNFTBalance);
                }
                else if (web3Manager.nftText != null)
                {
                    web3Manager.nftText.text = "NFT: Checking...";
                }
            }
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        RestoreOnce();
#endif
    }

    private void FindSceneReferences()
    {
        web3Manager = FindObjectOfType<Web3Manager>();

        connectButtonText = null;
        GameObject btnTextObj = GameObject.Find("ConnectWalletText");

        if (btnTextObj != null)
            connectButtonText = btnTextObj.GetComponent<Text>();

        UpdateConnectButtonText();
    }

    private void RestoreOnce()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
    if (isRestoring) return;

    isRestoring = true;
    RestoreWalletSession();
    Invoke(nameof(ResetRestoreLock), 1f);
#endif
    }

    private void ResetRestoreLock()
    {
        isRestoring = false;
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
        {
            web3Manager.OnWalletConnected(address);

            if (web3Manager.nftText != null)
                web3Manager.nftText.text = "NFT: Checking...";
        }

        UpdateConnectButtonText();
    }

    public void OnNFTChecked(string balance)
    {
        Debug.Log("NFT balance from JS: " + balance);

        lastNFTBalance = balance;

        if (web3Manager == null)
            web3Manager = FindObjectOfType<Web3Manager>();

        if (web3Manager != null)
            web3Manager.OnNFTChecked(balance);
    }

    public void OnWalletDisconnected(string _)
    {
        Debug.Log("Wallet disconnected or no session");
        currentWallet = "";

        if (web3Manager != null)
            web3Manager.SetDisconnected();

        UpdateConnectButtonText();
    }

    private void UpdateConnectButtonText()
    {
        if (connectButtonText == null) return;

        connectButtonText.text = string.IsNullOrEmpty(currentWallet)
            ? "CONNECT WALLET"
            : "CONNECTED";
    }
}