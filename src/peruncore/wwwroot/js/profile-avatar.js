Dropzone.autoDiscover = false;
(function() {
    var isUploading = false,
        cropper = undefined,
        $avatarModal = $('#avatar-modal'),
        $avatarCropModal = $('#crop-avatar-modal'),
        $avatarDropzone = $('#avatar-dropzone'),
        $dropzoneError = $('#avatar-error'),
        $dropzoneMessage = $avatarDropzone.find('.dz-message'),
        $startAvatarCrop = $('#start-avatar-crop'),
        $startAvatarUpload = $('#start-avatar-upload');

    var dz = new Dropzone(
        '#avatar-dropzone',
        {
            url: '/avatar/upload',
            params: {
                avatar_x: 0,
                avatar_y: 0,
                avatar_width: 0,
                avatar_height: 0
            },
            paramName: 'avatar_image',
            thumbnailWidth: 300,
            thumbnailHeight: 300,
            clickable: ['#avatar-select-button'],
            autoQueue: false,
            maxFiles: 2,
            maxFilesize: 2,
            acceptedFiles: 'image/*',
            accept: function(file, done) {
                $dropzoneError.text('').hide();
                $startAvatarCrop.removeAttr('disabled');
                $dropzoneMessage.hide();

                if (this.files.length > 1)
                    dz.removeFile(dz.files[0]);

                done();
            },
            error: function(file, message) {
                $dropzoneError.text(message).show();
                if (!isUploading)
                    this.removeFile(file);
                else {
                    $avatarModal.find('button:not(#start-avatar-crop)').removeAttr('disabled');
                    if (this.files.length === 0)
                        $startAvatarCrop.attr('disabled', '');
                }

                isUploading = false;
            },
            success: function(file, response) {
                if (response && response.imageUrl)
                    $('img[data-user-avatar]').attr('src', response.imageUrl);
            },
            previewsContainer: '#avatar-preview',
            previewTemplate:
            '<div class="dz-preview dz-image-preview">' +
            '<div class="dz-image"><img data-dz-thumbnail /></div>' +
            '<div class="dz-details">' +
            '<div class="dz-size"><span data-dz-size></span></div>' +
            '<div class="dz-filename"><span data-dz-name></span></div>' +
            '</div>' +
            '<div class="dz-progress">' +
            '<span class="dz-upload" data-dz-uploadprogress></span>' +
            '</div>' +
            '</div>'
        }
    );

    // Dropzone events
    dz
        .on('removedfile',
            function() {
                if (this.files.length === 0)
                    $dropzoneMessage.show();
            })
        .on('thumbnail',
            function(file) {
                dz.options.params.avatar_x = 0;
                dz.options.params.avatar_y = 0;
                dz.options.params.avatar_width = Math.round(file.width);
                // noinspection JSSuspiciousNameCombination
                dz.options.params.avatar_height = Math.round(file.height);
            })
        .on('sending',
            function() {
                isUploading = true;
                $avatarModal.find('button').attr('disabled', '');
            })
        .on('success',
            function() {
                isUploading = false;
                $avatarModal.modal('hide');
            });

    // Button event
    $startAvatarCrop.on('click',
        function() {
            var files = dz.getAcceptedFiles();
            if (files.length === 0)
                return;

            // Force background to be white
            var canvas = document.createElement('canvas');
            canvas.width = files[0].width;
            canvas.height = files[0].height;
            var ctx = canvas.getContext('2d');

            ctx.beginPath();
            ctx.rect(0, 0, files[0].width, files[0].height);
            ctx.fillStyle = "white";
            ctx.fill();

            var tmpImage = new Image();
            tmpImage.onload = function() {
                ctx.drawImage(tmpImage, 0, 0);

                var img = $avatarCropModal.find('img')[0];
                img.onload = function() {
                    $avatarCropModal.modal('show');
                };
                img.src = canvas.toDataURL('image/jpeg');
            };
            tmpImage.src = files[0].dataURL;
        });

    //
    $startAvatarUpload.on('click',
        function() {
            $avatarCropModal.modal('hide');
            dz.enqueueFiles(dz.getAcceptedFiles());
        });

    // Modal events
    $avatarModal
        .on('show.bs.modal', function() {
            dz.removeAllFiles();
            $avatarModal.find('button:not(#start-avatar-crop)')
                .removeAttr('disabled');
        })
        .on('hide.bs.modal',
            function(e) {
                if (isUploading) {
                    e.preventDefault();
                    e.stopImmediatePropagation();

                    return false;
                }

                return true;
            })
        .on('hidden.bs.modal',
            function() {
                $dropzoneError.text('').hide();
            });

    // Crop modal events
    $avatarCropModal
        .on('shown.bs.modal',
            function() {
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
                        crop: function(e) {
                            dz.options.params.avatar_x = Math.round(e.detail.x);
                            // noinspection JSSuspiciousNameCombination
                            dz.options.params.avatar_y = Math.round(e.detail.y);
                            dz.options.params.avatar_width = Math.round(e.detail.width);
                            // noinspection JSSuspiciousNameCombination
                            dz.options.params.avatar_height = Math.round(e.detail.height);
                        }
                    });
            })
        .on('hidden.bs.modal',
            function() {
                cropper.destroy();
                cropper = undefined;

                $avatarCropModal.find('img')[0].src = '';
            });
})();