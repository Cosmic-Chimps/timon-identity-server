using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using System;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Serialization;

public class PublishRegisteredUser : IRequest<PublishRegisteredUserResponse>
{
  public Guid Id { get; private set; }
  public String Email { get; private set; }
  public String AccessToken { get; private set; }

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

  public PublishRegisteredUserResponse(string clubId, string userId)
  {
    ClubId = clubId;
    UserId = userId;
  }
}

public class PublishRegisteredUserHandler : IRequestHandler<PublishRegisteredUser, PublishRegisteredUserResponse>
{
  public async Task<PublishRegisteredUserResponse> Handle(PublishRegisteredUser request, CancellationToken cancellationToken)
  {
    using var client = new HttpClient
    {
      BaseAddress = new Uri("http://localhost:3500")
    };

    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", request.AccessToken);

    var payloadJson = JsonConvert.SerializeObject(request, new JsonSerializerSettings
    {
      ContractResolver = new CamelCasePropertyNamesContractResolver()
    });

    var content = new StringContent(payloadJson, Encoding.UTF8, "application/json");

    var resp = await client.PostAsync("/v1.0/invoke/timon-link-server/method/users", content);

    var json = await resp.Content.ReadAsStringAsync();

    return JsonConvert.DeserializeObject<PublishRegisteredUserResponse>(json);

  }
}
