# Navigation-Mesh-Generator

This repo holds the code and package of the Navigation Mesh Generator prototype for my Degree Final Project.
This prototype is part of the paper Navigation mesh system for single agent 2D pathfniding.

Here you will find the project with all the code, testing, scripts of tests and such.

# Installing the tool

Befor installing the tool, we need to install LibTessDotNet, you can do so by downloading the package
from this [repo](https://github.com/pacoelayudante/com.guazu.lib-tess-dot-net) by [pacoelayudante](https://github.com/pacoelayudante). It contains steps on how to install it but if you're not sure, download the repo and in Unity under [...]


To install the tool/library to generate navigation meshes download and extract the .zip of the releases section.
Create a Resources folder if you don't have one, and create a folder named NavMeshGen. Inside said folder, unpack the package
NavMeshGen.
You can now run the demo scene to test anything you like or to see how a Navigation Mesh game object must be prepared.


# Creating a map with a navigation mesh 
Whenever you’re finished placing the tiles of your map to define the environment, here’s a little guide on how to make your navigation mesh work. 
To get started, add a composite collider component to the grid of your map (the parent of all the tile maps) and place 2D colliders around the scene as desired.


You can add simple colliders in different spaces, you can use a rule tile to spawn a game object with a collider, you can use the tile map collider feature etc.
As long as you set the colliders you place (in whatever method you want) as used by the composite collider (it's a checkbox in 2D colliders), you're good to go. 
The only requirement is that a closed area must be created, with all walkable areas and obstacles within said closed area. 



The next step is to add a NavMesh script to the game object that contains the composite collider. This script will instantiate the necessary classes from the different processes, and you can generate the mesh from scratch simply by calling its CreateMesh function.
Congratulations! You now have a navigation mesh that you can use with any pathfinding algorithm you want to use or implement.
