# Item Logistics
Item Logistics is a mod that provides item transportation and item sorting.

**Current Version = 0.1.0 (Beta)**

## WARNING:
This mod is in beta version. Backup your saves or create a new save before playing with it. You might lose items and get crashes.

## Contents:
- [Current Features for 0.1.0](#current-features-for-010)
	- [Pipes](#pipes)
		- [Connectors](#connectors)
		- [Outputs](#outputs)
		- [Inputs](#inputs)
	-  [Supported containers](#supported-containers)
	-  [Supported buildings](#supported-buildings)
-  [Network Building](#network-building)
-  [Misc](#misc)
-  [Planned Features](#planned-features)
-  [See also](#see-also)

---

## Current Features (for 0.1.0):

## Pipes:
- ### Connectors:
	- **Connector Pipe**  
Connector Pipes act as the link between Output and Input pipes.
It moves items at 2 tiles/1 second
!(https://github.com/sergiomadd/StardewValleyMods/edit/main/ItemLogistics/docs/img/ConnectorPipe.png)

## IOPipes:
- ### Signaling:
Three colors  
Each IO Pipe can only have 1 container adjacent. To attach a new IO Pipe to a container, you must remove all IO Pipes surrounding the container, then you could place the new one.

- ### Outputs:
	- **Extractor Pipe**  
Extractor Pipes extract items from an adjacent container, at 1 stack/3 seconds. 

- ### Inputs:
	- **Inserter Pipe**  
Inserter Pipes insert items into an adjacent container. They do not filter any item.  
	- **Polymorphic Pipe**  
Polymorphic Pipes insert items into an adjacent container. They filter items based on the container's current items. If the container is empty, it acts like an Inserter Pipe, until 1 item enters the container.  
	- **Filter Pipe**  
Filter Pipes insert items into an adjacent container. They filter items based on the pipe's internal inventory. To open the inventory simply left click the Filter Pipe like a chest. Then add the items you want filtered. If the internal inventory is empty, it acts like an Inserter Pipe, until alteast 1 item is added to the internal inventory.  

- ### Supported containers:
	- **Chest**  
Regular vanilla chests.  
	- **Junimo Chest**  
Junimo chests work as chests. But as their inventory is linked, so you can move items wirelessly.  
	- **Fridge**  

	- **Mini-Shipping Bin**
Mini-Shipping Bin work as chests.  

- ### Supported buildings:
	- **Shipping Bin**  
The Shipping Bin doesn't work like a regular chest, but you can insert items into it if and input is adjacent. These items will be shipped at the end of the day. 
Be careful, as you may not be able to get the items back once the get piped.  

---

## Network Building:
A valid network has to have **at least 1 Output Pipe and 1 Input Pipe**.  
Also for the output to start pumping items, the input has to have a valid adjacent container.  
That said, you can have as many outputs and inputs as you like.


---

## Misc:


---

## Planned Features (For 1.0.0 release):
- Custom Filter Chest UI
- Add whitelist and blacklist to Filter Chest?
- Add ghosts items in filter
- Add thread stopping when unloading save
- Add config file with debug mode
- Pipe tiers

## Planned Features:
**Short term:**
- Add machine that if connected to a network, it will turn that network pipes crossable
- Add IOPipe state message, so to not rely only on colors.
- Add a way to switch on/off outputs
- Add more pipe feedback (like when a network is correctly build)
- Add Extractor Filter Pipe?

**Long Term:**
- Storage Hub: A central storage chest. Like in AE from minecraft.
- Pipes working between locations.

## See also
- [Release Notes](https://github.com/sergiomadd/StardewValleyMods/edit/main/ItemLogistics/docs/release-notes.md)
- [Nexus Mod Page]
