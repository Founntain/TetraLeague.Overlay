# TetraLeague Overlay

A simple overlay for displaying your Tetra League (TL) stats in OBS using a Browser Source. You can choose to display your stats as a static image or a live view that updates every 30 seconds.

If you find this overlay useful, please consider giving it a ⭐ to show your support, or share it with others who might enjoy it. Thank you for using it!

## 🎖️ Usage

For screenshots and examples look at the [examples section](#examples) at the bottom
To use the overlay, simply use one of the following URLs:

**Base URL**: `https://tetrio.founntain.dev/<mode>/<username>/web`

### Tetra League
- **Live View**: `https://tetrio.founntain.dev/tetraleague/<username>/web`
- **Static Image**: `https://tetrio.founntain.dev/tetraleague/<username>`

### Quick Play
- **Live View**: `https://tetrio.founntain.dev/zenith/<username>/web`
- **Static Image**: `https://tetrio.founntain.dev/zenith/<username>`

### 40 Lines
- **Live View**: `https://tetrio.founntain.dev/sprint/<username>/web`
- **Static Image**: `https://tetrio.founntain.dev/sprint/<username>`

### Blitz
- **Live View**: `https://tetrio.founntain.dev/blitz/<username>/web`
- **Static Image**: `https://tetrio.founntain.dev/blitz/<username>`

### QP Speedrun Splits
- **Live View**: `https://tetrio.founntain.dev/zenith/splits/<username>/web`
- **Static Image**: `https://tetrio.founntain.dev/zenith/splits/<username>`
> for more info check the [splits section](#Splits).


### 📽️ OBS Setup

For OBS, it is recommended to use the live view URL. To set it up:

1. Create a new Browser Source in OBS.
2. Paste the live view URL into the Browser Source settings, replacing `<username>` with your Tetr.io username (make sure to remove the `<` and `>`), the same goes for `<mode>`.
3. Make sure the width and height is correct check below what sizes are best for each overlay:
   - Slide: 900 x 300
   - Tetra League: 900 x 300
   - Quick Play: 900 x 300
   - 40 Lines: 700 x 225
   - Blitz: 700 x 225
   - Speedrun splits: 1500 x 200

> [!NOTE]  
> Ensure you use the URL ending with `/web` for live updates. The data is cached and refreshes every 30 seconds. For 40 Lines and Blitz the default cache is used which is **5 minutes**.

### 🛠️ Customization Parameters
You can put any parameters at the end of the url. ***The order does not matter, however spelling is!***

- **`backgroundColor`**: Adjusts the background color (useful if you don’t want a transparent background). Default is `00FFFFFF`.
- **`textColor`**: Changes the text color and the color of the progress bar. Default is `FFFFFF`.
- **`displayUsername`**: This only works for 40L, Blitz and Quick Play if set to `false` it will hide the username.
- **`modes`**: See [Slide section](#slide).

> **Example:** `https://tetrio.founntain.dev/tetraleague/founntain/web?backgroundColor=FF0000&textColor=00FF00`

## Slide

> [!IMPORTANT]
> The slide has `displayUsername` set to `false` because the username is always visible on the Tetra League overlay. On other pages, the mode is shown instead. However, you can set `displayUsername` to `true` to display the username instead of the modes.

The slide functions similarly to the other browser source, but it cycles through the modes. It essentially acts as one browser source to rule them all! To use the slide, you can use the URL of any mode—just replace `/web` with `/slide`. By default, the slide shows all available modes, but if you want to display specific modes, you can use the `modes=` parameter.  
Here are some examples of how to use the `modes=` parameter:
- `modes=tetraleague,zenith`
- `modes=sprint,blitz`

#### Available Modes
| Mode         | Parameter     |
|--------------|---------------|
| Tetra League | `tetraLeague` |
| Quick Play   | `zenith`      |
| 40 Lines     | `sprint`      |
| Blitz        | `blitz`       |

*Example URL that displays a slide for `founntain` with the modes `tetraLeague` and `zenith`: `https://tetrio.founntain.dev/zenith/founntain/slide?modes=tetraleague,zenith`*

> [!NOTE]
> The duration for which something is visible can't be changed. The duration depends on how many modes you display; the slide takes 30 seconds to return to its starting point. The more modes, the faster it switches; the fewer modes, the longer each is displayed. This is to prevent API spam. If you use the slide for a single overlay, it has the same update time as the single overlay, with just more overhead. Only use the slide if you want to display more than one mode.

> [!CAUTION]
> Parameters need to be separated by a single `comma` (`,`) otherwise you will receive an error.

## Splits

Ever wanted to know how fast you clear floors without hitting hyperspeed, or not able to hit hyperspeed ever, to see the splits in general? We have you covered. With the splits overlay, you can compare your times with your gold splits from this week.  
The splits overlay uses your last 100 games from the current week; all games past that aren't counted. We chose this limit because we don't want to make excessive API calls, and the last 100 games is enough to compare your performance this week. If you haven't played this week yet, we use your career best splits for display.  
With the `expert` parameter you can show the splits for expert or normal, default value is `false`

![blink](https://github.com/user-attachments/assets/592daaf8-0e03-412f-be87-7de6274f5a15)

> [!CAUTION]
> The splits overlay is still very early and pretty complex. If something does not work as intended, or is not working at all, please let me know so I can look into it!  
> ***The splits overlay is not supported in slide mode, because it is quite large. Maybe in the future.***

## 🏠 Running Locally

If you prefer not to use the hosted version, you have a few alternatives:

- Clone the repository, build, and run the project locally, then access it via `localhost`.
- Pull the [Docker image](https://hub.docker.com/repository/docker/founntain/tetraleague.overlay.api/general) and run it with Docker, accessing it via `localhost` and the assigned port.

## 🔨 Contributing

Contributions are welcome! Feel free to open issues, request features, provide feedback, report bugs, or even contribute code. If you're unsure about anything, let's discuss it. Thanks for your support!

## 🧡Special Thanks

- **[osk](https://tetr.io)**: for creating Tetr.io and providing an amazing, well-structured API.
- **[Veggie_Dog](https://www.twitch.tv/theveggiedog)**: motivating to make this project a reality, testing and feedback
- **[PixelAtc](https://www.twitch.tv/pixelatc)**: providing feedback, ideas and spreading the word
- **[ZaptorZap](https://zaptorz.app/)**: for giving feedback and some incredible ideas

## Example
> *added for all examples a simple transparent dark background so those are readable for light github users aswell Parameter used `backgroundColor=AA000000`*
#### Tetra League
![founntain](https://github.com/user-attachments/assets/ee8e60e9-de03-4b89-b197-cee5d3e7f8c8)

#### Quick Play
![founntain](https://github.com/user-attachments/assets/8c6cd129-5a58-4d05-a00a-8ea995d8080f)

#### Speedrun splits
![blink](https://github.com/user-attachments/assets/592daaf8-0e03-412f-be87-7de6274f5a15)

#### Sprint
![founntain](https://github.com/user-attachments/assets/d7f00e47-326a-477f-9d74-0f8c46f66845)

#### Blitz
![founntain](https://github.com/user-attachments/assets/51ac5ca5-27be-465d-8b48-07b1e4e029e3)
