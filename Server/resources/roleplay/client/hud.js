import * as alt from 'alt';
import * as game from 'natives';
import { Player, Vector3, LocalStorage } from "alt";
import { Raycast, canInteract, setAccessory, setLastInteract, playAnimation, showCursor, setClothes, setTattoo, clearTattoos, setCorrectTattoos, clearProp } from './utilities.js';

const storage = LocalStorage.get();
export let hudBrowser = null;
export let browserReady = false;
let isInventoryCEFopened = false,
    isShopCEFopened = false,
    isATMCEFopened = false,
    isGarageCEFopened = false,
    isVehShopCEFopened = false,
    isTattooShopOpened = false,
    isClothesShopOpened = false,
    isClothesStorageOpened = false,
    isFactionManageCEFOpened = false,
    isLaptopActivated = false,
    isLaptopCEFOpened = false,
    isLaborCEFOpened = false;

alt.onServer("Client:HUD:createBrowser", (currentMoney) => {
    if (hudBrowser == null) {
        hudBrowser = new alt.WebView("http://resource/client/cef/hud/index.html");

        hudBrowser.on("Client:HUD:cefIsReady", () => {
            alt.setTimeout(function() {
                hudBrowser.emit("CEF:HUD:updateMoney", currentMoney);
                browserReady = true;
            }, 1000);
        });

        //Faction Manage
        hudBrowser.on("Client:FactionManage:closeCEF", () => {
            isFactionManageCEFOpened = false;
            showCursor(false);
            alt.toggleGameControls(true);
            hudBrowser.unfocus();
            alt.emitServer("Server:CEF:setCefStatus", false);
        });

        hudBrowser.on("Client:FactionManage:doAction", (action, factionId, accountId) => {
            switch (action) {
                case "rankup":
                    alt.emitServer("Server:FactionManage:rankUp", parseInt(factionId), parseInt(accountId));
                    break;
                case "rankdown":
                    alt.emitServer("Server:FactionManage:rankDown", parseInt(factionId), parseInt(accountId));
                    break;
                case "remove":
                    alt.emitServer("Server:FactionManage:removeMember", parseInt(factionId), parseInt(accountId));
                    break;
            }
        });

        hudBrowser.on("Client:FactionManage:inviteMember", (targetName) => {
            alt.emitServer("Server:FactionManage:inviteMember", targetName);
        });

        //Tattoo Shop
        hudBrowser.on("Client:TattooShop:closeShop", () => {
            isTattooShopOpened = false;
            showCursor(false);
            alt.toggleGameControls(true);
            hudBrowser.unfocus();
            game.freezeEntityPosition(alt.Player.local.scriptID, false);
            alt.emitServer("Server:CEF:setCefStatus", false);
            clearTattoos(alt.Player.local.scriptID);
            setCorrectTattoos();
        });

        hudBrowser.on("Client:TattooShop:buyTattoo", (shopId, tattooId) => {
            alt.emitServer("Server:TattooShop:buyTattoo", parseInt(shopId), parseInt(tattooId));
        });

        hudBrowser.on("Client:TattooShop:deleteTattoo", (id) => {
            alt.emitServer("Server:TattooShop:deleteTattoo", parseInt(id));
        });

        hudBrowser.on("Client:TattooShop:previewTattoo", (hash, collection) => {
            clearTattoos(alt.Player.local.scriptID);
            setTattoo(alt.Player.local.scriptID, collection, hash);
        });

        //Interaction
        hudBrowser.on("Client:Interaction:giveRequestedAction", (typ, action) => {
            InteractionDoAction(typ, action);
        });

        //Inventar
        hudBrowser.on("Client:Inventory:dropItem", (itemName, itemAmount) => {
            isInventoryCEFopened = false;
            showCursor(false);
            alt.toggleGameControls(true);
            alt.emitServer("Server:Inventory:dropItem", itemName, parseInt(itemAmount));
            playAnimation("anim@narcotics@trash", "drop_front", 500, 1, false);
            hudBrowser.unfocus();
        });

        hudBrowser.on("Client:Inventory:useItem", (itemName, itemAmount) => {
            isInventoryCEFopened = false;
            showCursor(false);
            alt.toggleGameControls(true);
            alt.emitServer("Server:Inventory:useItem", itemName, parseInt(itemAmount));
            hudBrowser.unfocus();
        });

        //Rotation HUD
        hudBrowser.on("Client:Utilities:setRotation", (rotZ) => {
            game.setEntityRotation(alt.Player.local.scriptID, game.getEntityPitch(alt.Player.local.scriptID), game.getEntityRoll(alt.Player.local.scriptID), rotZ, 2, true);
        });

        //Vehicle Shop
        hudBrowser.on("Client:VehicleShop:closeShop", () => {
            isVehShopCEFopened = false;
            showCursor(false);
            alt.toggleGameControls(true);
            hudBrowser.unfocus();
            alt.emitServer("Server:CEF:setCefStatus", false);
        });

        hudBrowser.on("Client:VehicleShop:buyVehicle", (shopId, stringifiedHash) => {
            alt.emitServer("Server:VehicleShop:buyVehicle", parseInt(shopId), stringifiedHash);
        });

        //Shop
        hudBrowser.on("Client:Shop:buyItem", (shopId, itemName, itemAmount) => {
            alt.emitServer("Server:Shop:buyItem", parseInt(shopId), itemName, parseInt(itemAmount));
        });

        hudBrowser.on("Client:Shop:depositShopItem", (shopId, itemName, itemAmount) => {
            alt.emitServer("Server:Shop:depositShopItem", parseInt(shopId), itemName, parseInt(itemAmount));
        });

        hudBrowser.on("Client:Shop:takeShopItem", (shopId, itemName, itemAmount) => {
            alt.emitServer("Server:Shop:takeShopItem", parseInt(shopId), itemName, parseInt(itemAmount));
        });

        hudBrowser.on("Client:Shop:setItemPrice", (shopId, itemName, itemPrice) => {
            alt.emitServer("Server:Shop:setItemPrice", parseInt(shopId), itemName, parseInt(itemPrice));
        });

        hudBrowser.on("Client:Shop:closeShop", () => {
            isShopCEFopened = false;
            showCursor(false);
            alt.toggleGameControls(true);
            hudBrowser.unfocus();
            alt.emitServer("Server:CEF:setCefStatus", false);
        });

        //ATM
        hudBrowser.on("Client:ATM:withdrawMoney", (money) => {
            alt.emitServer("Server:ATM:withdrawMoney", parseInt(money));
        });

        hudBrowser.on("Client:ATM:depositMoney", (money) => {
            alt.emitServer("Server:ATM:depositMoney", parseInt(money));
        });

        hudBrowser.on("Client:ATM:closeATM", () => {
            isATMCEFopened = false;
            showCursor(false);
            alt.toggleGameControls(true);
            hudBrowser.unfocus();
            alt.emitServer("Server:CEF:setCefStatus", false);
        });

        //Garage
        hudBrowser.on("Client:Garage:takeVehicle", (garageId, vehicleId) => {
            alt.emitServer("Server:Garage:takeVehicle", parseInt(garageId), parseInt(vehicleId));
        });

        hudBrowser.on("Client:Garage:storeVehicle", (garageId, vehicleId) => {
            alt.emitServer("Server:Garage:storeVehicle", parseInt(garageId), parseInt(vehicleId));
        });

        hudBrowser.on("Client:Garage:closeGarage", () => {
            isGarageCEFopened = false;
            showCursor(false);
            alt.toggleGameControls(true);
            hudBrowser.unfocus();
            alt.emitServer("Server:CEF:setCefStatus", false);
        });

        // Kleiderschrank
        hudBrowser.on("Client:ClothesStorage:SwitchClothes", (type, clothesName) => {
            alt.emitServer("Server:ClothesStorage:SwitchClothes", type, clothesName);
        });

        hudBrowser.on("Client:ClothesStorage:destroyCEF", () => {
            isClothesStorageOpened = false;
            showCursor(false);
            game.freezeEntityPosition(alt.Player.local.scriptID, false);
            alt.toggleGameControls(true);
            hudBrowser.unfocus();
            alt.emitServer("Server:CEF:setCefStatus", false);
        });

        //Kleiderladen
        hudBrowser.on("Client:ClothesShop:setAccessory", (comp, draw, tex) => {
            setAccessory(alt.Player.local.scriptID, comp, draw, tex);
        });

        hudBrowser.on("Client:ClothesShop:setClothes", (comp, draw, tex) => {
            setClothes(alt.Player.local.scriptID, comp, draw, tex);
        });

        hudBrowser.on("Client:ClothesShop:clearAccessory", (comp) => {
            clearProp(alt.Player.local.scriptID, comp);
        });

        hudBrowser.on("Client:ClothesShop:buyItem", (shopId, clothesName) => {
            alt.emitServer("Server:ClothesShop:buyItem", parseInt(shopId), clothesName);
        });

        hudBrowser.on("Client:Utilities:RequestCurrentSkin", () => {
            alt.emitServer("Server:Utilities:RequestCurrentSkin");
        });

        hudBrowser.on("Client:ClothesShop:destroyCEF", () => {
            isClothesShopOpened = false;
            showCursor(false);
            game.freezeEntityPosition(alt.Player.local.scriptID, false);
            alt.toggleGameControls(true);
            hudBrowser.unfocus();
            alt.emitServer("Server:CEF:setCefStatus", false);
        });

        //Labor
        hudBrowser.on("Client:Labor:switchItemToInventory", (name, amount) => {
            alt.emitServer("Server:Labor:switchItemToInventory", name, parseInt(amount));
        });

        hudBrowser.on("Client:Labor:switchItemToLabor", (name, amount) => {
            alt.emitServer("Server:Labor:switchItemToLabor", name, parseInt(amount));
        });

        hudBrowser.on("Client:Labor:destroy", () => {
            isLaborCEFOpened = false;
            showCursor(false);
            game.freezeEntityPosition(alt.Player.local.scriptID, false);
            alt.toggleGameControls(true);
            hudBrowser.unfocus();
            alt.emitServer("Server:CEF:setCefStatus", false);
        });
    }
});

