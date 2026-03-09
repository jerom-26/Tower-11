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
            var contract = await ThirdwebManager.Instance.GetContract(
                address: contractAddress,
                chainId: chainId
            );

            BigInteger balance = await contract.Read<BigInteger>(
                "balanceOf",
                walletAddress
            );

            hasPremiumPlane = balance > 0;

            if (hasPremiumPlane)
                Debug.Log("Premium Plane Unlocked");
            else
                Debug.Log("No NFT Found");
        }
        catch (System.Exception e)
        {
            Debug.LogError("NFT check failed: " + e.Message);
        }
    }
}