# Passage
Ever wish for Pokemon-styled walk-through warp doors, or fence gates that can sense you approach--you know--like those grocery store sliding doors or vicious predators stalking some prey?

Features
---
- No more accidentally dismounting your horse while trying to open a fence gate
- That's about the only discernable difference, except the presence of the tiny ghost that hurriedly closes the fence gates behind you

Click <a href="https://github.com/CoronaSophium/Passage/releases/latest" target="_blank">here</a> for the least release's download and instructions.

---
I made this mod out of a random necessity I personally encountered while playing Stardew Valley and learning C# and Stardew Valley modding. Turns out, however, there's another mod out there that accomplishes the same thing: `AutoGate` by Teban100/eroticremix [[Nexus Mods](https://rd.nexusmods.com/stardewvalley/mods/820/?) - [Forums](https://community.playstarbound.com/threads/autogate-automatically-opening-closing-gates.129074/)]. Whoops. Other than `AutoGate`'s lack of a config, the two mods are pretty much the same in functionality. I have yet to make any comparisons in performance or timings, however.

## Config
Checkout `Stardew Valley/Mods/Passage/config.json` for mod configuration:
| Default           | Description                                                                                                                                             | Name                                  |
|-------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------|---------------------------------------|
| [bool]: **true**  | while enabled, allows you to travel through doors by simply walking into them                                                                           | **EnableAutoDoorWarp**                |
| [bool]: **true**  | while enabled, makes fence gates automatically open as you approach, and close as you leave them behind                                                 | **EnableAutoFenceGateManagement**     |
| [bool]: **false** | while enabled, requires you to be riding a horse for the automatic fence management to occur                                                            | **OnlyOpenFenceGateWhileRidingHorse** |
| [float]: **1.1**  | this is how far you must travel away from a fence gate for it to automatically close behind you, after having been automatically opened by walking into | **MaxDistanceToKeepFenceGateOpen**    |
<br />
<br />
<hr />
<p align="center">
	<br />
	<b><i>Special thanks to:</i> </b>
	<br />
	<span>Cat, Prismuth, and spacechase0</span>
	<br />
	<b><i>from the <a href="https://discordapp.com/invite/stardewvalley" target="_blank">Stardew Valley Discord</a>!</i></b>
</p>
