import * as alt from 'alt';
import * as game from 'natives';
import { LocalStorage } from 'alt';
import { showCursor, fadeScreenOut, fadeScreenIn, loadIPL, setClothes } from './utilities.js';

let loginBrowser = null,
    loginCamera = null,
    charcreatorPedHandle = null,
    charcreatorModelHash = null;
const storage = LocalStorage.get();

function createLoginBrowser() {
    if (loginBrowser != null) return;
    loginCamera = game.createCamWithParams("DEFAULT_SCRIPTED_CAMERA", 3280, 5220, 26, 0, 0, 240, 50, true, 2);
    game.setCamActive(loginCamera, true);
    game.renderScriptCams(true, false, 0, true, false);
    fadeScreenIn(500);
    game.displayRadar(false);
    game.freezeEntityPosition(alt.Player.local.scriptID, true);
    showCursor(true);
    alt.toggleGameControls(false);

    loginBrowser = new alt.WebView("http://resource/client/cef/login/index.html");
    loginBrowser.focus();
    loginBrowser.on("Client:Login:cefReady", () => {
        alt.setTimeout(() => {
            if (storage.get("username")) loginBrowser.emit("CEF:Login:setStorage", storage.get("username"), storage.get("password"));
        }, 1500);
    });

    loginBrowser.on("Client:Login:ValidataLoginCredentials", (name, password) => {
        storage.set("username", name);
        storage.set("password", password);
        storage.save();
        alt.emitServer("Server:Login:ValidateLoginCredentials", name, password);
    });
}

function switchToCharSelect(existSkin, typ) {
    if (loginBrowser == null) return;
    if (typ == 0) loginBrowser.emit("CEF:Login:fadeOut");
    else if (typ == 1) loginBrowser.emit("CEF:Charcreator:fadeOut");
    // fadeScreenOut(1000);
    alt.setTimeout(() => {
        if (loginBrowser != null) loginBrowser.destroy();
        loginBrowser = null;
        loginBrowser = new alt.WebView("http://resource/client/cef/charselector/index.html");
        loginBrowser.on("Client:Charselector:cefReady", () => {
            alt.setTimeout(() => {
                if (existSkin) openCharCreatorSceneSelect();
                else openCharCreatorGenderSelect();
            }, 250);
        });

        loginBrowser.on("Client:Charselector:spawnCharacter", () => {
            fadeScreenOut(500);
            alt.setTimeout(() => {
                destroyLoginBrowser();
                destroyLoginCam();
            }, 500);
        });
        loginBrowser.focus();
        alt.emitServer("Server:Player:setPos", parseFloat(-775.9007), parseFloat(330.3822), parseFloat(212.2218));
        destroyLoginCam();
        createLoginCam(-773.9007, 330.3822, 212.2218, 380);
        fadeScreenIn(1000);
    }, 1600);
}

function openCharCreatorSceneSelect() {
    fadeScreenOut(1000);
    alt.setTimeout(() => {
        alt.emitServer("Server:Player:setPos", parseFloat(-1837.052), parseFloat(-1182.378), parseFloat(25.49));
        loginBrowser.emit("CEF:Charselector:showLoadingscreen", 1000);
        alt.setTimeout(() => {
            createLoginCam(-1786.361, -1139.534, 43.02559, 283);
            fadeScreenIn(1000);
            alt.setTimeout(() => {
                alt.emitServer("Server:Charselector:loadCharacter");
            }, 1500);
        }, 1000);
    }, 1000);
}

