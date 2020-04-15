C# Albion Event and Operation Packet Hooking Framework
Create new hooks by adding an attribute to your method with which EventCode or OperationCode you'd like to receive:
Ex:
[EventHandler(EventCodes.Join)]
OR
[OperationHandler(OperationCodes.Move)]

Events can be attached currently to new players added, and loot being added to players.

Included Loot Logging UI and Framework

Todo: Move hooking framework to it's own DLL and generate object classes for each event and operation to automatically convert deserialized photon data to objects to be passed in to each event and operation instead of Dictionary's

