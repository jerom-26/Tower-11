# Tower 11

Tower 11 is a fast-paced 2D arcade survival game built with Unity. Control a plane, dodge towers and rockets, and survive as long as possible.

<p align="center">
  <img src="https://github.com/user-attachments/assets/a0167784-5baa-4d5b-93a0-8ae717a53841" width="700"/>
</p>

---

## Play

| Version | Link |
|---------|------|
| Standard | https://jerom001.itch.io/tower-11 |
| Web3 (Experimental) | https://jerom001.github.io/Tower-11 |

---

## Gameplay

Fly through a city, avoid towers and rockets. One mistake ends the run. Difficulty scales with score.

**Controls**
- Tap / Click / Space — fly up
- Release — fall
- ESC — pause

**Rockets**
- Chase — tracks your movement
- Charge — delayed attack
- Decoy — straight path
- Dummy — harmless

---

## Features

- Smooth 2D movement with dynamic difficulty scaling
- Parallax background and pause system
- Username system with validation
- Persistent personal bests
- Online leaderboard via Supabase (syncs on new high score only)
- Object pooling for performance

---

## Web3 Integration (Experimental)

The Web3 version adds wallet-based identity and NFT-gated content on top of the standard build.

- MetaMask wallet connection via browser
- Wallet address used as player identity
- NFT ownership verified through `balanceOf` on a smart contract
- Content unlocked dynamically based on ownership

**Implementation:** Ethers.js handles blockchain interaction through a JavaScript ↔ Unity WebGL plugin bridge, passing wallet state into game systems in real time.

---

## Tech Stack

| Category | Tech |
|----------|------|
| Engine | Unity (2D) |
| Language | C# |
| Backend | Supabase |
| Web3 | Ethers.js |
| Platform | WebGL |
| Version Control | Git & GitHub |

---

## Developer

**Jerom Jiju** — Game Developer / Software Developer

- GitHub: https://github.com/jerom001
- Itch.io: https://jerom001.itch.io
- LinkedIn: https://linkedin.com/in/jerom-jiju26
