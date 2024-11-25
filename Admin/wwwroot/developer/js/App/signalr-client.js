$(function () { // Handler for .ready() called. 
    //create connection
    var connection = new signalR.HubConnectionBuilder()
        .withUrl(getAPIUrl().replace("/api/", "/hubs/info"))
        .configureLogging(signalR.LogLevel.Debug)
        .build();

    //connect to methods that hub invokes, receive notifications from hub
    connection.on("updateTotalViews", (value) => {
        var newCountSpan = document.getElementById("totalViewsCounter");
        newCountSpan.innerText = value.toString();
    });

    connection.on("updateTotalUsers", (value) => {
        var newCountSpan = document.getElementById("totalUsersCounter");
        newCountSpan.innerText = value.toString();
    });

    //invoke hub methods, send notification to hub
    newWindowLoadedOnClient = () => { connection.send("NewWindowLoaded"); }

    //start connection
    fulfilled = () => {
        //do something on start
        console.log("Connection to user hub successful");
        newWindowLoadedOnClient();
    }

    rejected = () => {
        //rejected logs
        console.log("Connection to user hub rejected");
    }

    connection.start().then(fulfilled, rejected);
});