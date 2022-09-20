using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using TangyWebClient.Service.IService;

namespace TangyWebClient.Pages.Authentication
{
    public partial class RedirectToLogin
    {

        [CascadingParameter]
        public Task<AuthenticationState> authState { get; set; }

        [Inject]
        public NavigationManager navManager { get; set; }

        private bool NotAuthorized { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var authState = await this.authState;
            if (authState.User.Identity is null || !authState.User.Identity.IsAuthenticated)
            {
                var returnUrl = this.navManager.ToBaseRelativePath(this.navManager.Uri);
                if (string.IsNullOrEmpty(returnUrl))
                {
                    this.navManager.NavigateTo("login");
                }
                else
                {
                    this.navManager.NavigateTo($"login?returnUrl={returnUrl}");
                }
            }
            else
            {
                this.NotAuthorized = true;
            }
        }
    }
}