function openCharCreatorGenderSelect() {
    fadeScreenOut(1000);
    alt.setTimeout(() => {
        alt.emitServer("Server:Player:setPos", parseFloat(402.778), parseFloat(-996.9758), parseFloat(-98));
    }, 1000);
    alt.setTimeout(() => {
        destroyLoginBrowser();
        fadeScreenIn(1000);
        if (loginBrowser != null) return;
        loginBrowser = new alt.WebView("http://resource/client/cef/charselector/index.html");
        loginBrowser.focus();
        loginBrowser.on("Client:Charselector:cefReady", () => {
            alt.setTimeout(() => {
                loginBrowser.emit("CEF:Charcreator:openGenderSelect");
            }, 250);
        });
        loginBrowser.on("Client:Charcreator:hideGenderSelector", (gender) => {
            if (gender == undefined) return;
            alt.setTimeout(() => {
                fadeScreenOut(500);
                alt.setTimeout(() => {
                    destroyLoginCam();
                    openCharCreator(gender);
                }, 500);
            }, 250);
        });

        loginBrowser.on("Client:Charcreator:UpdateHeadOverlays", (headoverlaysarray) => {
            let headoverlays = JSON.parse(headoverlaysarray);
            game.setPedHeadOverlayColor(charcreatorPedHandle, 1, 1, parseInt(headoverlays[2][1]), 1);
            game.setPedHeadOverlayColor(charcreatorPedHandle, 2, 1, parseInt(headoverlays[2][2]), 1);
            game.setPedHeadOverlayColor(charcreatorPedHandle, 5, 2, parseInt(headoverlays[2][5]), 1);
            game.setPedHeadOverlayColor(charcreatorPedHandle, 8, 2, parseInt(headoverlays[2][8]), 1);
            game.setPedHeadOverlayColor(charcreatorPedHandle, 10, 1, parseInt(headoverlays[2][10]), 1);
            game.setPedEyeColor(charcreatorPedHandle, parseInt(headoverlays[0][14]));
            game.setPedHeadOverlay(charcreatorPedHandle, 0, parseInt(headoverlays[0][0]), parseInt(headoverlays[1][0]));
            game.setPedHeadOverlay(charcreatorPedHandle, 1, parseInt(headoverlays[0][1]), parseFloat(headoverlays[1][1]));
            game.setPedHeadOverlay(charcreatorPedHandle, 2, parseInt(headoverlays[0][2]), parseFloat(headoverlays[1][2]));
            game.setPedHeadOverlay(charcreatorPedHandle, 3, parseInt(headoverlays[0][3]), parseInt(headoverlays[1][3]));
            game.setPedHeadOverlay(charcreatorPedHandle, 4, parseInt(headoverlays[0][4]), parseInt(headoverlays[1][4]));
            game.setPedHeadOverlay(charcreatorPedHandle, 5, parseInt(headoverlays[0][5]), parseInt(headoverlays[1][5]));
            game.setPedHeadOverlay(charcreatorPedHandle, 6, parseInt(headoverlays[0][6]), parseInt(headoverlays[1][6]));
            game.setPedHeadOverlay(charcreatorPedHandle, 7, parseInt(headoverlays[0][7]), parseInt(headoverlays[1][7]));
            game.setPedHeadOverlay(charcreatorPedHandle, 8, parseInt(headoverlays[0][8]), parseInt(headoverlays[1][8]));
            game.setPedHeadOverlay(charcreatorPedHandle, 9, parseInt(headoverlays[0][9]), parseInt(headoverlays[1][9]));
            game.setPedHeadOverlay(charcreatorPedHandle, 10, parseInt(headoverlays[0][10]), parseInt(headoverlays[1][10]));
            game.setPedComponentVariation(charcreatorPedHandle, 2, parseInt(headoverlays[0][13]), 0, 0);
            game.setPedHairColor(charcreatorPedHandle, parseInt(headoverlays[2][13]), parseInt(headoverlays[1][13]));
        });

        loginBrowser.on("Client:Charcreator:UpdateFaceFeature", (facefeaturesdata) => {
            let facefeatures = JSON.parse(facefeaturesdata);

            for (let i = 0; i < 20; i++) {
                game.setPedFaceFeature(charcreatorPedHandle, i, facefeatures[i]);
            }
        });

        loginBrowser.on("Client:Charcreator:UpdateHeadBlends", (headblendsdata) => {
            let headblends = JSON.parse(headblendsdata);
            game.setPedHeadBlendData(charcreatorPedHandle, headblends[0], headblends[1], 0, headblends[2], headblends[5], 0, headblends[3], headblends[4], 0, 0);
        });

        loginBrowser.on("Client:Charcreator:updateCam", (category) => {
            if (category == "face") updateLoginCam(402.8, -997.8, -98.3, 0, 0, 358, 50);
            else updateLoginCam(402.85, -999, -98.4, 0, 0, 358, 50);
        });

        loginBrowser.on("Client:Charcreator:CreateCharacter", (birthdate, gender, isCrimeFlagged, facefeaturesarray, headblendsdataarray, headoverlaysarray) => {
            alt.emitServer("Server:Charcreator:CreateCharacter", birthdate, parseInt(gender), isCrimeFlagged, facefeaturesarray, headblendsdataarray, headoverlaysarray);
        });
    }, 1500);
}

