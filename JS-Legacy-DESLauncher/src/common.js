(function () {
    var fs = require("fs");

    function getManifest(dir) {
        var path = dir + "/manifest.json";
        if (fs.existsSync(path)) {
            var str = fs.readFileSync(path);
            return jsonparse(str);
        }
        return -1;
    }

    function jsonparse(text) {
        try {
            return JSON.parse(text);
        } catch (e) {
            error(e);
            if (debug) error(e.stack);
            return -1;
        }
    }

    function error(err) {
        alert(err);
    }

    function log(err) {
        console.log(err);
    }

    module.exports = {
        fs: fs,
        getManifest: getManifest,
        jsonparse: jsonparse,
        error: error,
        log: log
    }
})();