namespace infrastructure.user.interfaces
{    public interface IAuthSchemeNameService
    {
        string getProviderName(string providerParameter);
        string getDefaultProvider();
    }
}
