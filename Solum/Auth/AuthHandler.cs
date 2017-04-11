using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Solum.Service;
using Xamarin.Forms;

namespace Solum.Auth
{
    public class AuthHandler : DelegatingHandler
    {
        private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1);
        private static bool _isReauthenticating;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //Clone the request in case we need to send it again
            var clonedRequest = await CloneRequest(request);
            var response = await base.SendAsync(clonedRequest, cancellationToken);

            //If the token is expired or is invalid, then we need to either refresh the token or prompt the user to log back in
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (_isReauthenticating)
                    return response;

                var service = DependencyService.Get<AzureService>();
                var client = App.Client;

                string authToken = client.CurrentUser.MobileServiceAuthenticationToken;
                await Semaphore.WaitAsync();
                //In case two threads enter this method at the same time, only one should do the refresh (or re-login), the other should just resend the request with an updated header.
                if (authToken != client.CurrentUser.MobileServiceAuthenticationToken) // token was already renewed
                {
                    Semaphore.Release();
                    return await ResendRequest(client, request, cancellationToken);
                }

                _isReauthenticating = true;
                var gotNewToken = false;
                try
                {
                    gotNewToken = await RefreshToken(client);


                    //Otherwise if refreshing the token failed or Facebook\Twitter is being used, prompt the user to log back in via the login screen
                    if (!gotNewToken)
                        gotNewToken = await service.LoginAsync();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Unable to refresh token: " + e);
                }
                finally
                {
                    _isReauthenticating = false;
                    Semaphore.Release();
                }


                if (gotNewToken)
                    if (!request.RequestUri.OriginalString.Contains("/.auth/me"))
                        //do not resend in this case since we're not using the return value of auth/me
                        return await ResendRequest(client, request, cancellationToken);
            }

            return response;
        }


        private async Task<HttpResponseMessage> ResendRequest(IMobileServiceClient client, HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Clone the request
            var clonedRequest = await CloneRequest(request);

            // Set the authentication header
            clonedRequest.Headers.Remove("X-ZUMO-AUTH");
            clonedRequest.Headers.Add("X-ZUMO-AUTH", client.CurrentUser.MobileServiceAuthenticationToken);

            // Resend the request
            return await base.SendAsync(clonedRequest, cancellationToken);
        }

        private async Task<bool> RefreshToken(IMobileServiceClient client)
        {
            var authentication = DependencyService.Get<IAuthentication>();
            if (authentication == null)
                throw new InvalidOperationException("Make sure the ServiceLocator has an instance of IAuthentication");

            try
            {
                return await authentication.RefreshUser(client);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Unable to refresh user: " + e);
            }

            return false;
        }


        private async Task<HttpRequestMessage> CloneRequest(HttpRequestMessage request)
        {
            var result = new HttpRequestMessage(request.Method, request.RequestUri);
            foreach (var header in request.Headers)
                result.Headers.Add(header.Key, header.Value);

            if (request.Content != null && request.Content.Headers.ContentType != null)
            {
                var requestBody = await request.Content.ReadAsStringAsync();
                var mediaType = request.Content.Headers.ContentType.MediaType;
                result.Content = new StringContent(requestBody, Encoding.UTF8, mediaType);
                foreach (var header in request.Content.Headers)
                    if (!header.Key.Equals("Content-Type", StringComparison.OrdinalIgnoreCase))
                        result.Content.Headers.Add(header.Key, header.Value);
            }

            return result;
        }
    }
}