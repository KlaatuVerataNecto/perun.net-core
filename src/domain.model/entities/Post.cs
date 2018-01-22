using System;
using domain.model.common;

namespace domain.model
{
    public class Post: Entity
    {
        private int _id;
        private string _title;
        private string _postImage;
        private bool _isPublished;
        private DateTime _dateCreated;

        private string _guid;

        public Post() { }

        public Post(Guid guid, string title, string image)
        {
            // TODO: validate 
            _title = title;
            _postImage = image;
            _guid = guid.ToString();
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
        //public int Id { get => _id; }
        //public string Title { get => _title; }
        //public string PostImage { get => _postimage; }
    }
}
