using ExpenseTracker.MAUI.Models.Auth;
using ExpenseTracker.MAUI.Services;
using System.Threading.Tasks;

namespace ExpenseTracker.MAUI
{
    public partial class App : Application
    {
        private readonly AuthService auth;

        public App(AuthService auth)
        {
            InitializeComponent();

            this.auth = auth;
            MainPage = new AuthShell();
        }

        /// <summary>
        /// Auto login with refresh token and user id from Secure Storage.
        /// </summary>
        protected override async void OnStart()
        {
            base.OnStart();

            var token = await TokenStorage.GetRefreshTokenAsync();
            var userId = await TokenStorage.GetUserIdAsync();

            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(userId))
            {
                var request = new RefreshTokenRequest
                {
                    UserId = Guid.Parse(userId),
                    RefreshToken = token
                };

                var response = await auth.RefreshTokensAsync(request);
                if (response)
                {
                    MainPage = new AppShell();
                    return;
                }
            }

            MainPage = new AuthShell();
        }
    }
}