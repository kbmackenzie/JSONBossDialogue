This is a BepInEx plugin mod made for Inscryption.
This mod lets you load custom dialogue for the three first bosses in the game (the Prospector, the Angler and the Trapper/Trader) with a JSON file.

This mod doesn't change your game files and shouldn't affect your save data, but making a backup of your save file before installation is still advised.

## Installation
This mod’s only dependency is BepInEx, which you can find in the [BepInEx Github page](https://github.com/BepInEx/BepInEx/releases) or in the Thunderstore page for ["BepInExPack Inscryption"](https://inscryption.thunderstore.io/package/BepInEx/BepInExPack_Inscryption/). The latter comes with a preconfigured `BepInEx.cfg`, so it's advised.

There are two ways of installing this mod: with the help of a mod manager (like r2modman or the Thunderstore Mod Manager) or manually.

**At the time of writing this,** the mod isn't yet on Thunderstore, so using a mod manager will only help you install BepInEx. This is already extremely helpful, though.

#### Installation (Mod Manager)
1. Download and install [r2modman](https://thunderstore.io/package/ebkr/r2modman/) or the [Thunderstore Mod Manager](https://www.overwolf.com/app/Thunderstore-Thunderstore_Mod_Manager).
2. Go to the page for the ["BepInExPack Inscryption" mod on Thunderstore](https://inscryption.thunderstore.io/package/BepInEx/BepInExPack_Inscryption/) and follow its automated installation guide.
3. Go on `Settings > Browse Profile Folder` to open up the folder of your current mod manager profile and find the `BepInEx > plugins` folder.
4. Place the contents of **"JSONBossDialogue.zip"** in a new folder within the plugins folder.

#### Installation (Manual)
1. Download and install BepInEx.
    1. If you're downloading it from [its Github page](https://github.com/BepInEx/BepInEx/releases), follow [this installation guide](https://docs.bepinex.dev/articles/user_guide/installation/index.html#where-to-download-bepinex).
    2. If you're downloading ["BepInExPack Inscryption" from Thunderstore](https://inscryption.thunderstore.io/package/BepInEx/BepInExPack_Inscryption/), follow the manual installation guide on the Thunderstore page itself.
4. Find the `BepInEx > plugins` folder.
5. Place the contents of **"JSONBossDialogue.zip"** in a new folder within the plugins folder.


## Creating the JSON File
**Your JSON file’s name must end in `_bd.json`.** This is required in order for this mod to find it.
You can name it anything you want so long as the name ends in `_bd.json`.

If you don't know how to create a JSON file, an easy way to do it is creating a `.txt` file and renaming it to end in `.json`.

Afterward, paste the following into the file:

```json
{
	"Prospector": {
		"PreIntro": "",
		"Intro": "",
		"BeforePickaxe": "",
		"AfterPickaxe": "",
		"IfNoGold": ""
	},
	"Angler": {
		"PreIntro": "",
		"Intro": "",
		"GoFish": "",
		"AimingHook": "",
		"EasyChoose": "",
		"HookPull": ""
	},
	"TrapperTrader": {
		"PreIntro": "",
		"Intro": "",
		"PrePhase2": "",
		"Phase2": "",
		"PreTrade": "",
		"Trade": "",
		"PostTrade": ""
	}
}
```

Your custom dialogue should go between the second pair of quotation marks in each line. Be sure not to erase any commas!

You can leave as many fields empty as you wish. If a field is empty, the mod will just let the game play the regular dialogue for the corresponding part.

And here's an explanation of each field and the dialogue lines they replace:

#### Prospector

| Field         | Dialogue                                                                                                                              |
|---------------|---------------------------------------------------------------------------------------------------------------------------------------|
| PreIntro      | Leshy's ominous lines before the Prospector's intro.                                                                                  |
| Intro         | Prospector's introduction.                                                                                                            |
| BeforePickaxe | What the Prospector says before hitting the board with the pickaxe. Defaults to *"THAR'S GOLD IN THEM CARDS!"*.                       |
| AfterPickaxe  | What the Prospector says *after* hitting the board with the pickaxe. Defaults to *"G-G-GOLD! I'VE STRUCK GOLD!"*.                     |
| IfNoGold      | What the Prospector says if there are no cards on the board when he's about to strike it (I assume?). Defaults to *"N-... NO GOLD?"*. |

#### Angler

| Field      | Dialogue                                                                                     |
|------------|----------------------------------------------------------------------------------------------|
| PreIntro   | Leshy's ominous lines before the Angler's intro.                                             |
| Intro      | Angler's introduction.                                                                       |
| GoFish     | What the Angler says after placing his Bait Buckets.                                         |
| AimingHook | What the Angler says when he randomly aims his hook at one of your cards.                    |
| EasyChoose | What the Angler says when you place a new card on the board and he aims his hook towards it. |
| HookPull   | What the Angler says when he pulls one of your cards with his hook.                          |

#### Trapper / Trader

| Field     | Dialogue                                          |
|-----------|---------------------------------------------------|
| PreIntro  | Leshy's ominous lines before the Trapper's intro. |
| Intro     | Trapper's introduction.                           |
| PrePhase2 | Trapper switches with Trader.                     |
| Phase2    | Trader's introduction.                            |
| PreTrade  | What the Trader says before trading.              |
| Trade     | What the Trader says during the trade.            |
| PostTrade | What the Trader says after trading.               |


## Important Notes
A few things you should keep in mind about your JSON file:

- **You cannot have more than one `_bd.json` file in the plugins folder.** Having more than one WILL cause an error. Choose one.
- This mod may not be compatible with other mods that change boss dialogue.
- Validating your JSON with the help of an online tool can save you a lot of time and headache! Just look up "JSON Validator".
  - An online JSON validator tool I personally like is [JSONLint](https://jsonlint.com/), it's neat! 

Being unable to have more than one `_bd.json` file in the plugins folder does sadly mean that mods that use this custom dialogue mod are incompatible with each other: they cancel each other out and the dialogue will just default to base game dialogue. Everything else should work completely fine, there simply won't be custom dialogue.
I may add a workaround for that in the future. For now, the simplest way around this is to just choose one of the conflicting `_bd.json` files to keep and either rename or delete the others.


## Help
**Q:** *"I got an error that says no `_bd.json` file could be found! What do I do?"*
**A:** Double-check your JSON file. Make sure it ends in `_bd.json` and that it's somewhere inside of the `BepInEx/plugins` folder.

**Q:** *"I got an error that says there's more than one `_bd.json` in the plugins folder! What do I do?"*
**A:** That's self explanatory. You cannot have more than one `_bd.json` file in the plugins folder. Choose one and delete the others.
Read the "Important Notes" section above for more information on this issue.

**Q:** *"I got an error that says it could not load JSON from my `_bd.json` file! What does this mean?*
**A:** It means there's something wrong with your file. Make sure you didn't erase any commas or curly brackets.
As I mentioned in the "Important Notes" section above, using an online JSON validator tool can help you a lot with this. I personally like [JSONLint](https://jsonlint.com/), it's neat.
