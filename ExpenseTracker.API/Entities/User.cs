namespace ExpenseTracker.API.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public string? RefreshToken { get; private set; }
        public DateTime? RefreshTokenExpiryTime { get; private set; }

        public User()
        {
        }

        public User(string email)
        {
            Email = email;
        }

        public void SetPasswordHash(string passwordHash)
        {
            PasswordHash = passwordHash;
        }

        public void SetRefreshToken(string refreshToken, DateTime expiryTime)
        {
            RefreshToken = refreshToken;
            RefreshTokenExpiryTime = expiryTime;
        }
    }
}
