// TODO currently VS2015 has some problems with snippets in NodeJS project, so it's normal website project
// it should be made NodeJS project ASAP
// for now Alt+Space -> debug.bat does the job

(function () {

    if (nw == null) { console.log("This script requires Node-Webkit >=0.13"); process.exit(-1); }

    var LDIR = "./";
    var PKGDIR = LDIR + "packages/";
    var USRDIR = LDIR + "user/"; // TODO maybe move to AppData
    var USRCFG = USRDIR + "settings.json";

    var execFile = require("child_process").execFile;
    var execFileSync = require("child_process").execFileSync;
    var common = require(LDIR + "common.js");

    var NODELOC = process.platform === "win32" ? "C:\\nwjs-sdk\\nw.exe" : "nw"; // TODO change

    var styleCfg;
    var runningItems = [];
    var cfgWndOpen = false;

    //process.on('exit', killTaskbar); // TODO probably not needed, children should be attached, TODO test on Linux
    process.on('SIGINT', function () { // TODO maybe remove in production
        common.log("SIGINT");
        killTaskbar();
        process.exit(0);
    });
    main();

    function main() {
        if (common.fs.existsSync(USRCFG)) {
            parseCfg();
        } else {
            configWindow();
        }
    }

    function parseCfg() {
        var str = common.fs.readFileSync(USRCFG);
        var obj = common.jsonparse(str);
        if (obj === -1) {
            common.error("Corrupted settings.json, unable to continue. Press OK to rename your config file to settings.bak and use default values.");
            common.fs.renameSync(USRCFG, USRDIR + "settings.bak");
            configWindow();
            return;
        }

        styleCfg = obj.style;
        runItems(obj, true);
        runningParser = false;
    }

    function runItems(container, deep) {
        var items = container.items;
        if (items != null) {
            for (var i = 0, length = items.length; i < length; i++) {
                var item = items[i];
                var type = item.type;
                if (type === "widget") {
                    runItem(item);
                } else if (type === "panel") {
                    runItems(item, false);
                }
            }
        }
    }

    function runItem(item) {
        if (item.width == null
            || item.height == null
            || item.posX == null
            || item.posY == null
            || item.widgetClass == null
            || item.settings == null) {

            common.error("Item config not complete " + item.widgetClass);
            return;
        }

        var dir = PKGDIR + item.widgetClass;

        var manifest = common.getManifest(dir);
        if (manifest === -1) {
            common.error("Cannot parse manifest for " + item.widgetClass);
            return;
        }
        var pkg = manifest.package;
        var settings = manifest.settings;

        var file = pkg.execFiles['all'] || pkg.execFiles[process.platform];
        if (file == null) {
            common.error("Unsupported OS for " + pkg.name);
            return;
        }

        var command, params;
        if (pkg.execType === "standalone") {
            command = file;
            params = [];
        }
        else if (pkg.execType === "nw") {
            command = NODELOC;
            params = [file];
        }
        else {
            common.error("Unsupported execType for " + item.name);
            return;
        }

        var appsettings = [];
        for (var i = 0, length = settings.length; i < length; i++) {
            var setting = settings[i];
            var settingName = setting[0];
            var value = item.settings[settingName] || setting[3];
            appsettings.push(settingName + "=" + value);
        }

        appsettings = appsettings.join('|');
        params = params.concat([
            appsettings,
            item.width, item.height,
            item.posX, item.posY
        ]).concat(styleCfg);

        var cwd = { "cwd": dir };

        (function (id) {
            var child = execFile(command, params, cwd, (error, stdout, stderr) => {
                if (error) {
                    common.error("Cannot execute " + pkg.name + " (" + error.toString() + ")");
                    err = true;
                }
                runningItems[id] = null;
            });
            runningItems[id] = child;
        }(runningItems.length)); // making sure a copy will be created (as it's used in callback)
    }

    function killTaskbar() {
        for (var i = 0, length = runningItems.length; i < length; i++) {
            if (runningItems[i] != null) {
                runningItems[i].kill("SIGINT");
            }
        }
    }

    function configWindow() {
        try {
            execFileSync(NODELOC, ["config"]);
            killTaskbar();
        } catch (error) {
            common.error("Cannot execute config!! (" + error.toString() + ")");
        }
        setTimeout(main, 1); // allow to parse events (signals)
    }

})();