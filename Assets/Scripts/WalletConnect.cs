using UnityEngine;
using Reown.AppKit.Unity;
using static Reown.AppKit.Unity.Connector;

public class WalletConnect : MonoBehaviour
{
    public string walletAddress;
    public NFTChecker nftChecker;

    private bool eventRegistered = false;

    public async void ConnectWallet()
    {
        while (!AppKit.IsInitialized)
        {
            await System.Threading.Tasks.Task.Delay(100);
        }

        if (!eventRegistered)
        {
            AppKit.AccountConnected += OnAccountConnected;
            eventRegistered = true;
        }

        Debug.Log("Opening wallet modal...");
        AppKit.OpenModal();
    }

    private async void OnAccountConnected(object sender, AccountConnectedEventArgs e)
    {
        walletAddress = e.Account.Address;
        Debug.Log("Wallet connected: " + walletAddress);

        if (nftChecker != null)
        {
            await nftChecker.CheckNFT(walletAddress);
        }
    }

    private void OnDestroy()
    {
        if (eventRegistered)
        {
            AppKit.AccountConnected -= OnAccountConnected;
        }
    }
}