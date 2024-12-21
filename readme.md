# Using
### In Unity
In Unity install the package.

Go to preferences > Steam Deck Deployer and set the name to the name of you game in the DevKit Client tool

### Dev Kit Client Tool
In the DevKit Client tool make sure that the name is set to the same as in Unity, Steam Play is checked and Auto Upload is checked

Set the Local Folder to whatever you set to the Unity build output plus /SteamDeck
so if you output to C:/dev/build/mygame then the client tool should go to C:/dev/mygame/build/SteamDeck

then set the start command to SteamDeckBuild.exe along with any other needed start options