//Labor
alt.onServer("Client:Labor:openLabor", (invItems, laborItems) => {
    if (hudBrowser == null || !browserReady || isLaborCEFOpened) return;
    isLaborCEFOpened = true;
    hudBrowser.emit("CEF:Labor:openLabor", invItems, laborItems);
    showCursor(true);
    alt.toggleGameControls(false);
    hudBrowser.focus();
});

//Laptop
alt.onServer("Client:Laptop:activateLaptop", (isActivated) => {
    isLaptopActivated = isActivated;
    if (isActivated) alt.emit("Client:HUD:sendNotification", 1, 1500, "Du hast deinen Laptop angeschaltet.");
    else alt.emit("Client:HUD:sendNotification", 1, 1500, "Du hast deinen Laptop ausgeschaltet.");
});

alt.onServer("Client:Laptop:updateApps", (LSPDApp) => {
    if (hudBrowser == null || !browserReady) return;
    hudBrowser.emit("CEF:Laptop:updateApps", LSPDApp);
});

// Geld-HUD
alt.onServer("Client:HUD:updateMoney", (currentMoney) => {
    if (hudBrowser != null) {
        hudBrowser.emit("CEF:HUD:updateMoney", currentMoney);
    }
});

//Notifications
alt.onServer("Client:HUD:sendNotification", (type, duration, msg, delay) => {
    alt.setTimeout(() => {
        if (hudBrowser != null) {
            hudBrowser.emit("CEF:HUD:sendNotification", type, duration, msg);
        }
    }, delay);
});

