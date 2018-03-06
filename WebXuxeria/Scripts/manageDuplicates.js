'use strict'
 

$(document).ready(function () {
    $('.ajaxonchange').on('change', function () {

        var formData = new FormData();
        formData.append("duplicateid", $(this).closest('.wordid').data('wordid'));
        formData.append("question", $('#question').val());
        formData.append("answer1", $('#answer1').val()); 
        formData.append("answer2", $('#answer2').val());
        formData.append("answer3", $('#answer3').val());
        formData.append("answer4", $('#answer4').val());
        formData.append("answer1iscorrect", $('#answer1iscorrect').is(":checked"));
        formData.append("answer2iscorrect", $('#answer2iscorrect').is(":checked"));
        formData.append("answer3iscorrect", $('#answer3iscorrect').is(":checked"));
        formData.append("answer4iscorrect", $('#answer4iscorrect').is(":checked"));

        //formData.append("image0", $('#image0')[0].files[0]);
        //formData.append("image1", $('#image1')[0].files[0]);
        //formData.append("image2", $('#image2')[0].files[0]);
        //formData.append("image3", $('#image3')[0].files[0]);
        //formData.append("image4", $('#image4')[0].files[0]);
        $.ajax({
            url: '/Duplicates/ManageDuplicatesAjax',
            type: "POST",
            data: formData,
            contentType: false,
            processData: false,
            success: function (data) {
                //alert('success');
            },
            error: function (jXHR, textStatus, errorThrown) {
                alert(jXHR);
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
        formData.append("DuplicateId", $(this).closest('.wordid').data('wordid'));
        formData.append("Num", $(this).data('num'));
        $.ajax({
            url: '/Duplicates/UploadImage',
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
});
