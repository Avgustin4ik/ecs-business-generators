Stack: LeoEcsLite | R3
Structure: EcsGameCore contains and runs all systems. There are three of these: GameplaySystems, UISystem, and SaveLoadSystem.

SaveLoadSystem is triggered by ApplicationQuitHandler to enable saving during forced app quits on older smartphones.

Services are used to read configurations and provide them to systems. They are ScriptableObjects that create service classes and publish them to systems as shared data.

Generator-, UI-, and SaveLoad aspects store useful filters and component pools.
