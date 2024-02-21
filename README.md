# Roguelike Game
- The game is developed using Unity DOTS. 
- Unity New Input system is used.
- The player visual is spine Object,and all other enemies and bullets are rendered by custom shader which uses sprite sheets.
- Rendering is made by custom ECS System called 'SpriteSheetAnimationSystem'
- You can play the game either with joystick and WASD keys.

- To use joystick Input:
  * Open Scene called 'MainScene'-> Find PlayerTouchInputManager in Hierarchy window -> Change Input type to 'Joystick' and go to  Simulator window-> Select Simulator.
- To use joystick Input:
  * Open Scene called 'MainScene'-> Find PlayerTouchInputManager in Hierarchy window -> Change Input type to 'WASD'.
