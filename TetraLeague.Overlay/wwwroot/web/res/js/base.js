const { protocol, hostname, port } = window.location;
const baseUrl = `${protocol}//${hostname}${port ? `:${port}` : ''}`;
const animationDuration = 500;
const imgUrl = "/web/res/img/";

function lerp(a, b, t) {
    return a + (b - a) * t;
}

function lerpInt(a, b, t) {
    return (a + (b - a) * t).toFixed(0);
}

function lerpText(start, end, t) {
    const length = Math.floor(start.length + (end.length - start.length) * t);
    let result = '';

    for (let i = 0; i < length; i++) {
        const startCharCode = i < start.length ? start.charCodeAt(i) : ' '.charCodeAt(0);
        const endCharCode = i < end.length ? end.charCodeAt(i) : ' '.charCodeAt(0);

        const charCode = Math.floor(startCharCode + (endCharCode - startCharCode) * t);
        result += String.fromCharCode(charCode);
    }

    return result;
}

function animateValue(element, start, end, duration, lerpType = 0, prefix = "", suffix = "") {
    let currentValue;

    if(start === end){

        switch (lerpType) {
            case 0:
                currentValue = parseFloat(start.toFixed(2)).toLocaleString('en-US');
                break;
            case 1:
                currentValue = parseInt(start).toLocaleString('en-US');
                break;
            default:
                currentValue = start;
                break;
        }

        element.innerText = `${prefix}${currentValue}${suffix}`
        return;
    }

    let startTime = null;

    function animation(currentTime) {
        if (!startTime) startTime = currentTime;
        const elapsed = currentTime - startTime;

        // Calculate the progress (0 to 1)
        const progress = Math.min(elapsed / duration, 1);

        switch (lerpType) {
            case 0:
                currentValue = parseFloat(lerp(start, end, progress).toFixed(2)).toLocaleString('en-US');
                break;
            case 1:
                currentValue = parseInt(lerpInt(start, end, progress)).toLocaleString('en-US');
                break;
            case 2:
                currentValue = lerpText(start, end, progress);
        }

        element.innerText = `${prefix}${currentValue}${suffix}`

        // Continue the animation if not yet complete
        if (progress < 1) {
            requestAnimationFrame(animation);
        }
    }

    // Start the animation
    requestAnimationFrame(animation);
}