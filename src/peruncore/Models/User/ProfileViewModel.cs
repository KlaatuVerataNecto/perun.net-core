namespace peruncore.Models.User
{

    public class ProfileViewModel
    {
        private string _username;
        private string _country;
        private string _avatar;
        private string _cover;
        private bool _canUpdate;
        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileViewModel"/> class.
        /// </summary>
        public ProfileViewModel(
            string username,
            string country,
            string avatar, 
            string avatarImageDirURL,
            string cover,
            string coverImageDirURL,
            string imageDefaultDirURL,
            bool canUpdate
            )
        {
            _avatar = (string.IsNullOrWhiteSpace(avatar) ? imageDefaultDirURL + "avatar.jpg" : avatarImageDirURL + avatar);
            _cover = (string.IsNullOrWhiteSpace(cover) ? imageDefaultDirURL + "cover.jpg" : coverImageDirURL + cover);
            _username = username;
            _country = country;
            _canUpdate = canUpdate;
        }

        #region # Properties #

        #region == Public ==
        public string Avatar { get => _avatar; }
        public string Cover { get => _cover; }
        public string Username { get => _username; }
        public string Country { get => _country; }

        public bool CanUpdate { get => _canUpdate; }
        #endregion

        #endregion

    }
}