# Tower 11

Tower 11 is a 2D arcade survival game built with Unity. Players control a plane, avoid towers and incoming rockets, and survive as long as possible while the game increases in difficulty.

The project also includes an experimental Web3 ownership verification prototype that integrates MetaMask, Ethers.js, and NFT-based content unlocking within a Unity WebGL application.

---

## Technical Highlights

* Built a Unity WebGL ↔ JavaScript bridge using `.jslib` plugins
* Integrated MetaMask wallet connectivity
* Verified NFT ownership through smart contract `balanceOf()` calls
* Implemented wallet-based content unlocking
* Persisted wallet sessions across browser refreshes
* Deployed playable WebGL builds
* Connected blockchain ownership data directly to gameplay systems

---

## Play

| Version        | Link                                |
| -------------- | ----------------------------------- |
| Standard       | https://jerom001.itch.io/tower-11   |
| Web3 Prototype | https://jerom-26.github.io/Tower-11 |

---

## Screenshots

### Main Menu

<img src="https://github.com/user-attachments/assets/fc75f521-fa53-4e25-af50-d85489a33785" width="700"/>

### Wallet Connection Request

<img src="https://github.com/user-attachments/assets/5d190f5a-54bc-486f-9698-2707ec5775c7" width="700"/>

### Wallet Connected

<img src="https://github.com/user-attachments/assets/11fe5719-9c3c-46af-bbd5-8297e1a1ff74" width="700"/>

### Standard Gameplay

<img src="https://github.com/user-attachments/assets/c8e619b5-ef8b-4239-b157-5971336b4502" width="700"/>

### NFT Premium Skin Unlocked

<img src="https://github.com/user-attachments/assets/7df0cf0a-3526-4032-975c-c67884ef52b5" width="700"/>

---

## Gameplay

Navigate through an increasingly difficult city environment while avoiding towers and enemy rockets.

A collision with a tower or rocket ends the run.

### Controls

* Space / Click / Tap — Fly Up
* Release — Fall
* ESC — Pause

---

## Rocket Types

### Chase

Tracks the player's movement and attempts to intercept.

### Charge

Delays before attacking the player's last known position.

### Decoy

Travels in a straight path and acts as a visual distraction.

---

## Features

* 2D arcade survival gameplay
* Dynamic difficulty scaling
* Multiple rocket behaviors
* High score tracking
* Pause system
* Parallax backgrounds
* Username validation system
* Object pooling for performance
* WebGL deployment
* Experimental Web3 ownership verification

---

## Tech Stack

| Category        | Technology           |
| --------------- | -------------------- |
| Engine          | Unity                |
| Language        | C#                   |
| Platform        | WebGL                |
| Web3 Library    | Ethers.js            |
| Wallet          | MetaMask             |
| Blockchain      | Polygon Amoy Testnet |
| Version Control | Git & GitHub         |

---

## Web3 Ownership Verification Prototype

This project contains an experimental Web3 implementation designed to explore how external ownership data can be integrated into a Unity WebGL game.

Traditional games typically store ownership data inside centralized databases controlled by the developer. In this prototype, ownership is verified directly through a blockchain wallet.

The purpose of this implementation was to learn:

* Unity WebGL browser integration
* JavaScript ↔ C# communication
* MetaMask wallet connectivity
* Smart contract interaction
* NFT-based content unlocking
* Alternative ownership models for digital assets

---

## How It Works

1. The player connects a MetaMask wallet.
2. Unity calls a JavaScript WebGL plugin through `DllImport("__Internal")`.
3. The plugin communicates with MetaMask in the browser.
4. Ethers.js connects to the Polygon Amoy test network.
5. The game calls the NFT contract's `balanceOf()` function.
6. NFT ownership is verified.
7. Premium content is unlocked inside the game.

---

## Current Implementation

The current prototype unlocks a premium aircraft skin when NFT ownership is detected.

Ownership is linked directly to the connected wallet rather than a traditional game account.

The implementation demonstrates a complete ownership verification flow:

**MetaMask Connection → Wallet Verification → NFT Ownership Check → Premium Plane Skin Unlock**

This project is intentionally small in scope and serves as a proof of concept for wallet-based ownership verification inside a Unity game.

---

## Architecture

```text
Unity (C#)
    ↕
DllImport("__Internal")
    ↕
WebGL JavaScript Plugin (.jslib)
    ↕
MetaMask
    ↕
Ethers.js
    ↕
Polygon Amoy Testnet
    ↕
NFT Smart Contract
```

---

## Technical Challenges Solved

* Creating a Unity WebGL ↔ JavaScript communication bridge
* Using `DllImport("__Internal")` to call browser functions
* Passing wallet data from JavaScript back into Unity
* Persisting wallet sessions using browser storage
* Reading NFT ownership directly from a smart contract
* Unlocking gameplay content based on ownership verification
* Handling wallet connection failures and user rejection
* Synchronizing blockchain ownership data with game systems

---

## Project Status

This project is currently a technical proof of concept.

The gameplay experience is complete and playable, while the Web3 integration serves as an experimental ownership verification system.

---

## Developer

**Jerom Jiju**

GitHub: https://github.com/jerom-26

Itch.io: https://jerom001.itch.io
