var startIndex = 1
var endIndex = -1
var finishDays = -1

startIndex = window.prompt("Start Index: ", startIndex);
endIndex = window.prompt("End Index: ", endIndex);
finishDays = window.prompt("FinishDays: ", finishDays);

var text = [... document.querySelectorAll("ytd-thumbnail-overlay-time-status-renderer > span")];
var totalSec = 0;
var playlist = [];

function convertToTime(tsec){
	var hour = parseInt(tsec/3600);
	var temp = tsec%3600;
	var sec = temp%60;
	var min = (temp - sec)/60
	return hour + ":" + min + ":" + sec;
}

for (var element of text) {
    var dur = element.textContent.trim().split(':');
	var vLength;
	if (dur.length == 3) {
		vLength = parseInt(dur[0]) * 3600 + parseInt(dur[1]) * 60 + parseInt(dur[2]);
	}
	else if (dur.length == 2) {
		vLength = parseInt(dur[0]) * 60 + parseInt(dur[1]);
	}
	else if (dur.length == 1) {
		vLength = parseInt(dur[0]);
	}
	else {
		console.log("Else");
	}
	totalSec += vLength;
	playlist.push(vLength);
}

var customDuration = 0;
var customeVideos = 0;
for (var i = startIndex-1; i < playlist.length; i++) {
	customDuration += playlist[i];
	customeVideos++;
	if(endIndex != -1 && endIndex-1 == i)
        break;
}

console.log("Start index " + startIndex);
console.log("End index " + endIndex);
console.log("Total videos " + playlist.length);
console.log("Total duration " + convertToTime(totalSec));
console.log("Custom videos " + customeVideos);
console.log("Custom duration " + convertToTime(customDuration));
if(finishDays != -1) {
	console.log("Finish days " + finishDays);
	console.log("Dedicate time " + convertToTime(parseInt(customDuration/finishDays)));
}	