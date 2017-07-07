using System;
using infrastructure.libs.validators;

namespace infrastructure.email.models
{
    public class Email
    {
        protected int _id;
        public virtual int Id { get { return _id; } }
        private string _receiver;
        private string _sender;
        private string _body;
        private string _subject;
        private DateTime _datesent;
        private bool _issent;

        // for nHibernate
        protected Email() { }

        public Email(string from, string to, string subject, string body)
        {
            CustomValidators.NotNull(to, "to is required.");
            CustomValidators.NotNull(from, "from is required.");
            CustomValidators.NotNull(subject, "subject collection is required.");
            CustomValidators.NotNull(body, "body collection is required.");

            _receiver = to;
            _sender = from;
            _subject = subject;
            _body = body;
            _datesent = DateTime.Now;
            //_datesent = DateTime.Now;
        }

        public void EmailSent()
        {
            _datesent = DateTime.Now;
            _issent = true;
        }

        public string Receiver
        {
            get { return _receiver; }
        }

        public string Sender
        {
            get { return _sender; }
        }

        public string Body
        {
            get { return _body; }
        }

        public string Subject
        {
            get { return _subject; }
        }
        public bool IsSent
        {
            get { return _issent; }
        }

    }
}
