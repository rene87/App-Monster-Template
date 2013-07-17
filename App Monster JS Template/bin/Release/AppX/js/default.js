// For an introduction to the Blank template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkId=232509
(function () {
    "use strict";

    var myAppMonster;

    WinJS.Binding.optimizeBindingReferences = true;

    var app = WinJS.Application;
    var activation = Windows.ApplicationModel.Activation;

    app.onactivated = function (args) {
        if (args.detail.kind === activation.ActivationKind.launch) {
            if (args.detail.previousExecutionState !== activation.ApplicationExecutionState.terminated) {
                // TODO: This application has been newly launched. Initialize
                // your application here.
            } else {
                // TODO: This application has been reactivated from suspension.
                // Restore application state here.
            }
            args.setPromise(WinJS.UI.processAll().then(function completed() {
                
                createAppMonster();

                registerForSharing(); 
            }));
        }
    };

    app.oncheckpoint = function (args) {
        // TODO: This application is about to be suspended. Save any state
        // that needs to persist across suspensions here. You might use the
        // WinJS.Application.sessionState object, which is automatically
        // saved and restored across suspension. If you need to complete an
        // asynchronous operation before your application is suspended, call
        // args.setPromise().
    };

    app.onloaded = function () {
        WinJS.Resources.processAll();
    }

    WinJS.Application.onsettings = function (e) {
        e.detail.applicationcommands = {
            "aboutDiv": { title: WinJS.Resources.getString('aboutEntrypoint').value, href: "default.html" }
        };
        WinJS.UI.SettingsFlyout.populateSettings(e);
    };


    app.start();

    function createAppMonster() {

        // First we define the image file for your App Monster
        // This sample image is based on: OCAL, http://www.clker.com/clipart-26433.html, License: Public Domain
        var imageUri = new Windows.Foundation.Uri("ms-appx:///images/AppMonsterTemplateAvatar.png");

        // Set the values for you own App Monster here. Check wether your App Monster complies with the rules

        // The name is restricted to 30 letter, the description to 300 letters. You should think about localizing the description.
        var appMonsterName = "Template Monster ";
        var appMonsterDefaultDescription = "This is the english description for the Template Monster. This is the english description for the Template Monster.";

        // Is your App Monster a paid App? Rare (paid) App Monster have 20 more skillpoints you can use. The maximum allowed prize is an equivalent of € 4.99
        var paid = false;

        // Set the values for your App Monster here. You can use up to 40 points for a free and up to 60 for a paid App Monster
        var defense = 8;
        // This is the damage your monster can take in a fight
        var hitpoints = 20;
        // This is the amount of food your monster can harvest when farming
        var harvest = 3;

        // Define up to two attacks for your AppMonster;
        var firstAttack = new AppMonsters.AppMonsterAttack("Firepunch", 3);
        firstAttack.addLocalizedName("en-US", "Firepunch");
        firstAttack.addLocalizedName("de-DE", "Feuerhieb");

        var secondAttack = new AppMonsters.AppMonsterAttack("Faststrike", 3);
        secondAttack.addLocalizedName("en-US", "Faststrike");
        secondAttack.addLocalizedName("de-DE", "Schnellschlag");

        // Now we need to load the image and create you App Monster
        Windows.Storage.StorageFile.getFileFromApplicationUriAsync(imageUri).then(function (imageFile) {

            // This where things get exciting. Here we try to bring you new App Monster to life. While doing this we check whether you App Monster complies with the rules. Feels like being Frankenstein, doesn't it?
            myAppMonster = new AppMonsters.AppMonster(appMonsterName, appMonsterDefaultDescription, firstAttack, secondAttack, defense, hitpoints, harvest, paid, imageFile, imageUri);

            // You can add localized descriptions for your App Monster - this is optional. You still have to comply with the 300 letter limit. The translations in the example were created using http://translate.bing.com.
            myAppMonster.addLocalizedDescription("en-US", "This is the english description for the Template Monster. This is the english description for the Template Monster.");
            myAppMonster.addLocalizedDescription("de-DE", "Dies ist die deutsche Beschreibung für das Template Monster. Dies ist die deutsche Beschreibung für das Template Monster.");

            // Set you new AppMonster as the Datacontext for this page to display the App Monster Information
            setDataContext();
        });
    }

    // This code makes your monster visible on the main page of your app.
    function setDataContext() {
        var template = document.getElementById("monsterTemplate");

        WinJS.Binding.processAll(template, {
            name: myAppMonster.name,
            primaryAttack: {
                localizedName: myAppMonster.primaryAttack.localizedName,
                damage: myAppMonster.primaryAttack.damage
            },
            secondaryAttack: {
                localizedName: myAppMonster.secondaryAttack.localizedName,
                damage: myAppMonster.secondaryAttack.damage
            },
            defense: myAppMonster.defense,
            harvest: myAppMonster.harvest,
            foodCost: myAppMonster.foodCost,
            localizedDescription: myAppMonster.localizedDescription,
            hitPoints: myAppMonster.hitPoints,
            imageUri: myAppMonster.imageUri.absoluteUri
        });
    }


    function registerForSharing() {
        var dataTransferManager = Windows.ApplicationModel.DataTransfer.DataTransferManager.getForCurrentView();
        dataTransferManager.addEventListener("datarequested", shareAppMonsterHandler);
    }

    // If you want to open the sharing menu programmatically
    //function showShareUI() {
    //    Windows.ApplicationModel.DataTransfer.DataTransferManager.showShareUI();
    //}

    function shareAppMonsterHandler(e) {

        var request = e.request;

        request.data = myAppMonster.getShareData();
        
        //TODO Add remaining content
        // If you want to modify the default title, description or text for sharing from this page you can do it here
        //request.data.properties.title = "This is " + myAppMonster.name + " - the lastest addition to my App Monster team";
        //request.data.properties.description = "Share " + myAppMonster.name + " with the App Monsters Hub App";
        //request.data.setText("I just downloaded a new App Monster from the Windows Store: " + myAppMonster.name + "! Check it out yourself at http://apps.microsoft.com/windows/app/" + myAppMonster.appId);        
        
    }
})();



