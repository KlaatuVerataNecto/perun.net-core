
namespace peruncore.Models.Post
{
    public class PostViewModel
    {
        private string _title;
        private string _image;
        private bool _isThanks;

        public PostViewModel(string title, string image, bool isThanks = false)
        {
            // TODO: Validation
            _title = title;
            _image = image;
            _isThanks = isThanks;
        }

        public string Title { get => _title;}
        public string Image { get => _image;}
        public bool IsThanksOnCreate { get => _isThanks; }
    }
}
