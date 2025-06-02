# LasMeta Case Study: Dealer Interaction Prototype (Unity + Photon Fusion)

## Overview

This prototype demonstrates a lightweight, performance-conscious multiplayer poker scene built with **Unity** and **Photon Fusion**. Players interact with a dealer character who responds to their actions with animations, sound, and visual feedback. The objective was to simulate a realistic "deal cards" interaction, focusing on clarity, responsiveness, and sync accuracy across clients.

---

## ✅ Key Features

### 1. 3D Scene Composition

To simulate a believable environment while maintaining performance:

* A basic poker table was created using **optimized assets from Sketchfab**.
* The dealer character is animated with **Mixamo**, presented in a standing idle pose, and controlled via an **Animator Controller**.
* Player positions are represented with network-synced directional arrows labeled with **PlayerID** for clarity.

### 2. Photon Fusion Integration

Photon Fusion (v2) in **Shared Mode** was used to implement efficient and scalable networking:

* **NetworkEvents** handle player connections and spawning.
* "Deal Cards" is managed via **state-based validation**, ensuring only the correct player at the correct time can trigger it.
* Animation, sound, and other effects are **broadcasted using RPCs**, ensuring all clients stay in sync.

### 3. Dealer Animation, Audio & Networking

* Dealer performs an animation (`Run`, used here as a placeholder).
* A card dealing **SFX** is played on all clients.
* Card distribution is simulated using animated **3D cards** with **DoTween**, ensuring smooth transitions.
* Turn-based control is enforced with `RequestStateAuthority` and `ReleaseStateAuthority` calls to prevent overlapping actions.

### 4. UI & Input

User input is designed for both desktop and development convenience:

* A clean, **custom UI built in Figma** features a “Deal Cards” button.
* Interactions are validated based on game state and player turn.
* **3D card objects** are recycled efficiently to reduce memory usage and instantiation overhead.

### 5. Camera & Roaming

To allow inspection and immersion, player camera control includes:

* A default **third-person camera** with WASD + mouse movement.
* Toggle for **first-person roaming mode**.
* In third-person, players can orbit and zoom the camera using UI controls.
* A **runtime spotlight control** allows players to adjust light intensity with a UI slider, useful for visual emphasis or immersion testing.

### 6. Bonus Features

Going beyond the basic requirements:

* **Round-based card dealing** implemented (one card at a time).
* **Game Guide Info Panel** built using animated UI shows current game state and messages; animated via DoTween for smoothness and polish.
* State information is maintained via **Networked Properties** for all clients.
* Lighting is optimized via **static light baking** and **layer-masked real-time lights** to ensure only relevant objects receive lighting updates.
* **Performance-conscious object reuse** strategy to reduce runtime allocation and GC overhead.
* Game logic is structured around clear **turn control** via Fusion’s authority system.

---

## ⚙️ Tools & Systems Used

| Tool                                  | Purpose                                      |
| ------------------------------------- | -------------------------------------------- |
| **Unity 2022 LTS**                    | Base development platform                    |
| **Photon Fusion 2** (Shared Mode)     | Networking and multiplayer logic             |
| **Mixamo**                            | Dealer animations                            |
| **DoTween**                           | UI and 3D card animation                     |
| **ParrelSync**                        | Multi-instance local testing for multiplayer |
| **Figma**                             | UI asset design                              |
| **Fusion GUI**                        | Quick connection and session handling        |
| **Fusion RPC & Networked Properties** | State sync and event broadcasting            |
| **Light Baking & Layer Masking**      | Lighting optimization                        |

---

## ⚠️ Challenges Faced

Building a real-time synced experience came with hurdles:

* `IPlayerJoined` on `SimulationBehaviour` failed to trigger, requiring a fallback to `NetworkEvents`.
* Documentation around **ChangeDetector** and **OnChanged** was limited, so RPCs were used instead—though not ideal for large-scale design.
* In some cases, **state authority delays** introduced race conditions; managing those required careful event ordering.
* Unexpected behavior from `HasStateAuthority` until authority transfer was explicitly managed.
* While Shared Mode was used for simplicity, future implementations would benefit from **Fusion Server Mode** for stronger control and robustness.

---

## 🎬 Demo



---

## 📩 Contact

**MD SAKIB HASAN**  
📧 [sakibh20@gmail.com](mailto:sakib.hasan@tuni.fi)  
🌐 [mdsakibhasan.com](https://mdsakibhasan.com)  
📁 [Portfolio](https://mdsakibhasan.com)  
📱 +358 0417432173  
