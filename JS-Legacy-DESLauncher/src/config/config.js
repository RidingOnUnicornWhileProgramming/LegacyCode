(function () {
    if (nw == null) { console.log("This script requires Node-Webkit >=0.13"); process.exit(-1); }
    if (window == null) { console.log("This script requires config.html to be launcher by Node-Webkit >=0.13"); process.exit(-1); }

    var LDIR = "../";
    var PKGDIR = LDIR + "packages/";
    var USRDIR = LDIR + "user/"; // TODO maybe move to AppData
    var USRCFG = USRDIR + "settings.json";

    var common = require(LDIR + "common.js");

    var manifests = {};
    var $ = function (id) { return document.querySelector(id); };
    var objs = {
        mps: document.querySelector("#myPanels_select"),
        aws: document.querySelector("#avalWidgets_select")
    }
    var mpsItems = [];
    var awsItems = [];

    preloadManifests();
    loadDataFromFile();

    function preloadManifests() {
        var folders = common.fs.readdirSync(PKGDIR);
        for (var i = 0, length = folders.length; i < length; i++) {
            var dir = PKGDIR + folders[i];
            manifests[dir] = common.getManifest(dir);
        }
    }

    function loadDataFromFile() {
        if (common.fs.existsSync(USRCFG)) {
            var str = common.fs.readFileSync(USRCFG);
            var obj = common.jsonparse(str);
            if (obj === -1) {
                common.error("Corrupted settings.json. Press OK to rename your config file to settings.bak and use default values.");
                common.fs.renameSync(USRCFG, USRDIR + "settings.bak");
                loadDefaultData();
            } else {
                parseData(obj);
            }
        } else {
            loadDefaultData();
        }
    }

    function loadDefaultData() {
        if (common.fs.existsSync("default_settings.json")) {
            var str = common.fs.readFileSync("default_settings.json");
            var obj = common.jsonparse(str);
            if (obj === -1) {
                common.error("Corrupted installation.");
                process.exit(-1);
            } else {
                parseData(obj);
            }
        } else {
            common.error("Corrupted installation.");
            process.exit(-1);
        }
    }

    function parseData(data) {
        parseMyPanels(data, objs.mps);
        Sortable.create(objs.mps, {
            animation: 150,
            draggable: '.draggable',
            handle: '.handle',
            group: 'widgets',
            onAdd: function (evt) {
                if (evt.item.classList.contains("panel")) {
                    Sortable.create(evt.item.querySelector(".panelInner"), {
                        animation: 150,
                        draggable: '.draggable',
                        handle: '.handle',
                        group: 'widgets',
                    });
                }
            }
        });
        parseAvalWidgets(objs.aws);
    }

    function parseMyPanels(data, el) {
        if (data.items != null) {
            for (var i = 0, length = data.items.length; i < length; i++) {
                var item = data.items[i];
                var type = item.type;
                var c = document.createElement("DIV");
                c.classList = type + " draggable";
                if (type === "widget") {
                    var result = styleElement(c, item);
                    if (result !== -1) {
                        mpsItems.push(item);
                        el.appendChild(c);
                    }
                }
                else if (type === "panel") {
                    mpsItems.push(item);
                    var container = styleGroup(c, item);
                    parseMyPanels(item, container);
                    c.appendChild(container);
                    Sortable.create(container, {
                        animation: 150,
                        draggable: '.draggable',
                        handle: '.handle',
                        group: 'widgets',
                    });
                    el.appendChild(c);
                }
            }
        }
    }

    function parseAvalWidgets(el) {
        (function () {
            var c = document.createElement("DIV");
            c.classList = "panel draggable";
            var item = {
                type: "panel",
                name: "New panel",
                width: 0,
                height: 0,
                posX: 0,
                posY: 0,
                items: []
            };
            awsItems.push(item);
            var container = styleGroup(c, item);
            c.appendChild(container);
            el.appendChild(c);
        })();

        var keys = Object.keys(manifests);
        for (var i = 0, length = keys.length; i < length; i++) {
            var item = {
                type: "widget",
                widgetClass: keys[i].replace(PKGDIR, ""),
                width: 0,
                height: 0,
                posX: 0,
                posY: 0,
                settings: {}
            };
            var c = document.createElement("DIV");
            c.classList = "widget draggable";
            var result = styleElement(c, item);
            if (result !== -1) {
                awsItems.push(item);
                el.appendChild(c);
            }
        }
        Sortable.create(el, {
            animation: 150,
            draggable: '.draggable',
            handle: '.handle',
            group: {
                name: 'widgets',
                pull: 'clone',
                put: false
            },
            sort: false
        });
    }

    //<div class="widget draggable">
    //  <div class="handle">
    //    <div class="icon"></div>
    //    <h2>Title</h2>
    //    <h3>Desc.</h3>
    //    <h4>0, 2000</h4>
    //  </div>
    //</div>
    // TODO rewrite as make("widget", c, item)
    function styleElement(c, item) {
        var dir = PKGDIR + item.widgetClass;
        var man = manifests[dir];
        if (man === -1) return -1;
        var pkg = man.package;

        var d = document.createElement("DIV");
        d.className = "handle";

        var icond = document.createElement("DIV");
        icond.className = "icon";
        if (pkg.image != null) {
            var icon = document.createElement("IMG");
            icon.src = "file://" + process.cwd() + "/" + dir + "/" + pkg.image;
            icond.appendChild(icon);
        }
        d.appendChild(icond);

        var h2 = document.createElement("H2");
        h2.innerHTML = pkg.name;
        d.appendChild(h2);

        var h3 = document.createElement("H3");
        h3.innerHTML = pkg.description;
        d.appendChild(h3);

        var h4 = document.createElement("H4");
        h4.innerHTML = item.posX + ", " + item.posY + ", " + item.width + "x" + item.height;
        d.appendChild(h4);

        c.appendChild(d);

        return 0;
    }

    //<div class="panel draggable">
    //  <div class="handle">
    //    <h2>Title</h2>
    //    <h3>0, 2000</h3>
    //  </div>
    //  <div class="panelInner">
    //    ...<div>s...
    //  </div>
    //</div>
    function styleGroup(c, item) {
        var d = document.createElement("DIV");
        d.className = "handle";

        var h2 = document.createElement("H2");
        h2.innerHTML = item.name;
        d.appendChild(h2);

        var h3 = document.createElement("H3");
        h3.innerHTML = item.posX + ", " + item.posY + ", " + item.width + "x" + item.height;
        d.appendChild(h3);

        c.appendChild(d);

        var d = document.createElement("DIV");
        d.className = "panelInner";
        return d;
    }
})();