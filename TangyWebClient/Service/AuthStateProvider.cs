using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using Tangy.Common;
using TangyWebClient.Helper;

namespace TangyWebClient.Service
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStore;

        public AuthStateProvider(HttpClient httpClient, ILocalStorageService localStore)
        {
            this.httpClient = httpClient;
            this.localStore = localStore;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string token = await this.localStore.GetItemAsync<string>(SD.Local_Token);
            if(token == null)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType")));

        }
    }
}
