import * as alt from 'alt';
import * as game from 'natives';

let cursor = false,
    lastInteract = 0,
    markerarray = [],
    playerTattoos = undefined;

export function setLastInteract() {
    lastInteract = Date.now();
}

export function canInteract() { return lastInteract + 1000 < Date.now() }

export function showCursor(state) {
    if (state && cursor || !state && !cursor) return;
    cursor = state;
    alt.showCursor(state);
}

export function fadeScreenOut(duration) {
    game.doScreenFadeOut(duration);
}

export function fadeScreenIn(duration) {
    game.doScreenFadeIn(duration);
}

export function loadIPL(iplName) {
    alt.requestIpl(iplName);
}

export function clearTattoos(entity) {
    game.clearPedDecorations(entity);
}

export function setTattoo(entity, collection, hash) {
    game.addPedDecorationFromHashes(entity, game.getHashKey(collection), game.getHashKey(hash));
}

export function setClothes(entity, compId, draw, tex) {
    game.setPedComponentVariation(entity, compId, draw, tex, 0);
}

export function setAccessory(entity, compId, draw, tex) {
    game.setPedPropIndex(entity, compId, draw, tex, false);
}

export function clearProp(entity, compId) {
    game.clearPedProp(entity, compId);
}

export function createBlip(X, Y, Z, sprite, scale, color, name, shortRange) {
    const blip = new alt.PointBlip(X, Y, Z);
    blip.sprite = sprite;
    blip.scale = scale;
    blip.color = color;
    blip.shortRange = shortRange;
    blip.name = name;
}

export function GetDirectionFromRotation(rotation) {
    var z = rotation.z * (Math.PI / 180.0);
    var x = rotation.x * (Math.PI / 180.0);
    var num = Math.abs(Math.cos(x));

    return new alt.Vector3(
        (-Math.sin(z) * num),
        (Math.cos(z) * num),
        Math.sin(x)
    );
}

export function spawnPed(model, x, y, z, rot) {
    let modelHash = game.getHashKey(model);
    new Promise((resolve, reject) => {
        if (game.hasModelLoaded(modelHash)) {
            resolve();
        }
        game.requestModel(modelHash);
        const timer = alt.setInterval(() => {
            if (game.hasModelLoaded(modelHash)) {
                alt.clearInterval(timer);
                resolve();
            }
        }, 10);
    }).then(() => {
        let pedHandle = game.createPed(4, modelHash, x, y, z, rot, false, true);
        game.setEntityInvincible(pedHandle, true);
        game.disablePedPainAudio(pedHandle, true);
        game.freezeEntityPosition(pedHandle, true);
        game.taskSetBlockingOfNonTemporaryEvents(pedHandle, true);
    });
}

export function playAnimation(animDict, animName, duration, flag, lockpos) {
    new Promise((resolve, reject) => {
        if (game.hasAnimDictLoaded(animDict)) {
            resolve();
        }
        game.requestAnimDict(animDict);
        const timer = alt.setInterval(() => {
            if (game.hasAnimDictLoaded(animDict)) {
                alt.clearInterval(timer);
                resolve();
            }
        }, 10);
    }).then(() => {
        game.taskPlayAnim(alt.Player.local.scriptID, animDict, animName, 8.0, 1, duration, flag, 1, lockpos, lockpos, lockpos);
    });
}

alt.onServer("Client:Vehicles:ToggleDoorState", (veh, doorid, state) => {
    toggleDoor(veh, parseInt(doorid), state);
});

export function toggleDoor(vehicle, doorid, state) {
    if (state) game.setVehicleDoorOpen(vehicle.scriptID, doorid, false, false);
    else game.setVehicleDoorShut(vehicle.scriptID, doorid, false);
}

alt.onServer("Client:Utilities:setClothes", (compId, draw, tex) => {
    setClothes(alt.Player.local.scriptID, compId, draw, tex);
});

alt.onServer("Client:Utilities:setAccessory", (compId, draw, tex) => {
    setAccessory(alt.Player.local.scriptID, compId, draw, tex);
});

