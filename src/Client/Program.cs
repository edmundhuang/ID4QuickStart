using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        private const string ID4ServerUri = "https://localhost:5011";
        private const string APIServerUri = "https://localhost:5012";
        private static async Task Main()
        {
            await UsePasswordGrant();
            //await UseClientCredential();

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static async Task<HttpClient> CreateClient()
        {
            // discover endpoints from metadata
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync(ID4ServerUri);
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return null;
            }

            return client;
        }

        private static async Task UsePasswordGrant()
        {
            // discover endpoints from metadata
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync(ID4ServerUri);
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            // request token
            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "ro.client",
                ClientSecret = "secret",

                UserName = "alice",
                Password = "alice",
                Scope = "api1"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            // call api
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var uri = $"{APIServerUri}/identity";

            Console.WriteLine($"Request api endpoint: {uri}");
            Console.WriteLine("Response:");

            var response = await apiClient.GetAsync(uri);
            await ConsoleOutput(response);
        }

        private static async Task UseClientCredential()
        {
            // discover endpoints from metadata
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync(ID4ServerUri);
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            // request token
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",

                Scope = "api1"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            // call api
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var uri = $"{APIServerUri}/identity";

            Console.WriteLine($"Request api endpoint: {uri}");
            Console.WriteLine("Response:");

            var response = await apiClient.GetAsync(uri);
            await ConsoleOutput(response);
        }

        private static async Task ConsoleOutput(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
        }
    }
}