alt.onServer("Client:Charcreator:showError", (msg) => {
    if (loginBrowser == null) return;
    loginBrowser.emit("CEF:Charcreator:showError", msg);
});

alt.onServer("Client:Charselector:setCorrectSkin", (facefeaturesarray, headblendsarray, headoverlaysarray) => {
    alt.log(facefeaturesarray);
    let facefeatures = JSON.parse(facefeaturesarray);
    let headblends = JSON.parse(headblendsarray);
    let headoverlays = JSON.parse(headoverlaysarray);

    game.setPedHeadBlendData(alt.Player.local.scriptID, headblends[0], headblends[1], 0, headblends[2], headblends[5], 0, headblends[3], headblends[4], 0, 0);
    game.setPedHeadOverlayColor(alt.Player.local.scriptID, 1, 1, parseInt(headoverlays[2][1]), 1);
    game.setPedHeadOverlayColor(alt.Player.local.scriptID, 2, 1, parseInt(headoverlays[2][2]), 1);
    game.setPedHeadOverlayColor(alt.Player.local.scriptID, 5, 2, parseInt(headoverlays[2][5]), 1);
    game.setPedHeadOverlayColor(alt.Player.local.scriptID, 8, 2, parseInt(headoverlays[2][8]), 1);
    game.setPedHeadOverlayColor(alt.Player.local.scriptID, 10, 1, parseInt(headoverlays[2][10]), 1);
    game.setPedEyeColor(alt.Player.local.scriptID, parseInt(headoverlays[0][14]));
    game.setPedHeadOverlay(alt.Player.local.scriptID, 0, parseInt(headoverlays[0][0]), parseInt(headoverlays[1][0]));
    game.setPedHeadOverlay(alt.Player.local.scriptID, 1, parseInt(headoverlays[0][1]), parseFloat(headoverlays[1][1]));
    game.setPedHeadOverlay(alt.Player.local.scriptID, 2, parseInt(headoverlays[0][2]), parseFloat(headoverlays[1][2]));
    game.setPedHeadOverlay(alt.Player.local.scriptID, 3, parseInt(headoverlays[0][3]), parseInt(headoverlays[1][3]));
    game.setPedHeadOverlay(alt.Player.local.scriptID, 4, parseInt(headoverlays[0][4]), parseInt(headoverlays[1][4]));
    game.setPedHeadOverlay(alt.Player.local.scriptID, 5, parseInt(headoverlays[0][5]), parseInt(headoverlays[1][5]));
    game.setPedHeadOverlay(alt.Player.local.scriptID, 6, parseInt(headoverlays[0][6]), parseInt(headoverlays[1][6]));
    game.setPedHeadOverlay(alt.Player.local.scriptID, 7, parseInt(headoverlays[0][7]), parseInt(headoverlays[1][7]));
    game.setPedHeadOverlay(alt.Player.local.scriptID, 8, parseInt(headoverlays[0][8]), parseInt(headoverlays[1][8]));
    game.setPedHeadOverlay(alt.Player.local.scriptID, 9, parseInt(headoverlays[0][9]), parseInt(headoverlays[1][9]));
    game.setPedHeadOverlay(alt.Player.local.scriptID, 10, parseInt(headoverlays[0][10]), parseInt(headoverlays[1][10]));
    game.setPedComponentVariation(alt.Player.local.scriptID, 2, parseInt(headoverlays[0][13]), 0, 0);
    game.setPedHairColor(alt.Player.local.scriptID, parseInt(headoverlays[2][13]), parseInt(headoverlays[1][13]));

    for (let i = 0; i < 20; i++) {
        game.setPedFaceFeature(alt.Player.local.scriptID, i, facefeatures[i]);
    }
});

alt.onServer("Client:Charselector:spawnCharacterFinal", () => {
    fadeScreenOut(500);
    alt.setTimeout(() => {
        destroyLoginBrowser();
        destroyLoginCam();
    }, 600);

    alt.setTimeout(() => {
        fadeScreenIn(1000);
        game.displayRadar(true);
        game.freezeEntityPosition(alt.Player.local.scriptID, false);
        showCursor(false);
        alt.toggleGameControls(true);
    }, 1500);
});