alt.onServer("Client:Utilities:clearAccessory", (compId) => {
    clearProp(alt.Player.local.scriptID, compId);
});

alt.onServer("Client:Utilities:setTattoos", (tattooJSON) => {
    playerTattoos = JSON.parse(tattooJSON);
    setCorrectTattoos();
});

export function setCorrectTattoos() {
    clearTattoos(alt.Player.local.scriptID);
    for (var i in playerTattoos) setTattoo(alt.Player.local.scriptID, playerTattoos[i].collection, playerTattoos[i].hash);
}

alt.onServer("Client:Utilities:LoadBlips", (blips) => {
    blips = JSON.parse(blips);
    for (var i in blips) {
        createBlip(blips[i].posX, blips[i].posY, blips[i].posZ, blips[i].sprite, blips[i].scale, blips[i].color, blips[i].name, blips[i].shortRange);
    }
});

alt.onServer("Client:Utilities:LoadPeds", (peds) => {
    peds = JSON.parse(peds);
    for (var i in peds) {
        spawnPed(peds[i].pedModel, peds[i].posX, peds[i].posY, peds[i].posZ, peds[i].pedRotation);
    }
});

alt.onServer("Client:Utilities:LoadMarkers", (markers) => {
    markers = JSON.parse(markers);

    for (var i in markers) {
        markerarray.push({
            type: markers[i].type,
            x: markers[i].posX,
            y: markers[i].posY,
            z: markers[i].posZ,
            scale: markers[i].scale,
            red: markers[i].red,
            green: markers[i].green,
            blue: markers[i].blue,
            alpha: markers[i].alpha,
            bobUpAndDown: markers[i].bobUpAndDown
        });
    }
});

alt.everyTick(() => {
    game.invalidateIdleCam();
    if (markerarray.length <= 0) return;
    for (var i = 0; i < markerarray.length; i++) {
        game.drawRect(0, 0, 0, 0, 0, 0, 0, 0);
        game.drawMarker(markerarray[i].type, markerarray[i].x, markerarray[i].y, markerarray[i].z, 0, 0, 0, 0, 0, 0, markerarray[i].scale, markerarray[i].scale, markerarray[i].scale, markerarray[i].red, markerarray[i].green, markerarray[i].blue, markerarray[i].alpha, markerarray[i].bobUpAndDown, false, 2, false, undefined, undefined, false);
    }
});

alt.on('keyup', (key) => {
    if (key == 117) showCursor(!cursor);
});

let count = 0;

alt.on("consoleCommand", (name, args) => {
    if (name == "rot") alt.log(`Rotation: ${JSON.stringify(game.getEntityRotation(alt.Player.local.scriptID, 2))}`);
});

export class Raycast {
    static player = alt.Player.local;

    static line(radius, distance) {
        let position = game.getPedBoneCoords(alt.Player.local.scriptID, 31086, 0.5, 0, 0);
        let direction = GetDirectionFromRotation(game.getGameplayCamRot(2));
        let farAway = new alt.Vector3((direction.x * distance) + (position.x), (direction.y * distance) + (position.y), (direction.z * distance) + (position.z));
        let ray = game.startShapeTestCapsule(position.x, position.y, position.z, farAway.x, farAway.y, farAway.z, radius, -1, alt.Player.local.scriptID, 7);
        return this.result(ray);
    }

    static result(ray) {
        let result = game.getShapeTestResult(ray, undefined, undefined, undefined, undefined);
        let hitEntity = result[4];
        if (!game.isEntityAPed(hitEntity) && !game.isEntityAnObject(hitEntity) && !game.isEntityAVehicle(hitEntity)) return undefined;
        return {
            isHit: result[1],
            pos: new alt.Vector3(result[2].x, result[2].y, result[2].z),
            hitEntity,
            entityType: game.getEntityType(hitEntity),
            entityHash: game.getEntityModel(hitEntity)
        }
    }
}