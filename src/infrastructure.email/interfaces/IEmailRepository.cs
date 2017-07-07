using infrastructure.email.models;
using System.Collections.Generic;

namespace infrastructure.email.repository
{
    public interface IEmailRepository
    {
        Email Add(Email entity);
        List<Email> GetAllNotSent();
    }
}
