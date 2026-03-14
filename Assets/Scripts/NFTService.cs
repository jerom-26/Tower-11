using UnityEngine;
using Thirdweb;
using System.Threading.Tasks;
using Thirdweb.Unity;

public class NFTService : MonoBehaviour
{
    public string contractAddress = "0xF6c82D77918160D1D9c35dee0a5a56E9b6AC32C";
    public int chainId = 80002;

    public bool hasPremiumPlane = false;

    public PremiumManager premiumManager;

    public async Task CheckNFT(string walletAddress)
    {
        try
        {
            Debug.Log("Checking NFT for: " + walletAddress);

            var contract = await ThirdwebManager.Instance.GetContract(
                contractAddress,
                chainId
            );

            // Read balance as string (safe for WebGL)
            var balanceString = await contract.Read<string>(
                "balanceOf",
                new object[] { walletAddress }
            );

            // Safely convert to BigInteger
            System.Numerics.BigInteger balance = 0;
            System.Numerics.BigInteger.TryParse(balanceString, out balance);

            Debug.Log("NFT Balance: " + balance);

            hasPremiumPlane = balance > 0;

            // Apply premium logic
            if (premiumManager != null)
            {
                premiumManager.ApplyPremium(hasPremiumPlane);
            }

            if (hasPremiumPlane)
                Debug.Log("Premium plane unlocked");
            else
                Debug.Log("Player does not own NFT");
        }
        catch (System.Exception e)
        {
            Debug.LogError("NFT check failed: " + e.Message);
        }
    }
}