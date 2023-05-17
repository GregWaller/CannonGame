# Cannon Game

This game was created to act as a benchmark for my current aptitude for gameplay programming in Unity with C#.  The goal of the project was to create a simple game that demonstrates:
- Use of physics to cause an object in 3D to move
- Detection and reaction to collisions in world space for that object
- And to control the trajectory of that object with a simple user interface that allows the pitch and yaw to be manipulated with the arrow keys, and the object to be fired with the space bar.
- 

## Technologies Used
The project was created with Unity 2021.3.25f1 using the High-Definition Render Pipeline.  Static asset models were created using Blender 3.5.1.  The project uses Visual Effect Graph 12.1.11 to achieve many of the particle effects used, as well as Shader Graph 12.1.11 for some environment shaders.  The code was written in C#.


## Usage
The game's controls are:

- A-D/Left-Right will control the cannon's yaw
- W-S/Up-Down will control the cannon's pitch
- Space/Left-Mouse will fire the cannon
- Q will purchase more cannon balls.  The cost is 100 doubloons for 10 balls.
- E will repair the ship.  The cost is 100 doubloons for 1 hit point.


## License
![Creative Commons](https://i.creativecommons.org/l/by-nc/4.0/88x31.png) This project is licensed under a Creative Commons Attribution-NonCommercial 4.0 International license.