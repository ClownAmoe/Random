namespace Presentation.Services
{
    public class AuthService : IAuthService
    {
        private int? _currentUserId;
        private string? _currentUserNickname;

        public int? CurrentUserId => _currentUserId;
        public string? CurrentUserNickname => _currentUserNickname;
        public bool IsAuthenticated => _currentUserId.HasValue;

        public void SetCurrentUser(int userId, string nickname)
        {
            _currentUserId = userId;
            _currentUserNickname = nickname;
        }

        public void Logout()
        {
            _currentUserId = null;
            _currentUserNickname = null;
        }
    }
}