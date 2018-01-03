(function () 
{
    mimeType = undefined;
    $fileChangeAvatar = $('#file-change-avatar');
    $avatarCropModal = $('#crop-avatar-modal');
    $btnAvatarUpload = $('#btn-avatar-upload');

    var $image = $("#preview-image");
    var $input = $("#file-change-avatar");

    $fileChangeAvatar.change(function() 
    {
        var oFReader = new FileReader();
        oFReader.readAsDataURL(this.files[0]);
        oFReader.onload = function (oFREvent) 
        {
            mimeType = dataURLtoMimeType(this.result);
            $image.attr('src', this.result);
            cropper = new Cropper(
                $avatarCropModal.find('img')[0],
                {
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
  });

    $btnAvatarUpload.click(function () 
    {
        cropper.getCroppedCanvas().toBlob(function (blob) 
        {
            var formData = new FormData();
            formData.append("avatar_image", blob); 

            $.ajax('/avatar/upload', {
                method: "POST",
                data: formData,
                contentType: false,
                processData: false,
                success: function () {
                    console.log('Upload success');
                },
                error: function () {
                    console.log('Upload error');
                }
            });

        }, mimeType, 0.9);

    });

    // Crop modal events
    $avatarCropModal
        .on('shown.bs.modal',
        function () {
            // noinspection PointlessArithmeticExpressionJS

            cropper = new Cropper(
                $avatarCropModal.find('img')[0],
                {
                    viewMode: 1,
                    aspectRatio: 1 / 1,
                    zoomable: false,
                    rotable: false,
                    scalable: false,
                    minCropBoxWidth: 300,
                    minCropBoxHeight: 300,
                    crop: function (e) {
                        form_data.append("avatar_x", Math.round(e.detail.x));
                        form_data.append("avatar_y", Math.round(e.detail.y));
                        form_data.append("avatar_width",Math.round(e.detail.width));
                        form_data.append("avatar_height",Math.round(e.detail.height));
                    }
                });
        })
        .on('hidden.bs.modal',
        function () {
            cropper.destroy();
            cropper = undefined;
            $avatarCropModal.find('img')[0].src = '';
        });

})();

// TODO: separate this to common library

function dataURLtoMimeType(dataURL) {
        var BASE64_MARKER = ';base64,';
        var data;

        if (dataURL.indexOf(BASE64_MARKER) == -1) {
            var parts = dataURL.split(',');
            var contentType = parts[0].split(':')[1];
            data = decodeURIComponent(parts[1]);
        } else {
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
        for(var i = 0; i < arr.length; i++) {
            header += arr[i].toString(16);
        }
        switch (header) {
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
                mimeType = ""; // Or you can use the blob.type as fallback
                break;
        }

        return mimeType;
    }