using Microsoft.AspNetCore.Components;
using Tangy.Models;
using TangyWebClient.Service.IService;

namespace TangyWebClient.Pages.Authentication
{
    public partial class Login
    {
        private SignInRequestDto SignInRequestDto { get; set; } = new();
        private bool IsProcessing { get; set; } = false;
        public bool ShowLoginErrors { get; set; }
        private string ErrorMessage { get; set; }

        [Inject]
        public IAuthenticationService authService { get; set; }

        [Inject]
        public NavigationManager navManager { get; set; }

        private async Task LoginUser()
        {
            this.ShowLoginErrors = false;
            this.IsProcessing = true;

            var result = await this.authService.Login(this.SignInRequestDto);

            if (result.IsAuthSuccessful)
            {
                this.navManager.NavigateTo("/");
            }
            else
            {
                // registration failed
                this.ErrorMessage = result.ErrorMessage;
                this.ShowLoginErrors = true;
            }
            this.IsProcessing = true;
        }
    }
}
