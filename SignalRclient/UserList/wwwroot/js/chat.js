"use strict";


//var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:44357/chatHub").build();

var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:44301/chatHub").build();



 ///***Send Messages Starts ****///
////Disable send button until connection is established

const queryString = window.location.search;
const urlParams = new URLSearchParams(queryString);
const id = urlParams.get("senderid");
var msgto = urlParams.get("msgto");
var msg = urlParams.get("msg");
//connection.start().then(function () {
//    console.log(parseInt(id));   
//    connection.invoke('GetConnectionId', parseInt(id))
//        .then(function (connectionId) {
//            sessionStorage.setItem('conectionId', connectionId);
//            alert(connection.ConnectionId);
//        });

//    $(document).ready(function () {
//        $("#sendButton").click(function (e) {
  
//            connection.invoke("SendMessages", 'Text', parseInt(msgto), parseInt(id) , msg).catch(function (err) {
//            //    connection.invoke("UserChatList", 3,'Driver').catch(function (err) {
//            return console.error(err.toString());
//        });       
//    });
//});

//connection.on("ReceiveMessage", function (data) {
//        console.log(data);        
//});
//});

/////***Send Messages Ends****///
  


//***Show User Chatist Starts***//
connection.start().then(function () {  

    $(document).ready(function () {
        var req = 
        {
            AuthToken: "eyJraWQiOiI3UGloR0p3MFlcL1dxdDRuSnMxQ0x0R2syOUZGYTZiSEhZVXpwNUY3N3Zlaz0iLCJhbGciOiJSUzI1NiJ9.eyJzdWIiOiJhZjIxNDFkMi0wMGU0LTQ4ZDItODZlZi1hZGEwOGE3YmNlOWMiLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsImlzcyI6Imh0dHBzOlwvXC9jb2duaXRvLWlkcC51cy13ZXN0LTIuYW1hem9uYXdzLmNvbVwvdXMtd2VzdC0yXzVBMUI4dExjOSIsImNvZ25pdG86dXNlcm5hbWUiOiJhcHBzZGV2ZWxvcGVyMjJAZ21haWwuY29tIiwib3JpZ2luX2p0aSI6IjZhOGJkZDJkLTNiYWMtNDQzMi04YjIzLTI4M2FhNjA2MjFiOSIsImF1ZCI6IjJncTBkN2k3YTZydWwwcTBhaTdsN2RzaWRzIiwiZXZlbnRfaWQiOiIwOWJkZmRlYi0wYmY1LTRhYjAtYjAzZC1lNWQ5NjlhYTA2OWMiLCJ0b2tlbl91c2UiOiJpZCIsImF1dGhfdGltZSI6MTY1MDk1MjAwNCwiZXhwIjoxNjUwOTU1NjA0LCJpYXQiOjE2NTA5NTIwMDQsImp0aSI6IjNiOTA3ZDQxLTYzMmEtNGMwOS1hYmI4LWUyOGI3ZTQzYmNhNSIsImVtYWlsIjoiYXBwc2RldmVsb3BlcjEyM0BnbWFpbC5jb20ifQ.EVs6r2m010cY0-Kr7yjEdd1tQiF6mPVDY3jjm807AyhbEu1YkgxuKOc6JnxmzBjEWDZ0n9QOQuSNdZARyHrcZ1wBsqDlnGLrClZ1-XrnNaVt4bQBMGu8oQTSMlnnWuxnWOMB7UlQ9oDs0xEsqnoz3avtBWSN3vd6f1bY4Oeo6erWhZuuD1vuX2nCu6NGjdMeOFifv-8Rk0V4v-NQXwLhTkhurUZ-N6K49PIel90O9AI2V-EsqOhm5SFckq39kMHflUdo6dJquNpEc27PYiBVGCV3ZfXB3RnuWiI8a5Sjf7yf2eHgI9cIoWa_bcSPw0nF1XOFgVz_5zZNVk_Qw0WMQw",
            SK:"PATIENT_1650001697567"

        }
     

        connection.invoke("UserChatList", req).catch(function (err) {
            return console.error(err.toString());
        });
    });

    connection.on("ShowUserChatList", function (data) {
        console.log(data);      
    });
});

//***Show User Chatist Ends***//

  



