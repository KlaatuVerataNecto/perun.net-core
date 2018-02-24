/* ------------------------------------------------------------------------
 * Post
 * -------------------------------------------------------------------------
 */

var Post = function () {

    var handleUpload = function () {

        Dropzone.options.dropzoneForm = {
            maxFilesize: 1,
            acceptedMimeTypes: 'image/jpeg, image/jpg, image/png',
            accept: function (file, done) {
                done();
            },
            uploadprogress: function (file, progress, bytesSent) {
                $("#progressbar").val(progress);
            },
            addedfile: function () {
                $("#progressbar-wrapper").show();
                $("#dropzoneForm").hide();
            },
            init: function () {
                this.on("maxfilesexceeded", function (file) {
                    $("#progressbar-wrapper").hide();
                    $("#post-upload-error-msg").html("One file per submission only.");
                    $("#post-upload-error").show();
                });
                this.on("success", function (file, response) {
                    $("#post_upload_error").hide();
                    $("#post_form").show();
                    $("#progressbar-wrapper").hide();
                    $("#post_image_file").val(response.imageFile);
                    $("#post_image_url").attr("src", response.imageUrl);
                    $("#post_image_url").show();

                });
                this.on('error', function (file, response) {
                    $("#progressbar-wrapper").hide();
                    $("#post-upload-error-msg").html(response);
                    $("#post-upload-error").show();
                });
            }
        };

        $("#post-try-again-button").click(function () {
            $("#post-upload-error").hide();
            $("#post-upload-error-msg").html("");
            $("#progressbar-wrapper").hide();
            $("#post_image_file").val("");
            $("#post_image_url").attr("src", "");
            $("#post_image_url").hide();
            $("#post_form").hide();
            $("#dropzoneForm").slideDown();
        });

    }

    return {
        //main function to initiate the module
        init: function () {
            handleUpload();
        }
    };

}();