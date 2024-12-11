## jcdcdev.Valheim.Signs

<img src="https://raw.githubusercontent.com/jcdcdev/jcdcdev.Valheim.Signs/main/icon.png" alt="jcdcdev.Valheim.Signs icon" height="150" align="right">

![GitHub Release](https://img.shields.io/github/v/release/jcdcdev/jcdcdev.Valheim.Signs?label&color=3c4834) [![Thunderstore Release](https://img.shields.io/badge/Install-Thunderstore-375a7f)](https://thunderstore.io/c/valheim/p/jcdcdev/Signs/) [![Nexus Mods Release](https://img.shields.io/badge/Install-Nexus%20Mods-b4762c)](https://www.nexusmods.com/valheim/mods/2881)

This is a BepInEx plugin for Valheim that allows you to configure signs to display in-game information.

Feel free to [suggest](https://github.com/jcdcdev/jcdcdev.Valheim.Signs/issues/new?assignees=&labels=enhancement&projects=&template=feature_request.yml&title=[Sign%20Suggestion]%20) other useful
signs!

## Features

Simply place any sign and set the text to one of the following options, e.g `{{onlineCount}}`.

### Online Count

Displays the current number of players online.

`{{onlineCount}}`

![In game screenshot of the Online Count sign](https://raw.githubusercontent.com/jcdcdev/jcdcdev.Valheim.Signs/main/docs/online-count.png)

### Death Count

Displays the current player's number of deaths on this world.

`{{deathCount}}`

![In game screenshot of the Death Count sign](https://raw.githubusercontent.com/jcdcdev/jcdcdev.Valheim.Signs/main/docs/deaths-count.png)

| Option     | Description                                                               | Example               | Example Output |
|------------|---------------------------------------------------------------------------|-----------------------|----------------|
| `playerId` | The player's name or ID.<br/>Defaults to the current player if left blank | `{{deathCount jcdc}}` | `3`            |

### Death Leaderboard

Displays the players with the most deaths on this world.

`{{deathBoard}}`

![In game screenshot of the Death Leaderboard sign](https://raw.githubusercontent.com/jcdcdev/jcdcdev.Valheim.Signs/main/docs/deaths-leaderboard.png)

| Option  | Description                                                   | Example            | Example Output              |
|---------|---------------------------------------------------------------|--------------------|-----------------------------|
| `count` | The number of players to show<br/>Defaults to 3 if left blank | `{{deathBoard 2}}` | `1. JIM: 3`<br/>`2. MIJ: 1` |

### Comfort

Displays the current comfort level of the player.

`{{comfort}}`

![In game screenshot of the Comfort sign](https://raw.githubusercontent.com/jcdcdev/jcdcdev.Valheim.Signs/main/docs/comfort.png)

### Stamina

Displays the current stamina level of the player.

`{{stamina}}`

![In game screenshot of the Comfort sign](https://raw.githubusercontent.com/jcdcdev/jcdcdev.Valheim.Signs/main/docs/stamina.png)

### Health

Displays the current health level of the player.

`{{health}}`

![In game screenshot of the Health sign](https://raw.githubusercontent.com/jcdcdev/jcdcdev.Valheim.Signs/main/docs/health.png)

## Skills

Displays the skill level of the player.

`{{skill bows}}`

![In game screenshot of the Skills sign](https://raw.githubusercontent.com/jcdcdev/jcdcdev.Valheim.Signs/main/docs/skill.png)
![In game screenshot of the Skills with Label & Emoji sign](https://raw.githubusercontent.com/jcdcdev/jcdcdev.Valheim.Signs/main/docs/skill-options.png)

#### Skills

- Axes
- Blocking
- BloodMagic
- Bows
- Clubs
- Crossbows
- ElementalMagic
- Fishing
- Jump
- Knives
- Pickaxes
- Polearms
- Ride
- Run
- Sneak
- Spears
- Swim
- Swords
- Unarmed
- WoodCutting

#### Options

| Option  | Description   | Example                | Example Output |
|---------|---------------|------------------------|----------------|
| `emoji` | Adds an emoji | `{{skill bows emoji}}` | `🏹 100`       |
| `label` | Adds a label  | `{{skill bows label}}` | `Bows 100`     |

### Game Time

Displays the current in-game time.

`{{gameTime}}`

![In game screenshot of the Game Time sign](https://raw.githubusercontent.com/jcdcdev/jcdcdev.Valheim.Signs/main/docs/game-time.png)
![In game screenshot of the Game Time Emoji sign](https://raw.githubusercontent.com/jcdcdev/jcdcdev.Valheim.Signs/main/docs/emoji-clock.png)

#### Options

| Option  | Description               | Example              | Example Output |
|---------|---------------------------|----------------------|----------------|
| `12`    | Outputs in 12 hour format | `{{gameTime 12}}`    | `12:00 AM`     |
| `s`     | Adds seconds to the time  | `{{gameTime s}}`     | `12:00:00`     |
| `emoji` | Outputs as an emoji       | `{{gameTime emoji}}` | `🕛`           |

### Actual Time

Displays the current time.

`{{actualTime}}`

![In game screenshot of the Actual Time sign](https://raw.githubusercontent.com/jcdcdev/jcdcdev.Valheim.Signs/main/docs/time.png)

#### Options

| Option  | Description                | Example              | Example Output            |
|---------|----------------------------|----------------------|---------------------------|
| `12`    | Outputs in 12 hour format  | `{{actualTime 12}}`  | `12:00 AM`                |
| `s`     | Adds seconds to the time   | `{{actualTime s}}`   | `12:00:00`                |
| `f`     | Outputs full date and time | `{{actualTime f}}`   | `06 September 2024 09:08` |
| `g`     | Outputs date and time      | `{{actualTime g}}`   | `06/09/2024 09:08`        |
| `emoji` | Outputs as an emoji        | `{{gameTime emoji}}` | `🕛`                      |

### Day Percentage

Displays the current day percentage.

`{{dayPercentage}}`

![In game screenshot of the Day Percentage sign](https://raw.githubusercontent.com/jcdcdev/jcdcdev.Valheim.Signs/main/docs/day-percent.png)

### Night Countdown

Displays the (real) time in minutes until the night.

`{{nightCountdown}}`

![In game screenshot of the Night Countdown sign](https://raw.githubusercontent.com/jcdcdev/jcdcdev.Valheim.Signs/main/docs/night-countdown.png)

#### Options

| Option | Description              | Example                | Example Output |
|--------|--------------------------|------------------------|----------------|
| `s`    | Adds seconds to the time | `{{nightCountdown s}}` | `02:12`        |

### Smelter/Eitr Content

Displays the contents of the nearest smelter or eitr.

`{{smelterContent}}`

![In game screenshot of the Smelter Content sign](https://raw.githubusercontent.com/jcdcdev/jcdcdev.Valheim.Signs/main/docs/smelter.png)

#### Options

| Option | Description        | Example                | Example Output |
|--------|--------------------|------------------------|----------------|
| `l`    | Removes the label  | `{{smelterContent l}}` | `10`<br>`5`    |
| `f`    | Only displays fuel | `{{smelterContent f}}` | `WOOD: 10`     |
| `o`    | Only displays ore  | `{{smelterContent o}}` | `COPPER: 5`    |

## Installation

This mod **must** be installed on the server and all clients to work.
Players will not be able to join the server/your game if they do not have the mod installed.

_If you're using a mod installer, you can likely ignore this section._

### Installation

#### Install BepInEx

- Download [BepInEx](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/),
- Extract everything inside `BepInEx_Valheim` into your Valheim folder
    - e.g `C:/<PathToYourSteamLibrary>/steamapps/common/Valheim`

#### Install jcdcdev.Valheim.Signs

- Download the mod
- Extract the ZIP
- Place the `jcdcdev.Valheim.Signs` folder into `BepInEx/plugins` of your Valheim folder.
