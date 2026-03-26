namespace Application.Services
{
    public interface ILiveTokenService
    {
        Task<string> GenerateTokenAsync(string roomId, string userId, string role, bool canPublish);
    }
}
