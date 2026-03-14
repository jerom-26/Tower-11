mergeInto(LibraryManager.library, {
    ConnectWallet: async function () {

        if (typeof window.ethereum === "undefined") {
            alert("MetaMask not installed");
            return;
        }

        const accounts = await window.ethereum.request({
            method: "eth_requestAccounts"
        });

        const wallet = accounts[0];

        SendMessage("Web3Manager", "OnWalletConnected", wallet);

        // --- NFT CHECK ---
        const contractAddress = "0xF6c82D77918160D1D9c35dee0a5a56E9b6AC32C";

        const provider = new ethers.providers.Web3Provider(window.ethereum);

        const abi = [
            "function balanceOf(address owner) view returns (uint256)"
        ];

        const contract = new ethers.Contract(contractAddress, abi, provider);

        const balance = await contract.balanceOf(wallet);

        SendMessage("Web3Manager", "OnNFTChecked", balance.toString());
    }
});