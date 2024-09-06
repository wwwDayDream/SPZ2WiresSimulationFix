# Wires Simulation Fix
Removes the LOD factor on the `requiredDeltaTicks` inside the SimulationGraph::Update.

This makes wire networks that are out of view update at the same rate as the ones you're currently looking at.

Requires BepInEx Version
`5.4.2100` [Thunderstore Download](https://thunderstore.io/package/download/BepInEx/BepInExPack/5.4.2100/)