alt.on("Client:HUD:sendNotification", (type, duration, msg) => {
    if (hudBrowser != null) {
        hudBrowser.emit("CEF:HUD:sendNotification", type, duration, msg);
    }
});

//Faction Manage
alt.onServer("Client:FactionManage:openFactionManager", (factionId, memberJson) => {
    if (hudBrowser == null || isFactionManageCEFOpened) return;
    isFactionManageCEFOpened = true;
    hudBrowser.emit("CEF:FactionManage:openFactionManager", factionId, memberJson);
    showCursor(true);
    alt.toggleGameControls(false);
    hudBrowser.focus();
});

//Garage
alt.onServer("Client:Garage:openGarage", (garageId, garageVehs, outsideVehs) => {
    if (hudBrowser == null || isGarageCEFopened) return;
    isGarageCEFopened = true;
    hudBrowser.emit("CEF:Garage:openGarage", garageId, garageVehs, outsideVehs);
    showCursor(true);
    alt.toggleGameControls(false);
    hudBrowser.focus();
});

//Inventar
alt.onServer("Client:Inventory:openInventory", (weight, maxWeight, invItems) => {
    if (hudBrowser == null || isInventoryCEFopened) return;
    isInventoryCEFopened = true;
    hudBrowser.emit("CEF:Inventory:openInventory", weight, maxWeight, invItems);
    showCursor(true);
    alt.toggleGameControls(false);
    hudBrowser.focus();
});

