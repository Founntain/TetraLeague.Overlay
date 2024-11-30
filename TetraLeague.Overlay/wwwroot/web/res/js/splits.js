let usernameInfo = document.getElementById("usernameInfo").innerText;
let expertInfo = document.getElementById("expertInfo").innerText;

let containers = document.getElementsByClassName("floorBoxWrapper")

const animationDuration = 500;

function lerp(a, b, t) {
    return a + (b - a) * t;
}

function lerpInt(a, b, t) {
    return (a + (b - a) * t).toFixed(0);
}

function animateValue(element, start, end, duration, isInt = false, prefix = "", suffix = "") {
    let startTime = null;

    function animation(currentTime) {
        if (!startTime) startTime = currentTime;
        const elapsed = currentTime - startTime;

        // Calculate the progress (0 to 1)
        const progress = Math.min(elapsed / duration, 1);

        // Interpolate the current value
        const currentValue = isInt ? lerpInt(start, end, progress) : lerp(start, end, progress);

        // Update the element's text content
        if (isInt)
            element.innerText = prefix + currentValue + suffix;
        else
            element.innerText = prefix + currentValue.toFixed(2) + suffix;

        // Continue the animation if not yet complete
        if (progress < 1) {
            requestAnimationFrame(animation);
        }
    }

    // Start the animation
    requestAnimationFrame(animation);
}

function updateStats() {
    const {protocol, hostname, port} = window.location;

    let url = `${protocol}//${hostname}${port ? `:${port}` : ''}/zenith/splits/${usernameInfo}/stats?expert=${expertInfo}`

    console.log(url)

    fetch(url)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            return response.json();
        })
        .then(data => {
            console.log(data);
            let notReached = false;
            let endReached = false;

            for (let i = 0; i < containers.length; i++) {

                let floorBox = containers[i];
                let floorData = data[i];


                if(endReached){
                    floorBox.parentElement.style.display = "none";
                }else{
                    floorBox.parentElement.style.display = "flex";
                }

                let floorTime = floorBox.getElementsByClassName("floorTime")[0];
                let floorDifference = floorBox.getElementsByClassName("floorDifference")[0];

                if(floorData.split === 0){
                    floorTime.innerText = "DNF";
                    endReached = true;
                }else{
                    floorTime.innerText = floorData.splitTime;
                }

                if(floorData.notReached === true){
                    if(notReached === false)
                        floorDifference.innerText = "NOT REACHED"
                    else{
                        floorDifference.innerText = " "
                    }
                    floorDifference.classList.add("notReached");
                    notReached = true;
                    continue;
                }else{
                    floorDifference.classList.remove("notReached");
                }

                var prefix = "";

                if(floorData.isSlower){
                    prefix = "+";

                    floorDifference.classList.remove("faster");
                    floorDifference.classList.add("slower");
                }else{
                    prefix = "-";

                    floorDifference.classList.remove("slower");
                    floorDifference.classList.add("faster");
                }

                if(floorData.differenceToGold === 0 && floorData.differenceToSecondGold > 0){
                    floorDifference.innerText = prefix + floorData.timeDifferenceToSecondGold;
                }else{
                    floorDifference.innerText = prefix + floorData.timeDifferenceToGold;
                }
            }
        })
        .catch(error => {
            console.error('There has been a problem with your fetch operation:', error);
        });
}

updateStats();

setInterval(updateStats, 10000);