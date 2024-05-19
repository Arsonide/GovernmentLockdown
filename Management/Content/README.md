# Government Lockdown

## What is it?

This plugin aims to make it harder to use government databases to trivialize your detective work. Databases are now better guarded, and hospitals now have medical information instead of fingerprints.

## Installation

### r2modman or Thunderstore Mod Manager installation

If you are using [r2modman](https://thunderstore.io/c/shadows-of-doubt/p/ebkr/r2modman/) or [Thunderstore Mod Manager](https://www.overwolf.com/oneapp/Thunderstore-Thunderstore_Mod_Manager) for installation, simply download the mod from the Online tab.

### Manual installation

Follow these steps:

1. Download BepInEx from the official repository.
2. Extract the downloaded files into the same folder as the "Shadows of Doubt.exe" executable.
3. Launch the game, load the main menu, and then exit the game.
4. Download the latest version of the plugin from the Releases page. Unzip the files and place them in corresponding directories within "Shadows of Doubt\BepInEx...". Also, download the [SOD.Common](https://thunderstore.io/c/shadows-of-doubt/p/Venomaus/SODCommon/) dependency.
5. Start the game.

## Usage and features

### Configuration

There are no configuration settings at this time.

### Features
* All government and medical databases are now found in secure rooms of City Hall. That means you won't find them in rooms that don't consider you to be trespassing, like the little reception areas outside of secure Enforcer offices. Government databases will be in secure Enforcer rooms, and medical databases will be in secure Hospital rooms.
* The Surveillance and Security apps will only be found in secure Enforcer areas, not Hospital areas.
* Crunchers outside of those areas will only have limited employee databases available.
* Hospital crunchers now contain medical databases that print birth certificates, not government files on citizens.
* When searching any database, before you needed two characters, but "space" also counted as one, which let people search for " B" and find every citizen with a last name starting with B. It is now required when searching that there be two non-whitespace characters. The intention of this change is to prevent these kinds of brute force searches. Ideally the player goes to the database to confirm a specific suspect in mind.
* The hope is that all of these changes are lightweight enough to not impact the game too much, but realistically weaken how powerful the government database is in the stock game.

## License

All code in this repo is distributed under the MIT License. Feel free to use, modify, and distribute as needed.