"use strict";

//var connection = new signalR.HubConnectionBuilder().withUrl("https://budgetingapp.harishparas.com/chatHub").build();
var connection = new signalR.HubConnectionBuilder().withUrl("http://localhost:51160/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;
const queryString = window.location.search;
const urlParams = new URLSearchParams(queryString);
const id = urlParams.get("senderid");

//connection.on("ReceiveMessage", function (user,sender_img, time, message) {
   
//    var li = document.createElement("li");
//    document.getElementById("messagesList").appendChild(li);
//    // We can assign user-supplied strings to an element's textContent because it
//    // is not interpreted as markup. If you're assigning in any other way, you 
//    // should be aware of possible script injection concerns.
//    li.textContent = `${user} says ${message} on ${time} --- image ${sender_img}`;
//});


connection.on("addChatMessage", function (message) {

    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${message}`;
});



connection.start().then(function () {
    console.log("connected 123");
    document.getElementById("sendButton").disabled = false;
    connection.invoke('getConnectionId', parseInt(id))
        .then(function (connectionId) {
            sessionStorage.setItem('conectionId', connectionId);
        });
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;

  //  connection.invoke("SendMessage", parseInt(user), parseInt(id), message,0,'Text').catch(function (err) {
//        return console.error(err.toString());
 //   });

    connection.invoke("JoinRoom", message, user).catch(function (err) {
        return console.error(err.toString());
    });

event.preventDefault();
});


