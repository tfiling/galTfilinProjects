var i = 0;

function timedCount() {
    var now = new Date().getTime();
    while (new Date().getTime() < now + 500) { /* do nothing */ }
    setTimeout("timedCount()", 500);
    postMessage("shit");
}

timedCount();