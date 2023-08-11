"use strict";

const headers = {
    headers: {
        "Apikey": "NomfyDUE47FfECdQT2GK5h51JXLCI5xT"
    }
}

var connection = new signalR.HubConnectionBuilder().withUrl("http://localhost:8000/app/chatHub", headers).build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});


var myHeaders = new Headers();
myHeaders.append("accept", "text/plain");
myHeaders.append("Apikey", "NomfyDUE47FfECdQT2GK5h51JXLCI5xT");

var requestOptions = {
    method: 'GET',
    headers: myHeaders,
    redirect: 'follow'
};

fetch("http://localhost:8000/app/WeatherForecast", requestOptions)
    .then(response => response.text())
    .then(result => console.log(result))
    .catch(error => console.log('error', error));