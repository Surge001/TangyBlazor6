using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Tangy.Common;
using Tangy.Models;
using TangyWebClient.Service.IService;

namespace TangyWebClient.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStore;
        private readonly AuthenticationStateProvider stateProvider;

        public AuthenticationService(HttpClient httpClient, ILocalStorageService localStore, AuthenticationStateProvider stateProvider)
        {
            this.httpClient = httpClient;
            this.localStore = localStore;
            this.stateProvider = stateProvider;
        }

        public async Task<SignInResponseDto> Login(SignInRequestDto request)
        {
            string content = JsonConvert.SerializeObject(request);
            StringContent bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            using (HttpResponseMessage response = await this.httpClient.PostAsync("api/account/signin", bodyContent))
            {
                var contentTemp = await response.Content.ReadAsStringAsync();
                var responseDto = JsonConvert.DeserializeObject<SignInResponseDto>(contentTemp);

                if (response.IsSuccessStatusCode)
                {
                    await this.localStore.SetItemAsync(SD.Local_Token, responseDto.Token);
                    await this.localStore.SetItemAsync(SD.Local_UserDetails, responseDto.UserDto);

                    // Next statement is needed to set httpClient instance authentication token for other calls:
                    this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", responseDto.Token);
                    return new SignInResponseDto() { IsAuthSuccessful = true, Token = responseDto.Token };
                }
                else
                {
                    return responseDto;
                }
            }


        }

        public async Task Logout()
        {
            await this.localStore.RemoveItemAsync(SD.Local_Token);
            await this.localStore.RemoveItemAsync(SD.Local_UserDetails);
            this.httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<SignupResponseDto> RegisterUser(SignupRequestDto request)
        {
            string content = JsonConvert.SerializeObject(request);
            StringContent bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            using (HttpResponseMessage response = await this.httpClient.PostAsync("api/account/SignUp", bodyContent))
            {
                var contentTemp = await response.Content.ReadAsStringAsync();
                var responseDto = JsonConvert.DeserializeObject<SignupResponseDto>(contentTemp);

                if (response.IsSuccessStatusCode)
                {
                    return new SignupResponseDto() { IsRegistrationSuccessful = true };
                }
                else
                {
                    return new SignupResponseDto() { IsRegistrationSuccessful = false, Errors = responseDto?.Errors };
                }
            }
        }
    }
}
