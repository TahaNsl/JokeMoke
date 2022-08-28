//using Microsoft.AspNetCore.Components.Authorization;
//using System.Net.Http.Json;
//using System.Security.Claims;

//namespace JokeMoke.Client
//{
//    public class CustomAuthStateProvider : AuthenticationStateProvider
//    {

//        private readonly HttpClient _httpClient;


//        public CustomAuthStateProvider()
//        {

//        }

//        public CustomAuthStateProvider(HttpClient httpClient)
//        {
//            _httpClient = httpClient;
//        }

//        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
//        {
//            User currentUser = await _httpClient.GetFromJsonAsync<User>("user/getcurrentuser");

//            if (currentUser != null && currentUser.Email != null)
//            {
//                // create claim

//                var claimEmailAdress = new Claim(ClaimTypes.Name, currentUser.Email);

//                var claimNameIdentifier = new Claim(ClaimTypes.NameIdentifier, Convert.ToString(currentUser.Id));

//                var claimRole = new Claim(ClaimTypes.Role, Convert.ToString(currentUser.Role.Name == null ? "" : currentUser.Role.Name));

//                // create claimsIdentity
//                var claimsIdentity = new ClaimsIdentity(new[] { claimEmailAdress, claimNameIdentifier, claimRole }, "serverAuth");

//                // create claimsPrincipal
//                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

//                return new AuthenticationState(claimsPrincipal);
//            }
//            else
//                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
//        }
//    }
//}