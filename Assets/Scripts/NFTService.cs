using UnityEngine;
using Thirdweb;
using System.Collections;
using Thirdweb.Unity;

public class NFTService : MonoBehaviour
{
    public string contractAddress = "0xf6c82d77918160d1d9c35dee0a5a56e9b6ac32c";
    public int chainId = 80002;

    public bool hasPremiumPlane = false;

    public PremiumManager premiumManager;

    // 👇 THIS IS WHAT UNITY WILL CALL
    public IEnumerator CheckNFT(string walletAddress)
    {
        var task = CheckNFTAsync(walletAddress);

        while (!task.IsCompleted)
            yield return null;

        if (task.Exception != null)
        {
            Debug.LogError("NFT check failed: " + task.Exception);
        }
    }

    // YOUR ORIGINAL LOGIC (SAFE)
    private async System.Threading.Tasks.Task CheckNFTAsync(string walletAddress)
    {
        try
        {
            Debug.Log("Checking NFT for: " + walletAddress);

            var contract = await ThirdwebManager.Instance.GetContract(
                contractAddress,
                chainId
            );

            var balanceString = await contract.Read<string>(
                "balanceOf",
                new object[] { walletAddress }
            );

            System.Numerics.BigInteger balance = 0;
            System.Numerics.BigInteger.TryParse(balanceString, out balance);

            Debug.Log("NFT Balance: " + balance);

            hasPremiumPlane = balance > 0;

            if (premiumManager != null)
            {
                premiumManager.ApplyPremium(hasPremiumPlane);
            }

            if (hasPremiumPlane)
                Debug.Log(" Premium plane unlocked");
            else
                Debug.Log(" Player does not own NFT");
        }
        catch (System.Exception e)
        {
            Debug.LogError("NFT check failed: " + e.Message);
        }
    }
}