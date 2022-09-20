using Microsoft.AspNetCore.Components;
using TangyWebClient.Service.IService;

namespace TangyWebClient.Pages.Authentication
{
    public partial class Logout
    {


        [Inject]
        public IAuthenticationService authService { get; set; }

        [Inject]
        public NavigationManager navManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await this.authService.Logout();
            this.navManager.NavigateTo("/");
        }
    }
}
