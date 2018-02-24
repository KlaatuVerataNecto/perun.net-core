using command.messages.common;
using infrastructure.libs.validators;
using System;

namespace command.messages.post
{
    public class CreatePostCommand : BaseCommand
    {
        private string _title;
        private string _image;
        private string _urlSlug;

        public CreatePostCommand(Guid guid, string title, string image, string urlSlug)
        {
            CustomValidators.StringNotNullorEmpty(title, "title is empty.");
            CustomValidators.StringNotNullorEmpty(image, "image is empty.");
            CustomValidators.StringNotNullorEmpty(urlSlug, "urlSlug is empty.");
            CustomValidators.NotNull(guid, "guid is empty.");

            _guid = guid;
            _title = title;
            _image = image;
            _urlSlug = urlSlug;
        }
        public string Title { get => _title; }
        public string ImageName { get => _image; }
        public string UrlSlug { get => _urlSlug; }
    }
}
