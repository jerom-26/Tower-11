mergeInto(LibraryManager.library, {
    ConnectWallet: function () {
        if (typeof window.ethereum === "undefined") {
            alert("MetaMask not installed");
            return;
        }

        if (typeof ethers === "undefined") {
            alert("ethers.js not loaded");
            return;
        }

        window.ethereum.request({ method: "eth_requestAccounts" })
            .then(function (accounts) {
                if (!accounts || accounts.length === 0) {
                    throw new Error("No wallet account found");
                }

                var wallet = accounts[0];
                localStorage.setItem("walletAddress", wallet);

                if (window.unityInstance) {
                    window.unityInstance.SendMessage(
                        "WalletManager",
                        "OnWalletConnected",
                        wallet
                    );
                }

                return checkNFT(wallet);
            })
            .then(function (balance) {
                if (window.unityInstance) {
                    window.unityInstance.SendMessage(
                        "WalletManager",
                        "OnNFTChecked",
                        balance.toString()
                    );
                }
            })
            .catch(function (err) {
                console.error("Web3 error:", err);

                if (err.code === 4001) {
                    alert("User rejected the connection");
                } else {
                    alert("Web3 error: " + err.message);
                }

                if (window.unityInstance) {
                    window.unityInstance.SendMessage(
                        "WalletManager",
                        "OnWalletDisconnected",
                        ""
                    );
                }
            });
    },

    RestoreWalletSession: function () {
        if (typeof window.ethereum === "undefined" || typeof ethers === "undefined") {
            if (window.unityInstance) {
                window.unityInstance.SendMessage(
                    "WalletManager",
                    "OnWalletDisconnected",
                    ""
                );
            }
            return;
        }

        var savedWallet = localStorage.getItem("walletAddress");

        window.ethereum.request({ method: "eth_accounts" })
            .then(function (accounts) {
                if (!accounts || accounts.length === 0) {
                    localStorage.removeItem("walletAddress");

                    if (window.unityInstance) {
                        window.unityInstance.SendMessage(
                            "WalletManager",
                            "OnWalletDisconnected",
                            ""
                        );
                    }
                    return null;
                }

                var currentWallet = accounts[0];

                if (savedWallet && savedWallet.toLowerCase() !== currentWallet.toLowerCase()) {
                    localStorage.setItem("walletAddress", currentWallet);
                }

                if (window.unityInstance) {
                    window.unityInstance.SendMessage(
                        "WalletManager",
                        "OnWalletConnected",
                        currentWallet
                    );
                }

                return checkNFT(currentWallet);
            })
            .then(function (balance) {
                if (balance != null && window.unityInstance) {
                    window.unityInstance.SendMessage(
                        "WalletManager",
                        "OnNFTChecked",
                        balance.toString()
                    );
                }
            })
            .catch(function (err) {
                console.error("Restore session error:", err);
                localStorage.removeItem("walletAddress");

                if (window.unityInstance) {
                    window.unityInstance.SendMessage(
                        "WalletManager",
                        "OnWalletDisconnected",
                        ""
                    );
                }
            });
    }
});

function checkNFT(wallet) {
    var contractAddress = "0xF6c882D77918160D1D9c35dee0a5a56E9b6AC32C";
    var provider = new ethers.providers.Web3Provider(window.ethereum);
    var abi = [
        "function balanceOf(address owner) view returns (uint256)"
    ];
    var contract = new ethers.Contract(contractAddress, abi, provider);
    return contract.balanceOf(wallet);
}