(function () {
    cropper = undefined;
    $fileChangeAvatar = $('#file-change-avatar');
    $avatarCropModal = $('#crop-avatar-modal');
    $btnAvatarUpload = $('#btn-avatar-upload');

    var form_data = new FormData();

    //var cropImageOptions = {
    //    avatar_image: null,
    //    avatar_x: null,
    //    avatar_y: null,
    //    avatar_width: null,
    //    avatar_height: null
    //}

    $fileChangeAvatar.change(function () {
        initCanvas();
    });

    $btnAvatarUpload.click(function () {
        form_data.append("avatar_image", $avatarCropModal.find('img')[0]);          

        $.ajax('/avatar/upload', {
            method: "POST",
            data: form_data,
            contentType: false,
            processData: false,
            success: function () {
                console.log('Upload success');
            },
            error: function () {
                console.log('Upload error');
            }
        });

    });

    function initCanvas()
    {
        if ($fileChangeAvatar.prop('files') &&
            $fileChangeAvatar.prop('files')[0]) {

            // create canvas
            var canvas = document.createElement('canvas');        

            // create temporrary image
            var tmpImage = new Image();

            tmpImage.onload = function () {  
                // set canvas properties
                canvas.width = tmpImage.width;
                canvas.height = tmpImage.height;
                var ctx = canvas.getContext('2d');              
                ctx.beginPath();
                ctx.rect(0, 0, tmpImage.width,tmpImage.height);
                ctx.fillStyle = "white";
                ctx.fill();
                // draw canvas
                ctx.drawImage(tmpImage, 0, 0);

                // set canvas in crop modal's image element
                var img = $avatarCropModal.find('img')[0];
                img.src = canvas.toDataURL('image/jpeg');

                img.onload = function () {
                    // trigger crop modal
                    $avatarCropModal.modal('show');
                    $avatarCropModal.show();
                };                
            };

            var reader = new FileReader();
            reader.onload = function (e) {
                // read input file and set it as source of temporary image
                tmpImage.src = e.target.result;
            };
            reader.readAsDataURL($fileChangeAvatar.prop('files')[0]);
        }
    }

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
                        form_data.append("avatar_image", $avatarCropModal.find('img')[0]);
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

//Dropzone.autoDiscover = false;

//(function() {
//    var isUploading = false,
//        cropper = undefined,
//        $avatarModal = $('#avatar-modal'),
//        $avatarCropModal = $('#crop-avatar-modal'),
//        $avatarDropzone = $('#avatar-dropzone'),
//        $dropzoneError = $('#avatar-error'),
//        $dropzoneMessage = $avatarDropzone.find('.dz-message'),
//        $startAvatarUpload = $('#start-avatar-upload');

//    var dz = new Dropzone(
//        '#avatar-dropzone',
//        {
//            url: '/avatar/upload',
//            params: {
//                avatar_x: 0,
//                avatar_y: 0,
//                avatar_width: 0,
//                avatar_height: 0
//            },
//            paramName: 'avatar_image',
//            //thumbnailWidth: 120,
//            //thumbnailHeight: 120,
//            clickable: ['#avatar-select-button'],
//            autoQueue: false,
//            maxFiles: 2,
//            maxFilesize: 2,
//            acceptedFiles: 'image/*',
//            accept: function (file, done) {
//                $dropzoneError.text('').hide();
//                $dropzoneMessage.hide();

//                if (this.files.length > 1) {
//                    dz.removeFile(dz.files[0]);
//                }               
//                done();
//            },
//            error: function(file, message) {
//                $dropzoneError.text(message).show();
//                if (!isUploading) {
//                    this.removeFile(file);
//                }
//                else {
//                    $avatarModal.find('button:not(#start-avatar-crop)').removeAttr('disabled');
//                }

//                isUploading = false;
//            },
//            success: function(file, response) {
//                if (response && response.imageUrl) {
//                    $('img[data-user-avatar]').attr('src', response.imageUrl);
//                }
//            },
//            //previewsContainer: '#avatar-preview',
//            //previewTemplate:
//            //'<div class="dz-preview dz-image-preview">' +
//            // '<div class="dz-image"><img data-dz-thumbnail /></div>' +
//            // '<div class="dz-details">' +
//            // '<div class="dz-size"><span data-dz-size></span></div>' +
//            // '<div class="dz-filename"><span data-dz-name></span></div>' +
//            // '</div>' +
//            // '<div class="dz-progress">' +
//            // '<span class="dz-upload" data-dz-uploadprogress></span>' +
//            // '</div>' +
//            //'</div>'
//        }
//    );

//    // Dropzone events
//    dz
//        .on('thumbnail',
//            function(file) {
//                dz.options.params.avatar_x = 0;
//                dz.options.params.avatar_y = 0;
//                dz.options.params.avatar_width = Math.round(file.width);
//                // noinspection JSSuspiciousNameCombination
//                dz.options.params.avatar_height = Math.round(file.height);
//                $avatarModal.modal('hide');
//                startAvatarCrop();
//            })
//        .on('sending',
//            function() {
//                isUploading = true;
//                $avatarModal.find('button').attr('disabled', '');
//            })
//        .on('success',
//            function() {
//                isUploading = false;
//                $avatarModal.modal('hide');               
//            });

//    function startAvatarCrop() {
//        var files = dz.getAcceptedFiles();
//        if (files.length === 0) {
//            return;
//        }

//        // Force background to be white
//        var canvas = document.createElement('canvas');
//        canvas.width = files[0].width;
//        canvas.height = files[0].height;
//        var ctx = canvas.getContext('2d');

//        ctx.beginPath();
//        ctx.rect(0, 0, files[0].width, files[0].height);
//        ctx.fillStyle = "white";
//        ctx.fill();

//        var tmpImage = new Image();
//        tmpImage.onload = function () {
//            ctx.drawImage(tmpImage, 0, 0);

//            var img = $avatarCropModal.find('img')[0];
//            img.onload = function () {
//                $avatarCropModal.modal('show');
//                $avatarCropModal.show();
//            };
//            img.src = canvas.toDataURL('image/jpeg');
//        };
        
//        var reader = new FileReader();
//        reader.onload = function (dt) {
//            tmpImage.src = dt.currentTarget.result;
//        };
        
//        reader.readAsDataURL(files[0]);
//    }

//    // Button events

//    //
//    $startAvatarUpload.on('click',
//        function() {
//            $avatarCropModal.modal('hide');
//            dz.enqueueFiles(dz.getAcceptedFiles());
//        });

//    // Modal events
//    $avatarModal
//        .on('show.bs.modal', function() {
//            dz.removeAllFiles();
//            $avatarModal.find('button:not(#start-avatar-crop)')
//                .removeAttr('disabled');
//        })
//        .on('hide.bs.modal',
//            function(e) {
//                if (isUploading) {
//                    e.preventDefault();
//                    e.stopImmediatePropagation();
//                    return false;
//                }
//                return true;
//            })
//        .on('hidden.bs.modal',
//            function() {
//                $dropzoneError.text('').hide();
//            });