"use strict";

//var connection = new signalR.HubConnectionBuilder().withUrl("https://budgetingapp.harishparas.com/chatHub").build();
var connection = new signalR.HubConnectionBuilder().withUrl("http://localhost:51160/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = false;
const queryString = window.location.search;
const urlParams = new URLSearchParams(queryString);
const id = urlParams.get("senderid");

connection.on("ReceiveMessage", function (user, message) {
   
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message} `;
});

connection.start().then(function () {
    console.log("connected 123");
    document.getElementById("sendButton").disabled = false;
    connection.invoke('getConnectionId', id)
        .then(function (connectionId) {
            sessionStorage.setItem('conectionId', connectionId);
        });
});


$(document).ready(function () {
    $("#sendButton").click(function (e) {
        connection.invoke('JoinGroup', "PrivateGroup");
        e.preventDefault();
    });
});

$(document).ready(function () {
    $("#btnSend").click(function (e) {
        let message = $('#messagebox').val();
        let sender = $('#senderUId').text();
        $('#messagebox').val('');

        connection.invoke('SendMessageToGroup', "1", id, message);

        e.preventDefault();
    });
});
