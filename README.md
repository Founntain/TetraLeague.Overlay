# TetraLeague Overlay

A simple overlay for displaying your Tetra League (TL) stats in OBS using a Browser Source. You can choose to display your stats as a static image or a live view that updates every 60 seconds.

If you find this overlay useful, please consider giving it a ⭐ to show your support, or share it with others who might enjoy it. Thank you for using it!

## Usage

To use the overlay, simply use one of the following URLs:

- **Live View** (auto-updates every 60 seconds): `https://tetrio.founntain.dev/tetraleague/stats/<username>/web`
- **Static Image**: `https://tetrio.founntain.dev/tetraleague/stats/<username>`

### OBS Setup

For OBS, it is recommended to use the live view URL. To set it up:

1. Create a new Browser Source in OBS.
2. Paste the live view URL into the Browser Source settings, replacing `<username>` with your Tetr.io username (make sure to remove the `<` and `>`).
3. Make sure the widht and height is **900 x 300**

> **Note:** Ensure you use the URL ending with `/web` for live updates. The data is cached and refreshes every 60 seconds. Tetra League data is cached for 5 minutes, so the maximum delay before updates is approximately 6 minutes.

### Customization Parameters

- **`backgroundColor`**: Adjusts the background color (useful if you don’t want a transparent background). Default is `00FFFFFF`.
- **`textColor`**: Changes the text color and the color of the progress bar. Default is `FFFFFF`.

> **Example:** `https://tetrio.founntain.dev/tetraleague/stats/<username>/web?backgroundColor=FF0000&textColor=00FF00`

## Running Locally

If you prefer not to use the hosted version, you have a few alternatives:

- Clone the repository, build, and run the project locally, then access it via `localhost`.
- Pull the [Docker image](https://hub.docker.com/repository/docker/founntain/tetraleague.overlay.api/general) and run it with Docker, accessing it via `localhost` and the assigned port.

## Contributing

Contributions are welcome! Feel free to open issues, request features, provide feedback, report bugs, or even contribute code. If you're unsure about anything, let's discuss it. Thanks for your support!

## Special Thanks

Special thanks to osk for creating Tetr.io and providing an amazing, well-structured API. ❤️

## Example

![founntain](https://github.com/user-attachments/assets/0cd8dd02-c4cb-47ba-baec-f52a1ea062db)
