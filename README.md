# TetraLeague Overlay
A simple overlay to display your TL stats in OBS via Browser Source.
You can either get your stats as an image or as a live view that updates every 60 seconds.

If you like the overlay I would really appreciate it if you drop a ⭐ and show me that you like the work! Thanks for using it!

## Usage
You can access it by using the following url:
- For the live view: `https://tetrio.founntain.dev/tetraleague/stats/<username>/web`
- For a simple image: `https://tetrio.founntain.dev/tetraleague/stats/<username>`

### OBS
For OBS use the live view one. Create a new Browser Source and paste it in and you are good to go. Just replace <username> with your tetr.io username (don't forget to remove the <>). ***Make sure that you use the one that ends with `/web` otherwise it won't auto update***

> The data is cached, so don't expect changes immediately the live view we updates every 60 seconds, but it only fetches the tetra channel API when the cache expired. Tetra League data is cached for 5 minutes, which means worst update time is 6 minutes

### Parameters
- `backgroundColor`: this changes the background color (in case you don't feel transparent); default is `00FFFFFF`
- `textColor`: Changes the text color and color of the progress bar; default is `FFFFFF`
> Example: `https://tetrio.founntain.dev/tetraleague/stats/<username>/web?backgroundColor=FF0000&textColor=00FF00`

## Running localy
If you don't want to use the hosted version provided by me, you have some alternatives.
- Pull, build and run the project and access it via `localhost`
- Pull the ![docker image](https://hub.docker.com/repository/docker/founntain/tetraleague.overlay.api/general) and run it with docker and also access it via `localhost` and your given port

## Contributing
Feel free to open issues and request features, send feedback or report bugs or even contribute to the code base as well.
If unsure we can talk about it. Thanks

## Special Thanks
Special thanks to osk for creating tetr.io and an amazing API that is easy to use and well structured. ❤️

## Example
![](https://tetrio.founntain.dev/tetraleague/stats/founntain)
