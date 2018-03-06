'use strict'
var event = new Event('partFinished');

function playSyllabus(item) {
    stopAllAudios();
    var wordid = $(item).closest('.wordContainer').data('wordid');
    var partid = $(item).data('partid');

    playByArray(wordid, [partid]);
}
function playFullWord(item) {
    stopAllAudios();
    var wordid = $(item).closest('.wordContainer').data('wordid');

    var todo = [];
    $(item).closest('.wordContainer').find('li').each(function (index) {
        if (this.nodeName == "LI")
            todo.unshift($(this).data('partid')); //push() adds to the bottom. unshift() adds to the front
    });
    //console.log(todo.toString());

    playByArray(wordid, todo);
}

function playOwnAndRestore(item) {
    playOwn(item);
    var wordid = $(item).closest('.wordContainer').data('wordid');
    
    setTimeout(function () { restoreCss(wordid); }, 1000);
}

function playOwn(item) {
    if (item == undefined ||  item==null)
        return;

    $(item).find('audio').get(0).play().then(() => {
        $(item).fadeTo('slow', 0.5);
        $(item).css('color', 'black');
        $(item).css('box-shadow', '0px 0px 0px white');
    }).catch((error) => {
        //if (!error.includes("gesture")) {
            //alert("merda:" + error);
            //si no hi ha so gravat
            //tb en android "must be gesture initiated"...
            //$(item).off("click");
            //$(item).html('<i class="material-icons">not_interested</i>');
            //$(item).css("background", "violet");
        //}
        console.log(error);
        });;
    
}

function stopAllAudios() {
    //  $('audio').each(function () { this.pause(); this.currentTime = 0; });
}
function playByArray(wordid, array) {
    
    var partid=array.pop();
    var audio = $('.wordContainer[data-wordid="' + wordid + '"]  li[data-partid="' + partid + '"] audio').get(0);
    //audio.autoplay = true;
    audio.play().then(() => {
        if (array.length > 0) {
            audio.onended = function () {
                playByArray(wordid, array);
            };
        } else {
            audio.onended = function () {
                restoreCss(wordid);
            };
        }
    }).catch((error) => {
        alert("merda:" + error);
    });
    var $li = $('.wordContainer[data-wordid="' + wordid + '"]  li[data-partid="' + partid + '"]');
    $li.fadeTo('slow', 0.5);
    $li.css('color', 'black');
    $li.css('box-shadow', '0px 0px 0px white');
}

function restoreCss(wordid) {
    var $li = $('.wordContainer[data-wordid="' + wordid + '"]  li');
                $li.css('opacity', 1);
                $li.css('color', '#dcac61');
                $li.css('box-shadow', '2px 2px 4px rgba(0, 0, 0, 0.57)');
}


/* events fired on the draggable target */
//document.addEventListener("dragged", function (event) {
//    alert('dropped');
//}, false);

//var contextClass;
//function Init() {
//    contextClass = (window.AudioContext ||
//      window.webkitAudioContext ||
//      window.mozAudioContext ||
//      window.oAudioContext ||
//      window.msAudioContext);
//    if (!contextClass) {
//        alert("Web Audio not available. Only recent versions of Chrome are supported.");
//        return;
//    }
//    // Web Audio API is available.
//    var context = new contextClass();
//    context = new webkitAudioContext();

//    bufferLoader = new BufferLoader(
//      context,
//      [
//        '../sounds/hyper-reality/br-jam-loop.wav',
//        '../sounds/hyper-reality/laughter.wav',
//      ],
//      finishedLoading
//      );

//    bufferLoader.load();

//}
//function finishedLoading(bufferList) {
//    // Create two sources and play them both together.
//    var source1 = context.createBufferSource();
//    var source2 = context.createBufferSource();
//    source1.buffer = bufferList[0];
//    source2.buffer = bufferList[1];

//    source1.connect(context.destination);
//    source2.connect(context.destination);
//    source1.start(0);
//    source2.start(0);
//}
//function playSound(buffer) {
//    var source = context.createBufferSource();
//    source.buffer = buffer;
//    source.connect(context.destination);
//    source.start(0);
//}
//$(function () {
//    Init();
//});