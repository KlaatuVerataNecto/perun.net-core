namespace infrastructure.email.interfaces
{
    public interface IEmailSettingsService
    {
        string GetHost();
        int GetPort();
        bool GetIsSSL();
        string GetFrom();
        string GetFromname();
        string GetUsername();
        string GetPassword();
    }
}
