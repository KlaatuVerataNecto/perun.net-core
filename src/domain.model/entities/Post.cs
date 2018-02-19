using System;
using domain.model.common;
using infrastructure.libs.validators;

namespace domain.model
{
    public class Post: Entity
    {
        private int _id;
        private string _title;
        private string _postImage;
        private bool _isPublished;
        private string _urlSlug;
        private DateTime _dateCreated;

        private string _guid;

        public Post() { }

        public Post(Guid guid, string title, string image, string urlSlug)
        {
            CustomValidators.StringNotNullorEmpty(title, "title is empty.");
            CustomValidators.StringNotNullorEmpty(image, "image is empty.");
            CustomValidators.StringNotNullorEmpty(urlSlug, "urlSlug is empty.");
            CustomValidators.NotNull(guid, "guid is empty.");

            _title = title;
            _postImage = image;
            _guid = guid.ToString();
            _urlSlug = urlSlug;
            _dateCreated = DateTime.Now;
            _isPublished = true;
        }

        public void VoteUp()
        {
            throw new NotImplementedException();
        }

        public void VoteDown()
        {
            throw new NotImplementedException();
        }

        public void AddTags()
        {
            throw new NotImplementedException();
        }

        public void AddComment()
        {
            throw new NotImplementedException();
        }
    }
}
