
//Probamos el API
using Microsoft.Identity.Client;
using Properties_to_Rent_Client;
using System.Net.Http.Headers;

public class Properties_Client {

    public static void Main(string[] args) 
    {
        RunAsync().GetAwaiter().GetResult();
    }


    private static async Task RunAsync()
    {
        AuthConfig config = AuthConfig.ReadJsonFromFile("C:/NELSON/Projects/BackEnd/0. Retos BackEnd/.NET/Properties_to_Rent_azure/Properties_to_Rent_Client/appsettings.json");
        IConfidentialClientApplication app;

        app = ConfidentialClientApplicationBuilder.Create(config.ClientId)
            .WithClientSecret(config.ClientSecret)
            .WithAuthority(new Uri(config.Authority))
            .Build();

        string[] ResourceId = new string[] { config.ResourceId };

        AuthenticationResult result = null;

        try
        {
            result = await app.AcquireTokenForClient(ResourceId).ExecuteAsync();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Token Aquired \n");
            Console.WriteLine(result.AccessToken);
            Console.ResetColor();
        }
        catch (MsalClientException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.ResetColor();
        }

        if (!string.IsNullOrEmpty(result.AccessToken))
        {
            var httpClient = new HttpClient();
            var defaultRequestHeaders = httpClient.DefaultRequestHeaders;

            if(defaultRequestHeaders.Accept==null || !defaultRequestHeaders.Accept.Any
                (m=>m.MediaType=="application/json"))
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await httpClient.GetAsync(config.BaseAddress);

                if (response.IsSuccessStatusCode)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    string json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(json);
                }
                else
                {
                    Console.ForegroundColor= ConsoleColor.Red;
                    Console.WriteLine($"Failed to call API: {response.EnsureSuccessStatusCode}");
                    string content =await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Content: {content}");
                }

                Console.ResetColor();

            }
        }

            
    }

}