alt.onServer("Client:Login:destroyBrowser", () => {
    destroyLoginBrowser();
    showCursor(false);
    fadeScreenOut(450);
    alt.setTimeout(() => {
        destroyLoginCam();
        game.displayRadar(true);
        game.freezeEntityPosition(alt.Player.local.scriptID, false);
        alt.toggleGameControls(true);
    }, 500);
});

function openCharCreator(gender) {
    if (loginBrowser == null || gender == undefined) return;
    alt.emitServer("Server:Charcreator:prepare");
    alt.setTimeout(() => {
        spawnCreatorPed(gender);
        createLoginCam(402.85, -999, -98.4, 358);
        fadeScreenIn(1000);
        loginBrowser.emit("CEF:Charcreator:openCreator");
    }, 1500);
}

function spawnCreatorPed(gender) { //gender (0 - male | 1 - female)
    if (gender == 0) charcreatorModelHash = game.getHashKey('mp_m_freemode_01');
    else if (gender == 1) charcreatorModelHash = game.getHashKey('mp_f_freemode_01');
    else return;
    game.requestModel(charcreatorModelHash);
    let interval = alt.setInterval(() => {
        if (game.hasModelLoaded(charcreatorModelHash)) {
            alt.clearInterval(interval);
            charcreatorPedHandle = game.createPed(4, charcreatorModelHash, 402.778, -996.9758, -100.01465, 0, false, true);
            game.setEntityHeading(charcreatorPedHandle, 180.0);
            game.setEntityInvincible(charcreatorPedHandle, true);
            game.disablePedPainAudio(charcreatorPedHandle, true);
            game.freezeEntityPosition(charcreatorPedHandle, true);
            game.taskSetBlockingOfNonTemporaryEvents(charcreatorPedHandle, true);


            setClothes(charcreatorPedHandle, 11, 15, 0);
            if (gender == 0) setClothes(charcreatorPedHandle, 8, 57, 0);
            else if (gender == 1) setClothes(charcreatorPedHandle, 8, 3, 0);
            setClothes(charcreatorPedHandle, 3, 15, 0);
        }
    }, 0);
}

function destroyLoginBrowser() {
    if (loginBrowser != null) loginBrowser.destroy();
    loginBrowser = null;
}

function destroyLoginCam() {
    game.renderScriptCams(false, false, 0, true, false);
    if (loginCamera != null) {
        game.setCamActive(loginCamera, false);
        game.destroyCam(loginCamera, true);
        loginCamera = null;
    }
}

function updateLoginCam(posX, posY, posZ, rotX, rotY, rotZ, fov) {
    if (loginCamera == null) return;
    game.setCamCoord(loginCamera, posX, posY, posZ);
    game.setCamRot(loginCamera, rotX, rotY, rotZ, 2);
    game.setCamFov(loginCamera, fov);
    game.setCamActive(loginCamera, true);
    game.renderScriptCams(true, false, 0, true, false);
}

function createLoginCam(x, y, z, rot) {
    if (loginCamera != null) game.destroyCam(loginCamera, true);
    loginCamera = game.createCamWithParams("DEFAULT_SCRIPTED_CAMERA", x, y, z, 0, 0, rot, 50, true, 2);
    game.setCamActive(loginCamera, true);
    game.renderScriptCams(true, false, 0, true, false);
}

alt.on("connectionComplete", () => {
    fadeScreenOut(100);
    alt.setTimeout(() => {
        createLoginBrowser();
    }, 3500);

    loadIPL("ferris_finale_Anim");
    loadIPL("shr_int");
    game.activateInteriorEntitySet(game.getInteriorAtCoordsWithType(-38.62, -1099.01, 27.31, 'v_carshowroom'), 'csr_beforeMission');
    game.activateInteriorEntitySet(game.getInteriorAtCoordsWithType(-38.62, -1099.01, 27.31, 'v_carshowroom'), 'shutter_closed');
});

alt.onServer("Client:Login:loginSuccess", (existSkin, typ) => {
    switchToCharSelect(existSkin, typ);
    if (typ == 1 && charcreatorPedHandle != null) {
        game.deletePed(charcreatorPedHandle);
        charcreatorPedHandle = null;
    }
});

alt.onServer("Client:Login:showError", (msg) => {
    if (loginBrowser == null) return;
    loginBrowser.emit("CEF:Login:showError", msg);
});