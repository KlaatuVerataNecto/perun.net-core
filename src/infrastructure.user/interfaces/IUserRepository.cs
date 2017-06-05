using System.Collections.Generic;
using infrastructure.user.models;

namespace infrastructure.user.interfaces
{   public interface IUserRepository
    {
        UserAuthDB GetByEmail(string email);
    }
}
