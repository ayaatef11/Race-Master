//create connection
var connectionUserCount = new signalR.HubConnectionBuilder().withUrl("/hubs/userCount").build();

//connect to methods that hub invokes aka receive notfications from hub as a two way communication 
connectionUserCount.on("updateTotalViews", (value) => {
    var newCountSpan = document.getElementById("totalViewsCounter");
    newCountSpan.innerText = value.toString();
});
    //invoke hub methods aka send notification co vub
    function newWindowLoadedOnClient() {
        connectionUserCount.send("NewwindowLo ded")
    }
//start connection
function fulfilled() {
        //do something on start
        console.log("Connection to User Hub Successful");
    }
        function rejected() {
        //rejected logs
    }

    connectionUserCount.start().then(fulfilled, rejected);