# Setup

## Compatibility
The executable file located in the `installer` folder is only compatible for Windows machines. For other operating systems, it is necessary to build and run the Unity application from the source code, using `client` as the root folder.
To download Unity, check out the [downloads page](https://unity3d.com/get-unity/download) of Unity's website. Note that the version of Unity used for this project was 2020.2.1f1.

For details on how to build and run a Unity application, please see [this video](https://youtu.be/7nxKAtxGSn8) (up to 6:05). 

## Dependencies
This application relies on two external server components: a [Lobby Service](https://github.com/kartoffelquadrat/LobbyService) (LS) instance and a [SmartFoxServer](https://www.smartfoxserver.com/) (SFS) instance. Due to the nature of this product being built for a course project, these server components were initially set up on an Azure instance. Currently, they are not running, and the application is now configured to rely on [localhost](https://www.hostinger.com/tutorials/what-is-localhost) for both of these instances. Therefore, in order to run this game, either (1) the LS and SFS must be set up locally; or (2) the LS and SFS must already be set up at (a) different location(s) and the source code must be modified to point to the new location(s).

### Dependency Setup
To set up the LS, please see the [LS Build & Deploy Instructions](https://github.com/kartoffelquadrat/LobbyService/blob/master/markdown/build-deploy.md).

To set up SFS, please see the [SFS documentation](http://docs2x.smartfoxserver.com/).

### Configuring source code for server components
To configure the LS/SFS instances with the application, IP address must be replaced by substituting the value `127.0.0.1` with the address being used. Note that it is possible to use two different instances for the LS and SFS, but it is not necessary.

#### LS
The IP address substitution must be made in various scripts located in `client\Assets\Scripts\`:
* `Login.cs` line 15
* `Register.cs` line 18
* `WaitingRoom.cs` line 24
* `ChooseCharacter.cs` line 33
* `GameBoard.cs` line 33
Please use the default port 4242.

If this is the first time the LS instance is being used for this application, a game service called "ColtExpress" must be manually created.

#### SFS
The IP address substitution must be made in the following script: `client\Assets\Scripts\SFS.cs` line 43. Please use the default port 9933.

If this is the first time the SFS instance is being used for this application, the server code must then be exported to the SFS instance. This can be done by exporting the server code located under the `server` folder. Please see [this video](https://www.youtube.com/watch?v=nKGxhwJ0Ccc&list=PLC16B8E94B9D7C3E5) for more details. Note that SFS requires Java 8 (not later).

The exported .jar file must be attached to an extension for an SFS zone entitled "MergedExt". This zone must be activated and a room must be created in this zone using any name.
