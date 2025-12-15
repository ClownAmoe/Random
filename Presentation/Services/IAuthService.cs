namespace Presentation.Services
{
    public interface IAuthService
    {
        int? CurrentUserId { get; }
        string? CurrentUserNickname { get; }
        void SetCurrentUser(int userId, string nickname);
        void Logout();
        bool IsAuthenticated { get; }
    }
}