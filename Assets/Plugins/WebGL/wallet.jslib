mergeInto(LibraryManager.library, {
    ConnectWallet: function () {

        //  Check MetaMask
        if (typeof window.ethereum === "undefined") {
            alert("MetaMask not installed");
            return;
        }

        //  Check ethers
        if (typeof ethers === "undefined") {
            alert("ethers.js not loaded");
            return;
        }

        //  Request wallet connection
        window.ethereum.request({ method: "eth_requestAccounts" })
        .then(function (accounts) {

            var wallet = accounts[0];
            console.log("Wallet connected:", wallet);

            //  Send wallet to Unity
            if (window.unityInstance) {
                window.unityInstance.SendMessage(
                    "Web3Manager",
                    "OnWalletConnected",
                    wallet
                );
            }

            // Setup contract
	    var contractAddress = "0xF6c882D77918160D1D9c35dee0a5a56E9b6AC32C";
            var provider = new ethers.providers.Web3Provider(window.ethereum);

            var abi = [
                "function balanceOf(address owner) view returns (uint256)"
            ];

            var contract = new ethers.Contract(contractAddress, abi, provider);

            //  Check NFT balance
            return contract.balanceOf(wallet);
        })
        .then(function (balance) {

            console.log("NFT Balance:", balance.toString());

            //Send balance to Unity
            if (window.unityInstance) {
                window.unityInstance.SendMessage(
                    "Web3Manager",
                    "OnNFTChecked",
                    balance.toString()
                );
            } else {
                console.warn("Unity instance not ready");
            }
        })
        .catch(function (err) {

            console.error("Web3 error:", err);

            if (err.code === 4001) {
                alert("User rejected the connection");
            } else {
                alert("Web3 error: " + err.message);
            }
        });
    }
});