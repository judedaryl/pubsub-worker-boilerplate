﻿using IdentityModel.Client;
using PubSubWorkerStarter.Contracts;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PubSubWorkerStarter.Handlers
{
    public class ProtectedApiBearerTokenHandler : DelegatingHandler
    {
        private readonly IAuthServerConnect _authServerConnect;
        public ProtectedApiBearerTokenHandler(IAuthServerConnect authServerConnect)
        {
            _authServerConnect = authServerConnect;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // request the access token
            var accessToken = await _authServerConnect.RequestClientCredentialsTokenAsync();

            // set the bearer token to the outgoing request as Authentication Header
            request.SetBearerToken(accessToken);

            // Proceed calling the inner handler, that will actually send the request to our protected api
            return await base.SendAsync(request, cancellationToken);
        }
    }
}