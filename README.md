C# Albion Event and Operation Packet Hooking Framework
Create new hooks by adding an attribute to your method with which EventCode or OperationCode you'd like to receive:

Ex:
[EventHandler(EventCodes.Join)]

OR

[OperationHandler(OperationCodes.Move)]

Events can be attached currently to new players added, and loot being added to players.

Included Loot Logging UI and Framework

Todo: Move hooking framework to it's own DLL and generate object classes for each event and operation to automatically convert deserialized photon data to objects to be passed in to each event and operation instead of Dictionary's

# Update bin files
The resources folder will contain a list of encrypted bin files. If a new release occurs you will need to drop the files from the game folder and the program will automate decrypting them on the next load up.

Files are found at:
[Game Installation Dir]/game/Albion-Online_Data/StreamingAssets/GameData

# INSTALLATION
1. Right click the ZIP file and go to Properties
2. Unblock the file
3. Unzip to a folder
4. Run LootUI.exe as Administrator (Right click Run as Administrator)
