(function () 
{
    MIN_COVER_WIDTH = 1140;
    MIN_COVER_HEIGHT = 350;

    mimeType = undefined;

    $fileChangeAvatar = $('#file-avatar-change');
    $avatarCropModal = $('#crop-avatar-modal');
    $btnAvatarUpload = $('#btn-avatar-save');
    $btnAvatarCancel = $('#btn-avatar-cancel');
    $avatarImage = $('.img-avatar');
    $avatarImagePreview = $('#preview-image');

    $coverImage = $('.profile-cover-img');
    $coverCropper = $('#image-cropper');
    $fileChangeCover = $('#file-cover-change');
    $btnCoverSave = $('#btn-cover-save');
    $btnCoverCancel = $('#btn-cover-cancel');
    $coverMessageModal = $('#cover-message-modal');
    $btnCoverMessageCancel = $('#btn-cover-message-cancel');

   // ------------ Cover Photo ------------

    $fileChangeCover.change(function () {

        $coverCropper.cropit(
            {
                onImageError: function () {
                    $coverMessageModal.show();
                    coverDone();
                }
            }
        );

        var oFReader = new FileReader();

        oFReader.onload = function (oFREvent) {
            var image = this.result;
            validFileImageSize(image, MIN_COVER_WIDTH, MIN_COVER_HEIGHT, function (result) {
                if (result) {
                    coverStart();
                    $coverCropper.cropit('imageSrc', image);
                } else {
                    $coverMessageModal.show();
                    coverDone();
                }
            });
        };

        oFReader.readAsDataURL(this.files[0]);
    });

    $btnCoverSave.click(function () {

        var imageData = $coverCropper.cropit('export', {
            type: 'image/jpeg',
            quality: 1
        });

        var blob = dataURItoBlob(imageData);

        var formData = new FormData();
        formData.append('cover_image', blob); 

        $.ajax('/image/cover', {
            method: "POST",
            data: formData,
            contentType: false,
            processData: false,
            beforeSend: function (xhr) {
                 coverUploadStart();
            },
            success: function (data) {
                coverUploadDone();
                $coverImage.css('background-image','url('+data.imageUrl+')');
            },
            error: function () {
                coverUploadDone();
                $coverMessageModal.show();
            }
        });

    });

    $btnCoverCancel.click(function () {
        coverDone();
    });

    $btnCoverMessageCancel.click(function () {
        $coverMessageModal.hide();
    });

    // TODO: selectors to vars

    function coverUploadStart() {
        $('.overlay-cover').show();
    }
    function coverUploadDone() {
        $('.overlay-cover').hide();
        coverDone();
    }

    function coverStart() {
        $('.change-cover').hide();
        $('.profile-cover .media').hide();
        $('#change-cover-buttons').show();
    }

    function coverDone() {
        $fileChangeCover.val('');
        $('.cropit-preview').removeClass('cropit-image-loaded');
        $('.cropit-preview-image').removeAttr('style');
        $('.cropit-preview-image').attr('src', '');
        $coverCropper.cropit('destroy');
        $('.change-cover').show();
        $('.profile-cover .media').show();
        $('#change-cover-buttons').hide();
    }


    // ------------ Profile Photo ------------

    $fileChangeAvatar.change(function() {
        var oFReader = new FileReader();

        oFReader.onload = function (oFREvent) {
            mimeType = dataURLtoMimeType(this.result);
            $avatarImagePreview.attr('src', this.result);
            cropper = new Cropper(
                $avatarCropModal.find('img')[0], {
                    viewMode: 1,
                    aspectRatio: 1 / 1,
                    responsive: true,
                    zoomable: false,
                    rotable: false,
                    scalable: false,
                    minCropBoxWidth: 300,
                    minCropBoxHeight: 300
                });               
        };

        oFReader.onloadend = function (oFREvent) {
            $avatarCropModal.show();
        };

        oFReader.readAsDataURL(this.files[0]);
  });

    $btnAvatarUpload.click(function () {
        cropper.getCroppedCanvas().toBlob(function (blob) {
            var formData = new FormData();
            formData.append('avatar_image', blob); 

            $.ajax('/image/avatar', {
                method: "POST",
                data: formData,
                contentType: false,
                processData: false,
                beforeSend: function (xhr) {
                    avatarStart();
                },
                success: function (data) {
                    $avatarImage.attr('src', data.imageUrl);
                    avatarDone();
                },
                error: function () {
                    avatarDone();
                }
            });

        }, mimeType, 0.9);

    });

    $btnAvatarCancel.click(function () {
        avatarDone();
    });

    function avatarStart() {
        $('.overlay-avatar').show();
        $btnAvatarUpload.prop('disabled', true);
        $btnAvatarCancel.prop('disabled', true);
    }

    function avatarDone() {
        $('.overlay-avatar').hide();
        $avatarCropModal.hide();
        $avatarImagePreview.attr('src', '');
        $fileChangeAvatar.val('');
        cropper.destroy();
        cropper = undefined;
        $btnAvatarUpload.prop('disabled', false);
        $btnAvatarCancel.prop('disabled', false);
    }      
})();



// TODO: separate this to common library

function validFileImageSize(file, minwidth, minheight, callback) {

    getFileImageSize(file, function (sizeAttributes) {
        var result = true;
        if (sizeAttributes.width < minwidth || sizeAttributes.height < minheight) {
            result =  false;
        }
        callback(result);
    });
}

function getFileImageSize(file, callback) {
    var img = new Image();
    img.onload = function () {
        var myResult = {
            width: img.width,
            height: img.height
        }
        callback(myResult)
    };
    img.src = file;
}

function dataURItoBlob(dataURI) {
    // convert base64 to raw binary data held in a string
    // doesn't handle URLEncoded DataURIs - see SO answer #6850276 for code that does this
    var byteString = atob(dataURI.split(',')[1]);

    // separate out the mime component
    var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0]

    // write the bytes of the string to an ArrayBuffer
    var ab = new ArrayBuffer(byteString.length);

    // create a view into the buffer
    var ia = new Uint8Array(ab);

    // set the bytes of the buffer to the correct values
    for (var i = 0; i < byteString.length; i++) {
        ia[i] = byteString.charCodeAt(i);
    }

    // write the ArrayBuffer to a blob, and you're done
    var blob = new Blob([ab], { type: mimeString });
    return blob;

}

function dataURLtoMimeType(dataURL) {
        var BASE64_MARKER = ';base64,';
        var data;

        if (dataURL.indexOf(BASE64_MARKER) == -1) {
            var parts = dataURL.split(',');
            var contentType = parts[0].split(':')[1];
            data = decodeURIComponent(parts[1]);
        }
        else {
            var parts = dataURL.split(BASE64_MARKER);
            var contentType = parts[0].split(':')[1];
            var raw = window.atob(parts[1]);
            var rawLength = raw.length;

            data = new Uint8Array(rawLength);

            for (var i = 0; i < rawLength; ++i) {
                data[i] = raw.charCodeAt(i);
            }
        }

        var arr = data.subarray(0, 4);
        var header = "";
        for (var i = 0; i < arr.length; i++) {
            header += arr[i].toString(16);
        }
        switch (header)
        {
            case "89504e47":
                mimeType = "image/png";
                break;
            case "47494638":
                mimeType = "image/gif";
                break;
            case "ffd8ffe0":
            case "ffd8ffe1":
            case "ffd8ffe2":
                mimeType = "image/jpeg";
                break;
            default:
                mimeType = "";
                break;
        }

        return mimeType;
    }