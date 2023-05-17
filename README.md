
# Cannon Game

This game was created to act as a benchmark for my current aptitude for gameplay programming in Unity with C#.  The goal of the project was to create a simple game that demonstrates:

- The use of physics and rigid bodies to cause a 3D object to move
- Detection and reaction to collisions in world space for that object
- And to control the trajectory of that object with a simple user interface that allows the pitch and yaw to be manipulated with the arrow keys, and the object to be fired with the space bar.

The project you see today grew from those basic requirements.


## Technologies Used
The project was created with Unity 2021.3.25f1 using the High-Definition Render Pipeline.  Static asset models were created using Blender 3.5.1.  The project uses Visual Effect Graph to achieve many of the particle effects used, as well as Shader Graph for the ocean shader.  The code was written in C#.  All other assets are built-in unity assets.


## Usage
The game's controls are:

- A-D/Left-Right will control the cannon's yaw
- W-S/Up-Down will control the cannon's pitch
- Space/Left-Mouse will fire the cannon
- Q will purchase more cannon balls.  The cost is 100 doubloons for 10 balls.
- E will repair the ship.  The cost is 100 doubloons for 1 hit point.


## Development Process 

I started this project on the afternoon of Friday, May 12th, 2023.  To speed up development, I imported several framework classes I've developed for other projects to get up and running quickly with some essentials -- a central game controller and input handling using Unity's "new" Input system.  From there, I started to work on building the basic controls for creating the pitch/yaw for the cannon.

The next step was to spawn a cannon ball.  The primary challenge here was to add a pooling mechanism to allow for easy instantiation of the objects without cluttering the hierarchy, making sure to re-use any objects that had hit a target.  Once that was done, it was simple enough to measure collisions and de-spawn balls accordingly.

The next step was to create the target.  I wanted to re-use the pooling mechanism I had made for the cannon balls, so I started work on re-factoring the factory class, as well as building a very simple patrol-based AI for allow the targets to move (and add some challenge to the game).  I think this is where the game started to begin to realize itself.  The decision was made that this would be a nautical/sea-faring game featuring pirates, water, boats, and cannon balls.  As such, I decided to give guns to the targets and let them shoot back at the player.  They used the same cannon and interface, but needed to track the player's position automatically.  They used the recycled cannon balls as well.

When I started building the UI, the logic for the player health and ammo counts just fell naturally into place.   The primary level controller came next, allowing for a consistent and re-playable game loop to form.  The player's economy developed from there, and I added interfaces and inputs to repair and purchase ammunition to continue fighting.  

The rest was, as they say, gravy.  I imported a number of particle systems I had developed for other games as a starting point to develop the graphs and systems that you see in the project today.

I wrapped up development on this project on the evening of Tuesday, May 16th, 2023, as I sat down to write this README.  I spent a total of 42 hours building this project, broken down as follows:

- Project Management: 5 hours
	- Includes Requirements Analysis, Design, Planning, Builds, Planning, and Repository Management
- Programming: 24 hours, 30 minutes
	- Includes Gameplay and Systems programming
- Art: 13 hours
	- Includes 3D modeling, animation, and tech art (particles, shaders, lighting, and post processing).

It's worth noting that I made a conscious decision to not include audio with this project.  I could have easily spent another day or two finding some free audio assets and built a simple audio controller to play the clips, but I didn't do this because I wanted this project to be a demonstration of my capabilities, and music and sound engineering are too far outside my skillset to be considered for inclusion.  As it is, everything that is in the project that isn't built-in Unity, is me.  Every line of code (in Assets/source), every model, every shader, every particle system, every material.

I hope you find Cannon Game as fun to play as I had building it.


## License
This project is licensed under a Creative Commons Attribution-NonCommercial 4.0 International license.
![Creative Commons](https://i.creativecommons.org/l/by-nc/4.0/88x31.png) 
