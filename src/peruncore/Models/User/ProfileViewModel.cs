namespace peruncore.Models.User
{

    public class ProfileViewModel
    {
        private string _username;
        private string _country;
        private string _avatar;
        private bool _canUpdate;
        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileViewModel"/> class.
        /// </summary>
        public ProfileViewModel(
            string username,
            string country,
            string avatar, 
            string avatarImageDirURL, 
            string imageDefaultDirURL,
            bool canUpdate
            )
        {
            _avatar = (string.IsNullOrWhiteSpace(avatar) ? imageDefaultDirURL + "avatar.jpg" : avatarImageDirURL + avatar);
            _username = username;
            _country = country;
            _canUpdate = canUpdate;
        }

        #region # Properties #

        #region == Public ==
        public string Avatar { get => _avatar; }
        public string Username { get => _username; }
        public string Country { get => _country; }

        public bool CanUpdate { get => _canUpdate; }
        #endregion

        #endregion

    }
}