'use strict'
var isPlaying = false;


//https://developer.mozilla.org/en-US/docs/Web/API/MediaStream_Recording_API/Using_the_MediaStream_Recording_API

function playNextAudio(button) {
    $(button).html('stop'); //icon
    $(button).css('opacity', '1');

    if (isPlaying) return;

    var wordid = $(button).closest('.wordid').data('wordid');
    var partid = $(button).data('partid');
    var typeWriter = new Audio("//" + window.location.host + '/Sound?wordid=' + wordid + '&partid=' + partid + '&cb=' +new Date().getTime());

  

    typeWriter.onended = function () {
        $(button).html('play_arrow');
        isPlaying = false;
    };

    var playPromise = typeWriter.play();
    // In browsers that don’t yet support this functionality,
    // playPromise won’t be defined.
    if (playPromise !== undefined) {
        playPromise.then(function () {
            isPlaying = true;
            // Automatic playback started!
        }).catch(function (error) {
            $(button).html('clear');
            $(button).css('opacity', '0.4');
            // Automatic playback failed.
            // Show a UI element to let the user manually start playback.
            console.log(error);
        });
    }
}
var chunks = [];
var mediaRecorder;
 function startUserMedia(stream) {

    mediaRecorder = new MediaRecorder(stream);
    mediaRecorder.ondataavailable = function (e) {
        chunks.push(e.data);
    }

    mediaRecorder.onstop = function (e) {
        console.log("recorder stopped");

        var blob = new Blob(chunks, { 'type': 'audio/ogg; codecs=opus' });
        chunks = [];

        upload2server(recordingWordId, recordingPartId, blob);
    }
}


 
function startRecording(button) {
    $(button).html('pause_circle_filled'); //icon
    mediaRecorder.start();
    console.log(mediaRecorder.state);
    console.log("recorder started");
}

function stopRecording(button) {
    recordingWordId = $(button).closest('.wordid').data('wordid');
    recordingPartId = $(button).data('partid');

    mediaRecorder.stop();
    console.log(mediaRecorder.state);
    console.log("recorder stopped");

    $(button).html('mic'); //icon
}


var recordingWordId;
var recordingPartId;



window.onload = function init() {
    navigator.getUserMedia = navigator.getUserMedia ||
                         navigator.webkitGetUserMedia ||
                         navigator.mozGetUserMedia;

    if (navigator.getUserMedia) {
        console.log('getUserMedia supported.');
        navigator.getUserMedia({ audio: true }, startUserMedia, function (e) {
            alert('No live audio input: ' + e);
        });
    }
};
 

function upload2server(wordid, partid, blob) {
   
        //post wav to controller
        var formData = new FormData();
        formData.append("File", blob);
        formData.append("WordId", wordid);
        formData.append("PartId", partid);

        $.ajax({
            type: "POST",
            url: '/Sound/Upload',
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                console.log(response.success); 
            },
            error: function (error) {
                console.log("Upload error: " + error);
                alert("error");
            }
        });
   
}

$(document).ready(function () {
    $('.part').on('change', function () {

        var formData = new FormData();
        formData.append("WordId", $(this).closest('.wordid').data('wordid'));
        formData.append("PartId", $(this).attr('id'));
        formData.append("Content", $(this).val());

        $.ajax({
            url: '/Words/ManageWords',
            type: "POST",
            data: formData,
            contentType: false,
            processData: false,
            success: function (data) {
                //alert('success');
            },
            error: function (jXHR, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    });


    //Image upload
    $('input:file').on('change', function () {

        var file = this.files[0];
        //if (file.size > 10000) {
        //    alert('max upload size is 1k')
        //    return;
        //}
        if (this.files.length == 0)
            return;

        //post wav to controller
        var formData = new FormData();
        formData.append("FileUpload", this.files[0]);
        formData.append("WordId", $(this).data('wordid'));
        formData.append("CollectionId", $(this).data('collectionid'));
        $.ajax({
            url: '/Words/UploadImage',
            type: "POST",
            data: formData,
            contentType: false,
            processData: false,
            success: function (data) {
                location.reload();
            },
            error: function (jXHR, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    });


    $('#CollectionName').on('change', function () {
        var formData = new FormData();
        formData.append("CollectionName", $(this).val());
        formData.append("CollectionId", $(this).data('collectionid'));

        $.ajax({
            url: '/Words/ChangeCollectionName',
            type: "POST",
            data: formData,
            contentType: false,
            processData: false,
            success: function (data) {
                //location.reload();
            },
            error: function (jXHR, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    });

});
