# Game Development Trial Project
Welcome to my game development trial project! This repository contains my implementations of the required tasks, demonstrating my skills in project setup, workflow management, gameplay mechanics, systems development, and maybe even custom editor tools.  
This project uses the Unity [6000.0.24f1](https://unity.com/releases/editor/archive) version.

## 📝 Overview
This trial consists of 4 tasks designed to prove my skills and train multiple areas of game development over 5 days.  
The tasks will evaluate:

- Project planning & estimation  
- Version control & workflow management  
- Problem-solving & critical thinking  
- Implementation of game mechanics & systems in the Unity engine

This project will follow a structured approach with proper documentation and respect a comprehensive commit history.

## ⏳ Task Breakdown & Time Estimation
| Task | Description | Estimated Time | Actual Time | Notes |
|-|-|-|-|-|
|Task 1	|Project setup, workflow diagram, and estimation |1h20m| 1h40m	| Editor installation took longer than expected. |
|Task 2	|Basic player controller, animations and camera |2h | 3h | I decided to spend some extra time to better refine the isometric movement and add particle effects for user feedback when moving on speed altering tiles. |
|Task 3	|Enemy AI state machine, nav mesh and animations |3h | 5h20m | NavMesh usage for 2D required some extra installation and understanding. I also greatly underestimated the task, especially that I need to rebuild the entire level to better fit an isometric world.|
|Task 4	|Extensive dialogue system, UI, branching and easy edit |4h | ? | Notes after completion |

## 🎮 Gameplay Mechanics
- **Player Movement**: Use the arrow keys or WASD to move the player character around the level.
- **Speed Altering Tiles**: Some tiles will increase or decrease the player's movement speed. (Removed for now due to the map being redesigned)
- **Enemy AI**: Enemies will chase the player when close and return to patroling when the player is too far.

# Sources and Credits
I used some inspiration from the following tutorials and resources to help me with the development of this project.  
It's the first time I'm working with 2D NavMesh and Isometric perspectives, so these tutorials were very helpful.  
NavMesh was especially tricky to get working in 2D since I only knew how to use it in 3D, but I managed to get it working in the end.
- **Isometric Tutorial**: [Beaver Joe](https://www.youtube.com/watch?v=ruDXAXcgqmE)
- **2D NavMesh Tutorial**: [King Dara](https://www.youtube.com/watch?v=QktWJHEJYlU)
