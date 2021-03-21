using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using System;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Serialization;
using Dapr.Client;
using IdentityModel.Client;

namespace TimonIdentityServer.Services
{
    public class PublishRegisteredUser : IRequest<PublishRegisteredUserResponse>
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string AccessToken { get; private set; }

        public PublishRegisteredUser(Guid id, string email, string accessToken)
        {
            Id = id;
            Email = email;
            AccessToken = accessToken;
        }
    }

    public class PublishRegisteredUserResponse
    {
        [JsonProperty("clubId")]
        public string ClubId { get; private set; }
        [JsonProperty("userId")]
        public string UserId { get; private set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; private set; }

        public PublishRegisteredUserResponse(string clubId, string userId, string displayName)
        {
            ClubId = clubId;
            UserId = userId;
            DisplayName = displayName;
        }
    }

    public class PublishRegisteredUserHandler : IRequestHandler<PublishRegisteredUser, PublishRegisteredUserResponse>
    {
        readonly DaprClient _daprClient;

        public PublishRegisteredUserHandler(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }
        public async Task<PublishRegisteredUserResponse> Handle(PublishRegisteredUser request, CancellationToken cancellationToken)
        {
            // using var client = new HttpClient
            // {
            //     BaseAddress = new Uri("http://localhost:3500")
            // };

            // client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", request.AccessToken);

            // var payloadJson = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            // {
            //     ContractResolver = new CamelCasePropertyNamesContractResolver()
            // });

            var daprRequest =
              _daprClient.CreateInvokeMethodRequest<PublishRegisteredUser>(
                  "timon-server",
                  "app/users",
                  request
              );

            daprRequest.SetBearerToken(request.AccessToken);

            var result = await _daprClient.InvokeMethodAsync<PublishRegisteredUserResponse>(daprRequest, cancellationToken).ConfigureAwait(false);

            // var content = new StringContent(payloadJson, Encoding.UTF8, "application/json");

            // var resp = await client.PostAsync("/v1.0/invoke/timon-link-server/method/users", content);

            // var json = await resp.Content.ReadAsStringAsync();

            return result;

        }
    }
}
