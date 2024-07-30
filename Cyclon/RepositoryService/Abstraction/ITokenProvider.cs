namespace Cyclone.RepositoryService.Abstraction
{
    public interface ITokenProvider
    {
        bool SetToken(string token);
        string? GetToken();
        void ClearToken();
    }
}
