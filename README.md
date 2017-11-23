# Passage
Ever wish for Pokemon-styled walk-through warp doors, or fence gates that can sense you approach--you know--like those grocery store sliding doors or vicious predators stalking some prey?

Features
---
- No more accidentally dismounting your horse while trying to open a fence gate
- That's about the only discernable difference, except the presence of the tiny ghost that hurriedly closes the fence gates behind you

Installation
---
1. Install [SMAPI](https://github.com/Pathoschild/SMAPI/releases/latest) (tested using SMAPI v2.1)
2. Download Passage's latest release from [here](https://github.com/CoronaSophium/Passage/releases/latest), and extract it to `Stardew Valley/Mods`
*_(optional)_*
3. Launch Stardew Valley with SMAPI once to generate Passge configuration file
4. Edit `Stardew Valley/Mods/Passage/config.json` to your heart's content

Config
---
Checkout `Stardew Valley/Mods/Passage/config.json` for mod configuration:

| Default           | Description                                                                                                                                             | Name                                  |
|-------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------|---------------------------------------|
| [bool]: **true**  | while enabled, allows you to travel through doors by simply walking into them                                                                           | **EnableAutoDoorWarp**                |
| [bool]: **true**  | while enabled, makes fence gates automatically open as you approach, and close as you leave them behind                                                 | **EnableAutoFenceGateManagement**     |
| [bool]: **false** | while enabled, requires you to be riding a horse for the automatic fence management to occur                                                            | **OnlyOpenFenceGateWhileRidingHorse** |
| [float]: **1.1**  | this is how far you must travel away from a fence gate for it to automatically close behind you, after having been automatically opened by walking into | **MaxDistanceToKeepFenceGateOpen**    |

AutoFence
---
I made this mod out of a random necessity I personally encountered while playing Stardew Valley and learning C# and Stardew Valley modding. ~~Turns out, however, there's another mod out there that accomplishes the same thing: ~~`AutoGate` by Teban100/eroticremix [[Nexus Mods](https://rd.nexusmods.com/stardewvalley/mods/820/?) - [Forums](https://community.playstarbound.com/threads/autogate-automatically-opening-closing-gates.129074/)].

I've rewritten the mod since its initial release and renamed it from "AutoFenceGate" to "Passage". While AutoFenceGate and AutoFence used to share 100% of their features (bar any configuration differences), Passage now _also_ supports auto-interaction with doors.
<br />
<br />
<hr />
<p align="center">
	<br />
	<b><i>Special thanks to:</i> </b>
	<br />
	<span>Cat, Prismuth, spacechase0, and Entoarox</span>
	<br />
	<b><i>from the <a href="https://discordapp.com/invite/stardewvalley" target="_blank">Stardew Valley Discord</a>!</i></b>
</p>
