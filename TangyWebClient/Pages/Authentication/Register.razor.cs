using Microsoft.AspNetCore.Components;
using System;
using Tangy.Models;
using TangyWebClient.Service.IService;

namespace TangyWebClient.Pages.Authentication
{
    public partial class Register
    {
        private SignupRequestDto SignupRequestDto { get; set; } = new();
        private bool IsProcessing { get; set; } = false;
        public bool ShowRegistrationErrors { get; set; }
        private IEnumerable<string> Errors { get; set; }

        [Inject]
        public IAuthenticationService authService { get; set; }

        [Inject]
        public NavigationManager navManager { get; set; }

        private async Task RegisterUser()
        {
            this.ShowRegistrationErrors = false;
            this.IsProcessing = true;

            var result = await this.authService.RegisterUser(this.SignupRequestDto);

            if (result.IsRegistrationSuccessful)
            {
                this.navManager.NavigateTo("/login");
            }
            else
            {
                // registration failed
                this.Errors = result.Errors;
                this.ShowRegistrationErrors = true;
            }
            this.IsProcessing = true;
        }
    }
}
