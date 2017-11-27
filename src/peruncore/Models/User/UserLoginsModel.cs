using infrastructure.user.models;
using peruncore.Config;
using System.Collections.Generic;

namespace peruncore.Models.User
{
    public class UserLoginsModel
    {
        public UserLoginsModel(List<UserLogin> userLoginsList, AuthSchemeSettings authSchemeSettings)
        {
            UserLogins = userLoginsList;
            var lp = new LoginsViewPermissionModel();

            foreach (var l in UserLogins)
            {
                if (l.Provider == authSchemeSettings.Google) lp.isViewGoogle = false;
                if (l.Provider == authSchemeSettings.Facebook) lp.isViewFacebook = false;
                if (l.Provider == authSchemeSettings.Twitter) lp.isViewTwitter = false;
            }
            LoginViewPermission = lp;
        }
        public List<UserLogin> UserLogins { get; }
        public LoginsViewPermissionModel LoginViewPermission { get; }

        public bool IsDeactivateAddAnotherLogin
        {
            get { return !LoginViewPermission.isViewFacebook && !LoginViewPermission.isViewGoogle; }
        }
    }
}
