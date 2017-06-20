
namespace peruncore.Config
{
    public class AuthSettings
    {
        public int SaltLength { get; set; }
        public int ExpiryDays { get; set; }
        public int ResetTokenLength { get; set; }
    }
}
