Dropzone.options.dropzoneForm = {
    maxFilesize: 1,
    acceptedMimeTypes: 'image/jpeg, image/jpg, image/png',
    accept: function (file, done) {
        done();
    },
    uploadprogress: function (file, progress, bytesSent) {
        //$(".progress-text").html(progress + "%");
        $("#progressbar").val(progress);
    },
    init: function () {
        this.on("maxfilesexceeded", function (file) {
            alert("No more files please!");
        });
        this.on("success", function (file, response) {
            file.previewElement.innerHTML = "";
            var obj = jQuery.parseJSON(response)
            console.log(obj);
        });
        this.on('error', function (file, response) {
            $(file.previewElement).find('.dz-error-message').text(response);
        });
    }
};