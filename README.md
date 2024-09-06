## jcdcdev.Valheim.Signs

This is a BepInEx plugin for Valheim that allows you to configure signs to display in-game information.

Feel free to [suggest](https://github.com/jcdcdev/jcdcdev.Valheim.Signs/issues/new?assignees=&labels=enhancement&projects=&template=feature_request.yml&title=[Sign%20Suggestion]%20) other useful signs!

## Features

Simply place any sign and set the text to one of the following options, e.g `{{onlineCount}}`.

### Online Count

Displays the current number of players online.

`{{onlineCount}}`

![In game screenshot of the Online Count sign](https://raw.githubusercontent.com/jcdcdev/jcdcdev.Valheim.Signs/main/docs/online-count.png)

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

![In game screenshot of the Comfort sign](https://raw.githubusercontent.com/jcdcdev/jcdcdev.Valheim.Signs/main/docs/health.png)

### Game Time

Displays the current in-game time.

`{{gameTime}}`

![In game screenshot of the Game Time sign](https://raw.githubusercontent.com/jcdcdev/jcdcdev.Valheim.Signs/main/docs/game-time.png)

#### Options

| Option | Description                | Example           | Example Output            |
|--------|----------------------------|-------------------|---------------------------|
| `12`   | Outputs in 12 hour format  | `{{gameTime 12}}` | `12:00 AM`                |
| `s`    | Adds seconds to the time   | `{{gameTime s}}`  | `12:00:00`                |

### Actual Time

Displays the current time.

`{{actualTime}}`

![In game screenshot of the Actual Time sign](https://raw.githubusercontent.com/jcdcdev/jcdcdev.Valheim.Signs/main/docs/time.png)

#### Options

| Option | Description                | Example           | Example Output            |
|--------|----------------------------|-------------------|---------------------------|
| `12`   | Outputs in 12 hour format  | `{{gameTime 12}}` | `12:00 AM`                |
| `s`    | Adds seconds to the time   | `{{gameTime s}}`  | `12:00:00`                |
| `f`    | Outputs full date and time | `{{gameTime f}}`  | `06 September 2024 09:08` |
| `g`    | Outputs date and time      | `{{gameTime g}}`  | `06/09/2024 09:08`        |

### Installation

_If you're using a mod installer, you can likely ignore this section._

#### Install BepInEx

Download [BepInEx](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/), extract everything inside `BepInEx_Valheim` into your Valheim folder (
typically `C:\<PathToYourSteamLibrary>\steamapps\common\Valheim`).

#### Install jcdcdev.Valheim.Signs

Download from Thunderstore, extract the ZIP and place all content into `BepInEx/plugins` of your Valheim install.
