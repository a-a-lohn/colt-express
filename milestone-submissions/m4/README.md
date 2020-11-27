# Milestone 4: Colt Express User Interface Demonstration (November 23rd - November 27th 2020)

## Description
The goal of this milestone is to provide a first demonstration of the user interface of your game. You run the show
and present how the player interacts with the user interface of the game. None of the actual game rules need to be
implemented, but some minimum functionality should be shown as outlined below.

* Minimum Requirements
  * must be performed on at least two machines
  * show how two players log in on their machines, and then see a list of other available players and/or available games You must
  * use the external lobby service to create a game session with one player and join the session with the other player.
  * demonstrate the main screen of the game and at least two bandits (one for each player) placed in some wagon
  * show that it is already possible to interact with the game in a rudimentary way
  * demonstrate how to move a hero from one wagon to an adjacent wagon
  * may demonstrate any other functionality 
  * additional points for any additional functionality that you demonstrate

## File Organization 
  * the Assets folder contains all the scenes, scripts, resources and imports used in this project. Can't upload the entire project because it is too large...

## File Structure 
```
Assets
 ┣ Mirror
 ┣ ParrelSync
 ┣ Plugins
 ┃ ┣ Newtonsoft.Json.dll
 ┃ ┣ RestSharp.dll
 ┣ Prefabs
 ┃ ┣ black.prefab
 ┣ RPG Music Pack
 ┣ Resources
 ┃ ┣ Scenes
 ┃ ┃ ┣ additional-bg.png
 ┃ ┃ ┣ lobby-background.jpg
 ┃ ┃ ┣ main-menu.jpg
 ┃ ┣ bg.png
 ┃ ┣ black.png
 ┃ ┣ blue.png
 ┃ ┣ cart.png
 ┃ ┣ green.png
 ┃ ┣ left arrow.png
 ┃ ┣ locomotive.png
 ┃ ┣ mnt1.png
 ┃ ┣ mnt2.png
 ┃ ┣ pink.png
 ┃ ┣ railroad.png
 ┃ ┣ red.png
 ┃ ┣ right arrow.png
 ┃ ┣ stagecoach.png
 ┃ ┣ tree.png
 ┃ ┣ white.png
 ┃ ┣ yellow.png
 ┣ Scenes
 ┃ ┣ Additional.unity
 ┃ ┣ Game.unity
 ┃ ┣ GameBoard.unity
 ┃ ┣ HostGame.unity
 ┃ ┣ JoinGame.unity
 ┃ ┣ Login.unity
 ┃ ┣ MainGameBoard.unity
 ┃ ┣ MainMenu.unity
 ┃ ┣ Register.unity
 ┃ ┣ SampleScene.unity
 ┃ ┗ WaitingRoom.unity
 ┣ Scripts
 ┃ ┣ Additional.cs
 ┃ ┣ Lobby.cs
 ┃ ┣ Login.cs
 ┃ ┣ MainMenu.cs
 ┃ ┣ Player.cs
 ┃ ┣ PlayerMove.cs
 ┃ ┣ WaitingRoom.cs
 ┣ TextMesh Pro
 ┗ .DS_Store
```

## Structure of the UI Demo 
  * demonstration of the client-server interactoin (done via using two computers to host/join the game)
  * demonstration of the main game board (how a player moves to an adjacent train car)
  * demonstration of the additional functionality (synchronized actions across LAN (need to solve port forwarding issues))
  


