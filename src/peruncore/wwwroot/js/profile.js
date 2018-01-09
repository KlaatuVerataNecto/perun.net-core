(function () 
{
  
    $('#image-cropper').cropit(
        {
            onImageError: function () {
                coverRepositionEnd();
            }
        }
    );

    mimeType = undefined;
    $fileChangeAvatar = $('#file-change-avatar');
    $avatarCropModal = $('#crop-avatar-modal');
    $btnAvatarUpload = $('#btn-avatar-upload');
    $btnAvatarCancel = $('#btn-avatar-cancel');

    $imageAvatar = $(".img-avatar");
    $preloader = $(".preloader");
    $imageAvatarPreview = $("#preview-image");
    $input = $("#file-change-avatar");


    $fileChangeCover = $("#file-change-cover");

    $fileChangeCover.change(function ()
    {
        var oFReader = new FileReader();

        oFReader.onload = function (oFREvent) {
            coverRepositionStart();
            $('#image-cropper').cropit('imageSrc', this.result);
        };

        oFReader.readAsDataURL(this.files[0]);
    });


    function coverRepositionStart()
    {
        $(".change-cover").hide();
        $(".profile-cover .media").hide();
        $("#change-cover-buttons").show();
        
    }

    function coverRepositionEnd()
    {
        $(".change-cover").show();
        $(".profile-cover .media").show();
        $("#change-cover-buttons").hide();
    }


    $fileChangeAvatar.change(function() 
    {
        var oFReader = new FileReader();

        oFReader.onload = function (oFREvent) 
        {
            mimeType = dataURLtoMimeType(this.result);
            $imageAvatarPreview.attr('src', this.result);
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

        oFReader.onloadend = function (oFREvent)
        {
            $avatarCropModal.show();
        };

        oFReader.readAsDataURL(this.files[0]);
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
                beforeSend: function (xhr) {
                    avatarStart();
                },
                success: function (data) {
                    $imageAvatar.attr("src", data.imageUrl);
                    avatarDone();
                },
                error: function () {
                    avatarDone();
                }
            });

        }, mimeType, 0.9);

    });

    $btnAvatarCancel.click(function ()
    {
        avatarDone();
    });

    function avatarStart()
    {
        $preloader.show();
        $btnAvatarUpload.prop('disabled', true);
        $btnAvatarCancel.prop('disabled', true);
    }

    function avatarDone()
    {
        $avatarCropModal.hide();
        $imageAvatarPreview.attr('src', "");
        $input.val("");
        cropper.destroy();
        cropper = undefined;
        $preloader.hide();
        $btnAvatarUpload.prop('disabled', false);
        $btnAvatarCancel.prop('disabled', false);
    }      
})();



// TODO: separate this to common library

function dataURLtoMimeType(dataURL)
{
        var BASE64_MARKER = ';base64,';
        var data;

        if (dataURL.indexOf(BASE64_MARKER) == -1)
        {
            var parts = dataURL.split(',');
            var contentType = parts[0].split(':')[1];
            data = decodeURIComponent(parts[1]);
        }
        else
        {
            var parts = dataURL.split(BASE64_MARKER);
            var contentType = parts[0].split(':')[1];
            var raw = window.atob(parts[1]);
            var rawLength = raw.length;

            data = new Uint8Array(rawLength);

            for (var i = 0; i < rawLength; ++i)
            {
                data[i] = raw.charCodeAt(i);
            }
        }

        var arr = data.subarray(0, 4);
        var header = "";
        for (var i = 0; i < arr.length; i++)
        {
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