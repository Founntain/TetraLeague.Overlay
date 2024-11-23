let usernameInfo = document.getElementById("usernameInfo").innerText;

//get all fields
let thisWeeksNormalScore = document.getElementById("thisWeeksNormalScore");
let pps = document.getElementById("pps");
let apm = document.getElementById("apm");
let vs = document.getElementById("vs");
let normalPersonalBest = document.getElementById("normalPersonalBest");
let mods = document.getElementById("mods");

let thisWeeksExpertScore = document.getElementById("thisWeeksExpertScore");
let ppsExpert = document.getElementById("ppsExpert");
let apmExpert = document.getElementById("apmExpert");
let vsExpert = document.getElementById("vsExpert");
let expertPersonalBest = document.getElementById("expertPersonalBest");
let modsExpert = document.getElementById("modsExpert");

let expertContainer = document.getElementById("expertContainer");

const imgUrl = "/web/res/img/";

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

    let url = `${protocol}//${hostname}${port ? `:${port}` : ''}/zenith/${usernameInfo}/stats`

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

            // NORMAL
            animateValue(thisWeeksNormalScore, parseFloat(thisWeeksNormalScore.innerText), data.zenith.altitude, animationDuration, false, "", " M");

            animateValue(pps, parseFloat(pps.innerText), data.zenith.pps, animationDuration, false, "", " PPS");
            animateValue(apm, parseFloat(apm.innerText), data.zenith.apm, animationDuration, false, "", " APM");
            animateValue(vs, parseFloat(vs.innerText), data.zenith.vs, animationDuration, false, "", " VS");

            animateValue(normalPersonalBest, parseFloat(normalPersonalBest.innerText), data.zenith.best, animationDuration, false, "", " M");

            mods.innerHTML = "";

            data.zenith.mods?.forEach(mod => {
                const img = document.createElement('img');
                img.classList.add("mod");
                img.src = `${imgUrl}${mod}.png`;
                mods.appendChild(img);
            });

            // EXPERT

            if(data.expertPlayed == false){
                expertContainer.style.display = "none";
            }else{
                expertContainer.style.display = "block";

                animateValue(thisWeeksExpertScore, parseFloat(thisWeeksExpertScore.innerText), data.zenithExpert.altitude, animationDuration, false, "", " M");

                animateValue(ppsExpert, parseFloat(ppsExpert.innerText), data.zenithExpert.pps, animationDuration, false, "", " PPS");
                animateValue(apmExpert, parseFloat(apmExpert.innerText), data.zenithExpert.apm, animationDuration, false, "", " APM");
                animateValue(vsExpert, parseFloat(vsExpert.innerText), data.zenithExpert.vs, animationDuration, false, "", " VS");

                animateValue(expertPersonalBest, parseFloat(expertPersonalBest.innerText), data.zenithExpert.best, animationDuration, false, "", " M");

                modsExpert.innerHTML = "";

                data.zenithExpert.mods.forEach(mod => {
                    const img = document.createElement('img');
                    img.classList.add("mod");
                    img.src = `${imgUrl}${mod}.png`;
                    modsExpert.appendChild(img);
                });
            }
        })
        .catch(error => {
            console.error('There has been a problem with your fetch operation:', error);
        });
}

updateStats();

setInterval(updateStats, 30000);