# 🎮 Tower 11

Tower 11 is a fast-paced 2D arcade survival game built with Unity. Control a plane flying through a scrolling cityscape, dodge deadly towers and unpredictable rockets, and survive as long as possible to climb the global leaderboard.

---

## 🕹️ Gameplay Preview

**Simple to play. Hard to master.**

- Fly continuously through a dynamic city  
- Avoid towers and guided rockets  
- One mistake = instant game over  
- Difficulty increases the longer you survive
<p align="center">
  <img src="https://github.com/user-attachments/assets/a0167784-5baa-4d5b-93a0-8ae717a53841" width="700"/>
  <br/>
</p>

---

## 🎯 Core Gameplay

### Controls
- Tap / Click / Space → Fly up  
- Release → Fall down  
- ESC → Pause menu  

### Scoring
- Pass towers to increase score  
- Higher score = higher difficulty  
- New personal bests are saved automatically  

---

## 🚀 Rocket Types

All rockets share the same visual design — their behavior is what makes them dangerous 👀  

- 🎯 **Chase** — Follows the player  
- ⏳ **Charge** — Waits, then attacks  
- ➡️ **Decoy** — Flies straight  
- 🟢 **Dummy** — Harmless (does not kill)  

---

## ✨ Features

- Smooth and responsive 2D controls  
- Dynamic difficulty scaling  
- Parallax scrolling background  
- Pause menu with How To Play guide  
- Local username system with validation  
- Persistent high scores  
- Online leaderboard integration

---

## 🌐 Online Leaderboard (Supabase)

- Players register a username once  
- Scores sync only when a new personal best is reached  
- Global leaderboard fetched in real time  

### Username Rules
- Lowercase only (a–z)  
- Numbers allowed (0–9)  
- Underscore allowed (_)  
- Length limits enforced  

---

## 🔗 Web3 Integration (Experimental)

Tower 11 includes a Web3 layer that connects gameplay with blockchain-based ownership and identity.

### 🔐 Wallet Connection
- Connect wallet through browser (WebGL build)
- Wallet acts as a unique player identity

### 🧾 NFT Unlock System
- Game checks NFT ownership from a smart contract  
- If owned → unlocks premium in-game content (e.g., special plane)  

### ⚙️ How It Works
- Uses `balanceOf` to verify NFT ownership  
- JavaScript ↔ Unity bridge for WebGL communication  
- Ethers.js used for blockchain interaction  

---

## 🧠 What Was Built

### Web3 + Unity Integration
- Implemented wallet connection using MetaMask + Ethers.js
- Built a JS plugin bridge to connect WebGL → Unity
- Passed wallet address into Unity game systems in real time
- Designed system where wallet acts as player identity

### NFT Verification System
- Integrated smart contract calls (balanceOf) to verify ownership
- Built logic to unlock premium content dynamically
- Built logic to unlock premium content dynamically

### Debugging & Problem Solving
- Uses `balanceOf` to verify NFT ownership
- Debugged WebGL runtime issues (e.g., memory errors, missing libraries)
- Fixed wallet connection inconsistencies across browser sessions
- Handled async communication between JS and Unity safely
- Traced blockchain calls using console + RPC responses

---

## 💡 Why Web3?

- **True Ownership** → Players own unlocks as NFTs  
- **Universal Identity** → Wallet replaces login systems  
- **Future Economy** → Enables tokens, trading, rewards  
- **Secure Unlocks** → Cannot fake ownership locally  

---

## 🛠️ Technical Details

| Category | Tech |
|--------|------|
| Engine | Unity (2D) |
| Language | C# |
| Backend | Supabase (REST API) |
| Web3 | Ethers.js + Smart Contracts |
| Platforms | WebGL / Desktop |
| Version Control | Git & GitHub |

---

## 📂 Project Highlights

- Object pooling for performance  
- Score-based difficulty scaling  
- Modular UI system  
- Clean separation of game logic & UI  
- Web3 wallet ↔ Unity communication bridge  
- On-chain NFT validation system  

---

## 🚧 Future Improvements

- New environments & themes  
- Additional hazards and enemy behaviors  
- Power-ups (slow time, shields, invincibility)  
- Mobile-friendly controls  
- Sound & visual polish  

### 🔮 Web3 Roadmap
- Token-based rewards  
- On-chain leaderboard  
- NFT achievements  
- Marketplace for skins  

---

## 👤 Developer

Built as an indie project using Unity + C#, focused on clean mechanics, performance, and scalable systems with Web3 integration.