//Tattoo Shop
alt.onServer("Client:TattooShop:openShop", (gender, shopId, ownTattoosJSON) => {
    if (hudBrowser == null || isTattooShopOpened) return;
    isTattooShopOpened = true;
    hudBrowser.emit("CEF:TattooShop:openShop", shopId, ownTattoosJSON);
    showCursor(true);
    game.freezeEntityPosition(alt.Player.local.scriptID, true);
    alt.toggleGameControls(false);
    hudBrowser.focus();
    if (gender == 0) {
        setClothes(alt.Player.local.scriptID, 11, 15, 0);
        setClothes(alt.Player.local.scriptID, 8, 15, 0);
        setClothes(alt.Player.local.scriptID, 3, 15, 0);
        setClothes(alt.Player.local.scriptID, 4, 21, 0);
        setClothes(alt.Player.local.scriptID, 6, 34, 0);
    } else {
        //ToDo
    }
});

alt.onServer("Client:TattooShop:sendItemsToClient", (items) => {
    if (hudBrowser == null) return;
    hudBrowser.emit("CEF:TattooShop:sendItemsToClient", items);
});

//Shop
alt.onServer("Client:Shop:openShop", (shopId, items) => {
    if (hudBrowser == null || isShopCEFopened) return;
    isShopCEFopened = true;
    hudBrowser.emit("CEF:Shop:openShop", shopId, items);
    showCursor(true);
    alt.toggleGameControls(false);
    hudBrowser.focus();
});

alt.onServer("Client:Shop:openShopManager", (shopId, inventoryItems, shopItems) => {
    if (hudBrowser == null || isShopCEFopened) return;
    isShopCEFopened = true;
    hudBrowser.emit("CEF:Shop:openShopManager", shopId, inventoryItems, shopItems);
    showCursor(true);
    alt.toggleGameControls(false);
    hudBrowser.focus();
});

//Vehicle Shop
alt.onServer("Client:VehicleShop:openShop", (shopId, items) => {
    if (hudBrowser == null || isVehShopCEFopened) return;
    isVehShopCEFopened = true;
    hudBrowser.emit("CEF:VehicleShop:openShop", shopId, items);
    showCursor(true);
    alt.toggleGameControls(false);
    hudBrowser.focus();
});

//ATM
alt.onServer("Client:ATM:openATM", (money) => {
    if (hudBrowser == null || isATMCEFopened) return;
    isATMCEFopened = true;
    hudBrowser.emit("CEF:ATM:openATM", money);
    showCursor(true);
    alt.toggleGameControls(false);
    hudBrowser.focus();
});

//Kleiderschrank
alt.onServer("Client:ClothesStorage:openStorage", (gender) => {
    if (hudBrowser == null || isClothesStorageOpened) return;
    isClothesStorageOpened = true;
    hudBrowser.emit("CEF:ClothesStorage:openClothesStorageHUD", gender);
    game.freezeEntityPosition(alt.Player.local.scriptID, true);
    showCursor(true);
    alt.toggleGameControls(false);
    hudBrowser.focus();
});

alt.onServer("Client:ClothesStorage:sendItemsToClient", (json) => {
    if (hudBrowser == null) return;
    hudBrowser.emit("CEF:ClothesStorage:sendItemsToClient", json);
});

//Kleiderladen
alt.onServer("Client:ClothesShop:openShop", (shopId) => {
    if (hudBrowser == null || isClothesShopOpened) return;
    isClothesShopOpened = true;
    hudBrowser.emit("CEF:ClothesShop:openShop", shopId);
    game.freezeEntityPosition(alt.Player.local.scriptID, true);
    showCursor(true);
    alt.toggleGameControls(false);
    hudBrowser.focus();
});

alt.onServer("Client:ClothesShop:sendItemsToClient", (json) => {
    if (hudBrowser == null) return;
    hudBrowser.emit("CEF:ClothesShop:sendItemsToClient", json);
});

//Raycast Interaktionsmenü
let selectedRaycastId = null,
    playerRC = null,
    vehicle = null,
    interactPlayer = null,
    interactVehicle = null,
    InteractMenuUsing = false;

function InteractionDoAction(type, action) {
    if (selectedRaycastId != null && selectedRaycastId != 0 && type != "none") {
        if (type == "vehicleOut" || type == "vehicleIn") type = "vehicle";
        if (type == "vehicle") {
            vehicle = alt.Vehicle.all.find(x => x.scriptID == selectedRaycastId);
            if (!vehicle) return;
            switch (action) {
                case "lockVehicle":
                    alt.emitServer("Server:Interaction:lockVehicle", vehicle);
                    break;
                case "engineVehicle":
                    alt.emitServer("Server:Interaction:engineVehicle", vehicle);
                    break;
            }
            vehicle = null;
        } else if (type == "player") {
            playerRC = alt.Player.all.find(x => x.scriptID == selectedRaycastId);
            if (!playerRC) return;
            if (action == "showSupportId") alt.emitServer("Server:Interaction:showSupportId", playerRC);
            playerRC = null;
        }
        selectedRaycastId = null;
    }
}

