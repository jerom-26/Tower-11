using UnityEngine;
using Thirdweb;
using System.Numerics;
using System.Threading.Tasks;
using Thirdweb.Unity;

public class NFTChecker : MonoBehaviour
{
    public string contractAddress = "0xF6c82D77918160D1D9c35dee0a5a56E9b6AC32C";
    public int chainId = 80002;

    public bool hasPremiumPlane = false;

    public async Task CheckNFT(string walletAddress)
    {
        try
        {
            Debug.Log("Checking NFT for: " + walletAddress);

            var contract = await ThirdwebManager.Instance.GetContract(
                contractAddress,
                chainId
            );

            var balance = await contract.Read<System.Numerics.BigInteger>(
                "balanceOf",
                new object[] { walletAddress }
            );

            Debug.Log("NFT Balance: " + balance);

            hasPremiumPlane = balance > 0;

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