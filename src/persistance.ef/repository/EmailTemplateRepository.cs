using System;
using System.Linq;
using infrastructure.email.repository;
using persistance.ef.common;
using infrastructure.email.entities;

namespace persistance.ef.repository
{
    public class EmailTemplateRepository: IEmailTemplateRepository
    {
        private IEFContext _efContext;

        public EmailTemplateRepository(IEFContext context)
        {
            _efContext = context;
        }

        public EmailTemplateDb getTemplateByType(string templateTypeName)
        {

            var obj = _efContext.EmailTemplates.Where(x => x.template_type == templateTypeName).SingleOrDefault();
            if (obj == null) return null;
            return obj;        
        }
    }
}
