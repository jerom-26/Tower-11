Tower 11

Tower 11 is a 2D arcade-style survival game developed in Unity. Players control a plane navigating a scrolling cityscape, avoiding towers and homing missiles to achieve the highest score possible. High scores persist locally and sync to an online leaderboard via Supabase.


🕹️ Gameplay Overview
Control a plane flying through a scrolling city environment.
Avoid towers and guided missiles to stay alive.

Score increases over time — collisions end the run instantly.

High scores are saved both locally and online.

✨ Features
Responsive and simple 2D gameplay.

Game Over screen with instant replay option.

Username system with local validation and storage.

Online leaderboard powered by Supabase.

Persistent high scores using Unity’s PlayerPrefs.

🛠️ Technical Details

Engine: Unity (2D)

Language: C#

Backend: Supabase (REST API)

Platform Target: WebGL / Desktop

Version Control: Git & GitHub

🌐 Leaderboard System

Players register a username once (validated locally).

Scores update only when a new personal best is achieved.

Fetches and displays the global top scores in real time.

Username validation rules:

All lowercase letters

Length within defined limits

Allowed characters: a–z, 0–9, _

🎮 Controls
Tap / Space Bar — Move the plane

Esc — Open menu or pause

Play Again — Restart instantly

🚧 Future Ideas
New environments and hazards

Power-ups for temporary invincibility

Mobile-friendly touch controls
