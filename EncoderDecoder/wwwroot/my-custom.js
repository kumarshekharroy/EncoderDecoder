window.blazoredLocalisation = {
    getBrowserLocale: function () {
        return navigator.languages && navigator.languages.length ? navigator.languages[0] : navigator['userLanguage']
            || navigator.language
            || navigator['browserLanguage']
            || 'en';
    }
};
window.clipboardCopy = {
    copyText: function (text) {
        return navigator.clipboard.writeText(text).then(function () {
            return "success";
           // alert("Copied to clipboard!");
        })
            .catch(function (error) {
                //alert(error);
            });
    }
};

setTitle = (title) => { document.title = title; };

setFevicon = (imagename) => {
    var link = document.querySelector("link[rel*='icon']") || document.createElement('link');
    link.type = 'image/png';
    link.rel = 'shortcut icon';
    link.href = '/' + imagename;
    document.getElementsByTagName('head')[0].appendChild(link);
}