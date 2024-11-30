﻿let usernameInfo = document.getElementById("usernameInfo").innerText;
let displayUsername = document.getElementById("displayUsername").innerText;

//get all fields
let rankImage = document.getElementById("bigRankImage");
let username = document.getElementById("username");
let tr = document.getElementById("tr");
let globalText = document.getElementById("globalText");
let globalRank = document.getElementById("globalRank");
let localRank = document.getElementById("localRank");
let apm = document.getElementById("apm");
let pps = document.getElementById("pps");
let vs = document.getElementById("vs");
let countryImage = document.getElementById("countryImage");
let topRankImage = document.getElementById("topRankImage");
let prevRankImage = document.getElementById("prevRankImage");
let nextRankImage = document.getElementById("nextRankImage");
let progressBar = document.getElementById("progressBar");
let topRankText = document.getElementById("topRankText");
let progressBarBackgrounds = document.getElementById("progressBarBackground");
let lastRank = document.getElementById("lastRank");

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
    const { protocol, hostname, port } = window.location;

    let url = `${protocol}//${hostname}${port ? `:${port}` : ''}/tetraleague/${usernameInfo}/stats`

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

            let placements = data.gamesPlayed < 10;

            rankImage.src = `${imgUrl + data.rank}.png`;
            topRankImage.src = `${imgUrl + data.topRank}.png`;
            prevRankImage.src = `${imgUrl + data.prevRank}.png`;

            if (data.rank == "x+")
                nextRankImage.src = `${imgUrl + "leaderboard1"}.png`;
            else {

                nextRankImage.src = `${imgUrl + data.nextRank}.png`;
            }

            if (data.rank == "d") {
                prevRankImage.src = null;
                prevRankImage.style.display = "none";
                animateValue(lastRank, parseInt(lastRank.innerText.replace('#', '')), data.prevAt, animationDuration, true, "#", "");
            } else {
                lastRank.style.display = "none";
                prevRankImage.style.display = "block";
            }

            if (data.country == null)
                countryImage.style.display = "none";
            else {
                countryImage.style.display = "block";
                countryImage.src = `https://tetr.io/res/flags/${data.country.toLowerCase()}.png`;
            }

            if (data.countryRank == -1) {
                localRank.innerText = "";
            }

            if (!placements) {
                if(data.rank == "z"){
                    progressBarBackground.style.display = "none";
                    progressBar.style.display = "none";
                    prevRankImage.style.display = "none";
                    nextRankImage.style.display = "none";

                    globalText.style.display = "none";
                    localRank.style.display = "none";
                    globalRank.style.display = "none";
                    countryImage.style.display = "none";
                }else{
                    globalText.style.display = "";
                    localRank.style.display = "";
                    globalRank.style.display = "";
                    countryImage.style.display = "";

                    topRankImage.style.display = "";

                    progressBarBackground.style.display = "";
                    progressBar.style.display = "";
                    prevRankImage.style.display = "";
                    nextRankImage.style.display = "";
                    topRankText.style.display = "";
                }
            } else {
                globalText.style.display = "none";
                localRank.style.display = "none";
                globalRank.style.display = "none";
                countryImage.style.display = "none";
                topRankImage.style.display = "none";

                progressBarBackground.style.display = "none";
                progressBar.style.display = "none";
                prevRankImage.style.display = "none";
                nextRankImage.style.display = "none";
                topRankText.style.display = "none";
            }

            username.innerText = data.username.toUpperCase();

            if (placements && data.tr < 0) {
                tr.innerText = `${data.gamesPlayed} / 10 PLACEMENTS`;
            } else {
                animateValue(tr, parseFloat(tr.innerText), data.tr, animationDuration, false, "", " TR");
            }

            animateValue(apm, parseFloat(apm.innerText), data.apm, animationDuration);
            animateValue(pps, parseFloat(pps.innerText), data.pps, animationDuration);
            animateValue(vs, parseFloat(vs.innerText), data.vs, animationDuration);
            animateValue(globalRank, parseInt(globalRank.innerText.replace('#', '')), data.globalRank, animationDuration, true, "# ", "");
            animateValue(localRank, parseInt(localRank.innerText.replace('#', '')), data.countryRank, animationDuration, true, "# ", "");


            let range = data.prevAt - data.nextAt;
            let distance = data.prevAt - data.globalRank;
            let rankPercentage = (distance / range) * 100;

            progressBar.style.width = `${rankPercentage}%`;
        })
        .catch(error => {
            console.error('There has been a problem with your fetch operation:', error);
        });
}

updateStats();

setInterval(updateStats, 15000);