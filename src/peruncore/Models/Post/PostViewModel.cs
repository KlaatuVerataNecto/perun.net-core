
namespace peruncore.Models.Post
{
    public class PostViewModel
    {
        private string _title;
        private string _image;

        public PostViewModel(string title, string image)
        {
            // TODO: Validation
            _title = title;
            _image = image;
        }

        public string Title { get => _title;}
        public string Image { get => _image;}
    }
}
