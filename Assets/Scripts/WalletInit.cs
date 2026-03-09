using UnityEngine;
using Reown.AppKit.Unity;
using Reown.AppKit.Unity.Model;

public class WalletInit : MonoBehaviour
{
    async void Start()
    {
        var config = new AppKitConfig(
            projectId: "c7a0318253675c5d16112a2fb26baeb9",
            metadata: new Metadata(
                name: "Tower 11",
                description: "Tower 11 Web3 version",
                url: "https://example.com",
                iconUrl: "https://example.com/icon.png"
            )
        );

        await AppKit.InitializeAsync(config);

        Debug.Log("Reown AppKit initialized.");
    }
}