alt.onServer("Client:Interaction:SetInfo", (typ, array) => {
    if (hudBrowser == null) return;
    hudBrowser.emit("CEF:Interaction:toggleInteractionMenu", typ, true, array);
});

// Key-Events
alt.on('keyup', (key) => {
    if (key == 'X'.charCodeAt(0)) {
        if (hudBrowser == null || InteractMenuUsing == false) return;
        hudBrowser.emit("CEF:Interaction:requestAction");
        hudBrowser.emit("CEF:Interaction:toggleInteractionMenu", "", false);
        InteractMenuUsing = false;
        hudBrowser.unfocus();
        showCursor(false);
        alt.toggleGameControls(true);
        return;
    }
    if (!canInteract) return;
    if (key == 'I'.charCodeAt(0)) {
        if (hudBrowser == null) return;
        setLastInteract();
        if (isInventoryCEFopened) {
            hudBrowser.emit("CEF:Inventory:closeInventory");
            showCursor(false);
            alt.toggleGameControls(true);
            alt.emitServer("Server:CEF:setCefStatus", false);
            isInventoryCEFopened = false;
            hudBrowser.unfocus();
        } else {
            if (alt.Player.local.getSyncedMeta("IsCefOpen") === true) return;
            alt.emitServer("Server:Inventory:openInventory");
        }
    } else if (key == 'E'.charCodeAt(0)) {
        setLastInteract();
        alt.emitServer("Server:Keyhandler:pressE");
    } else if (key == 114) {
        if (!isLaptopActivated) return;
        setLastInteract();
        if (isLaptopCEFOpened) {
            hudBrowser.emit("CEF:Laptop:hideLaptop");
            showCursor(false);
            alt.toggleGameControls(true);
            alt.emitServer("Server:CEF:setCefStatus", false);
            hudBrowser.unfocus();
        } else {
            hudBrowser.emit("CEF:Laptop:showLaptop");
            showCursor(true);
            alt.toggleGameControls(false);
            alt.emitServer("Server:CEF:setCefStatus", true);
            hudBrowser.focus();
        }
        isLaptopCEFOpened = !isLaptopCEFOpened;
    } else if (key == 76) {
        setLastInteract();
        alt.emitServer("Server:Keyhandler:pressL");
    }
});

alt.on("keydown", (key) => {
    if (key == 'X'.charCodeAt(0)) {
        if (alt.Player.local.getSyncedMeta("IsCefOpen")) return;
        let result = Raycast.line(1.5, 2.5);
        if (result == undefined && !alt.Player.local.vehicle) return;
        if (!alt.Player.local.vehicle) {
            if (result.isHit && result.entityType != 0) {
                if (result.entityType == 1 && hudBrowser != null) {
                    selectedRaycastId = result.hitEntity;
                    interactPlayer = alt.Player.all.find(x => x.scriptID == selectedRaycastId);
                    if (!interactPlayer) return;
                    InteractMenuUsing = true;
                    hudBrowser.focus();
                    showCursor(true);
                    alt.toggleGameControls(false);
                    alt.emitServer("Server:Interaction:GetPlayerInfo", interactPlayer);
                    interactPlayer = null;
                    return;
                } else if (result.entityType == 2 && hudBrowser != null) {
                    selectedRaycastId = result.hitEntity;
                    interactVehicle = alt.Vehicle.all.find(x => x.scriptID == selectedRaycastId);
                    if (!interactVehicle) return;
                    InteractMenuUsing = true;
                    hudBrowser.focus();
                    showCursor(true);
                    alt.toggleGameControls(false);
                    alt.emitServer("Server:Interaction:GetVehicleInfo", "vehicleOut", interactVehicle);
                    interactVehicle = null;
                    return;
                }
            }
        }

        if (alt.Player.local.vehicle && hudBrowser != null) {
            selectedRaycastId = alt.Player.local.vehicle.scriptID;
            interactVehicle = alt.Vehicle.all.find(x => x.scriptID == selectedRaycastId);
            InteractMenuUsing = true;
            hudBrowser.focus();
            showCursor(true);
            alt.toggleGameControls(false);
            if (!interactVehicle) return;
            alt.emitServer("Server:Interaction:GetVehicleInfo", "vehicleIn", interactVehicle);
            interactVehicle = null;
            return;
        }
    }
});