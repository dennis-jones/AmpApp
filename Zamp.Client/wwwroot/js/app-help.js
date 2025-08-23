showAppHelp = function (helpStateName) {
    const w = 500;
    const h = document.documentElement.clientHeight;

    const dualScreenLeft = typeof window.screenLeft !== "undefined" ? window.screenLeft : screen.left;
    const dualScreenTop = typeof window.screenTop !== "undefined" ? window.screenTop : screen.top;

    const width = window.innerWidth ? window.innerWidth :
        document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
    const height = window.innerHeight ? window.innerHeight :
        document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;

    //var left = ((width / 2) - (w / 2)) + dualScreenLeft; // to centre the new window
    const left = (width - w - 20) + dualScreenLeft;
    const top = ((height / 2) - (h / 2)) + dualScreenTop;
    const newWindow =
        window.open("help/" + helpStateName, "appHelpWindow", 'toolbar=0,scrollbars=1, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
};
