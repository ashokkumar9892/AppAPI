"use strict";


var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:44301/chatHub").build();

//var connection = new signalR.HubConnectionBuilder().withUrl("https://petjet.harishparas.com/chatHub").build();



 ///***Send Messages Starts ****///
////Disable send button until connection is established

const queryString = window.location.search;
const urlParams = new URLSearchParams(queryString);
const id = urlParams.get("senderid");
var msgto = urlParams.get("msgto");
var msg = urlParams.get("msg");
connection.start().then(function () {
    //console.log(parseInt(id));   
    //connection.invoke('GetConnectionId', parseInt(id))
    //    .then(function (connectionId) {
    //        sessionStorage.setItem('conectionId', connectionId);
    //        alert(connection.ConnectionId);
    //    });

    $(document).ready(function () {
        $("#sendButton").click(function (e) {
            var req = {
                SenderSK: "PATIENT_1645611610498",
                ReceiverSK: "PATIENT_1650001697567",
                Message: "New Message 1!",
                MessageType : "Text",
                AuthToken: "eyJraWQiOiI3UGloR0p3MFlcL1dxdDRuSnMxQ0x0R2syOUZGYTZiSEhZVXpwNUY3N3Zlaz0iLCJhbGciOiJSUzI1NiJ9.eyJzdWIiOiI5NzVjNTEwZC1mZWI1LTQ3OTEtOTkyZC00OGZlOTZjOWRkMzIiLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsImlzcyI6Imh0dHBzOlwvXC9jb2duaXRvLWlkcC51cy13ZXN0LTIuYW1hem9uYXdzLmNvbVwvdXMtd2VzdC0yXzVBMUI4dExjOSIsImNvZ25pdG86dXNlcm5hbWUiOiJ0aGlydXBhQHBhcmFzdGVjaG5vbG9naWVzLmNvbSIsIm9yaWdpbl9qdGkiOiJkZWRkZjM3OS02MTg4LTQxMWItYTllNS1kYTRlZWIxZTViNjMiLCJhdWQiOiIyZ3EwZDdpN2E2cnVsMHEwYWk3bDdkc2lkcyIsImV2ZW50X2lkIjoiMjExMDcwZDItZDRkMy00MDRhLWI4MDUtNjI1ZjM1ZWQ4ZmVkIiwidG9rZW5fdXNlIjoiaWQiLCJhdXRoX3RpbWUiOjE2NTA5NzQ2NzEsImV4cCI6MTY1MDk3ODI3MSwiaWF0IjoxNjUwOTc0NjcxLCJqdGkiOiIzZTQ4MDJiNy0xYjUzLTRjYmMtYTQwMy00YzZhODdmZjEwZGMiLCJlbWFpbCI6InRoaXJ1cGFAcGFyYXN0ZWNobm9sb2dpZXMuY29tIn0.biRWtCnnIKEO_zCGAeIFcRO7B5N3DW_2mds7o7d9rEg5dsUlAsXd0kgpIHOqzWwrD2rL4GBxL2TaGW-AqU5Kl4KvNyOeQCegtuNa3cL9ZWsyqP314X0UvjAeeX_qEbkrx2uCIxpSPBB95V0wfhCniWDXv3HPFMDo2ZCTCRBOv2k2ywnkxZ6V5dnnE5nrlsVt63W5Etj8lzA1FTU3RkNyI5WakbNZnaNhpHEueF-wbNZA5Zv7e3JzAruegkXsreS7tRCjKaV8naY-Q1eYFTFLrOVZ08UwQ5EMIoh0oWHEvj-iAUmBiQQv_FvNdO_DyUOl_OL2NLx9CNwxov0637bAeQ"
            }


            connection.invoke("SendMessages", req).catch(function (err) {
            //    connection.invoke("UserChatList", 3,'Driver').catch(function (err) {
            return console.error(err.toString());
        });       
    });
});

connection.on("ReceiveMessage", function (data) {
        console.log(data);        
});
});

/////***Send Messages Ends****///
  


//***Show User Chatist Starts***//
//connection.start().then(function () {  

//    $(document).ready(function () {
//        connection.invoke("UserChatList", parseInt(id), 'Agent').catch(function (err) {
//            return console.error(err.toString());
//        });
//    });

//    connection.on("ShowUserChatList", function (data) {
//        console.log(data);      
//    });
//});

//***Show User Chatist Ends***//

  



