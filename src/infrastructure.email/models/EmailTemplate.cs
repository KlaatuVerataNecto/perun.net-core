using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using infrastructure.libs.validators;

namespace infrastructure.email.models
{
    public class EmailTemplate
    {
        protected int _id;
        public virtual int Id { get { return _id; } }
        private string _sender;
        private string _body;
        private string _subject;
        private string _templateType;

        // for nHibernate
        protected EmailTemplate() { }

        public EmailTemplate(string sender, string subject, string body, string template_type)
        {
            CustomValidators.StringNotNullorEmpty(template_type, "type is required.");
            CustomValidators.StringNotNullorEmpty(subject, "subject collection is required.");
            CustomValidators.StringNotNullorEmpty(body, "body collection is required.");
            CustomValidators.StringNotNullorEmpty(sender, "sender collection is required.");

            _templateType = template_type;
            _subject = subject;
            _body = body;
            _sender = sender;
        }
        public string TemplateType
        {
            get { return _templateType; }
        }

        public string Body
        {
            get { return _body; }
        }

        public string Subject
        {
            get { return _subject; }
        }

        public string Sender
        {
            get { return _sender; }
        }

    }
}
