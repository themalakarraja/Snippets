let myPid = "MG19038";
let chatDivClass = "vvTMTb";
let pidArr = ["MG19045", "MG19040", "MG19012", "MG19015", "MG19024"];
let sendBtnClass = "Cs0vCd";
let chatBoxInputName = "chatTextInput";
let chatBoxValuePropertyName = "data-initial-value";
let chatDiv;

var currentTime;

if (document.getElementsByName(chatBoxInputName)[0].getAttribute(chatBoxValuePropertyName).toString() != myPid) {
	alert("Please enter your PID in chatbox");
}

var attendanceTimer = window.setInterval(function(){
	chatDiv = document.getElementsByClassName(chatDivClass)[0].innerHTML.toString();
	for (var pid of pidArr) {
		if(chatDiv.includes(pid)) {
			clearInterval(attendanceTimer);
	    	document.getElementsByClassName(sendBtnClass)[0].click();
			currentTime = new Date();
			console.log("Attendance submitted at " + currentTime.getHours() + ":" + currentTime.getMinutes());
			break;
		}
	}
}, 2000);

var peopleDivClass = "rua5Nb";
var exitBtnClass = "ZPasfd"
var minPeople = 10;
var currentPeople;

var exitTimer = window.setInterval(function(){
	currentPeople = parseInt(document.getElementsByClassName(peopleDivClass)[0].innerHTML.toString().replace("(", "").replace(")", ""));
	if (currentPeople < minPeople) {
		clearInterval(exitTimer);
		document.getElementsByClassName(exitBtnClass)[0].click();
		currentTime = new Date();
		console.log("Exit form meeting at " + currentTime.getHours() + ":" + currentTime.getMinutes());
	}
}, 2000);