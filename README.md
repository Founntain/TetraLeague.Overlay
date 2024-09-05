# TetraLeague Overlay

A simple overlay for displaying your Tetra League (TL) stats in OBS using a Browser Source. You can choose to display your stats as a static image or a live view that updates every 60 seconds.

If you find this overlay useful, please consider giving it a ⭐ to show your support, or share it with others who might enjoy it. Thank you for using it!

## 🎖️ Usage

To use the overlay, simply use one of the following URLs:

**Base URL**: `https://tetrio.founntain.dev/<mode>/<username>/web`

### Tetra League
- **Live View**: `https://tetrio.founntain.dev/tetraleague/<username>/web`
- **Static Image**: `https://tetrio.founntain.dev/tetraleague/<username>`

### 40 Lines
- **Live View**: `https://tetrio.founntain.dev/sprint/<username>/web`
- **Static Image**: `https://tetrio.founntain.dev/sprint/<username>`

### Blitz
- **Live View**: `https://tetrio.founntain.dev/blitz/<username>/web`
- **Static Image**: `https://tetrio.founntain.dev/blitz/<username>`

### Quick Play
- **Live View**: `https://tetrio.founntain.dev/zenith/<username>/web`
- **Static Image**: `https://tetrio.founntain.dev/zenith/<username>`

### 📽️ OBS Setup

For OBS, it is recommended to use the live view URL. To set it up:

1. Create a new Browser Source in OBS.
2. Paste the live view URL into the Browser Source settings, replacing `<username>` with your Tetr.io username (make sure to remove the `<` and `>`), the same goes for `<mode>`.
3. Make sure the width and height is correct check below what sizes are best for each overlay:
   - Tetra League: 900 x 300
   - 40 Lines: 700 x 225
   - Blitz: 700 x 225
   - Quick Play: 900 x 300

> **Note:** Ensure you use the URL ending with `/web` for live updates. The data is cached and refreshes every 30 seconds. For 40 Lines and Blitz the default cache is used which is **5 minutes**.

### 🛠️ Customization Parameters
You can put any parameters at the end of the url. ***The order does not matter, however spelling is!***

- **`backgroundColor`**: Adjusts the background color (useful if you don’t want a transparent background). Default is `00FFFFFF`.
- **`textColor`**: Changes the text color and the color of the progress bar. Default is `FFFFFF`.
- **`displayUsername`**: This only works for 40L and Blitz if set to `false` it will hide the username.

> **Example:** `https://tetrio.founntain.dev/tetraleague/<username>/web?backgroundColor=FF0000&textColor=00FF00`

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

#### Sprint
![founntain](https://github.com/user-attachments/assets/d7f00e47-326a-477f-9d74-0f8c46f66845)

#### Blitz
![founntain](https://github.com/user-attachments/assets/51ac5ca5-27be-465d-8b48-07b1e4e029e3)

#### Quick Play
![founntain](https://github.com/user-attachments/assets/ef55ea23-82c6-4f24-acc1-75ee8f0853c7)


