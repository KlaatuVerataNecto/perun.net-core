Dropzone.options.dropzoneForm = {
    maxFilesize: 1,
    acceptedMimeTypes: 'image/jpeg, image/jpg, image/png',
    accept: function (file, done) {
        done();
    },
    uploadprogress: function (file, progress, bytesSent) {
        $("#progressbar").val(progress);
    },
    addedfile: function(){
        $("#progressbar-wrapper").show();
        $("#dropzoneForm").slideUp();
    },
    init: function () {
        this.on("maxfilesexceeded", function (file) {
            alert("No more files please!");
        });
        this.on("success", function (file, response) {
            console.debug(response);
            $("#post_form").show();
            $("#progressbar-wrapper").hide();
            $("#post_image_file").val(response.imageFile);
            $("#post_image_url").attr("src", response.imageUrl);
            $("#post_image_url").show();
  
        });
        this.on('error', function (file, response) {
            $(file.previewElement).find('.dz-error-message').text(response);
        });
    }
};