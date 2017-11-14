using System;
using domain.model.common;

namespace domain.model
{
    public class Post: Entity
    {
        private int _id;
        private string _title;

        public Post() { }

        public Post(string title)
        {
            // TODO: validate 
            _title = title;
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
        public int Id { get => _id; }
        public string Title { get => _title; }
    }
}
