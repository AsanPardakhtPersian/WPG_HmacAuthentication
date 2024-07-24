using System.Text;
using WPG_HmacAuthentication;
using HttpClientToCurl;

Console.WriteLine("Executing for creating a sample...");
var userRequest = new User
{
    ClientRequestId = "3000",
    Amount = "10000"
};
var requestUrl = "/api/v1/Withdraw/wallet/1/bill";
string requestUri = Helpers.EncodeRequestUri(requestUrl.ToLower()); // value: $"{httpRequest.Path}{httpRequest.QueryString.Value}"
string requestMethod = "POST"; //value: httpRequest.Method
//TODO: appid and apikey
var hmacResult = HmacClient.CreateHmac("your-appId", "eW91ci1hcGlLZXk=", requestUri, requestMethod, DateTime.UtcNow, userRequest.ToJson());

Console.WriteLine(hmacResult);

Console.WriteLine("Executed.");

// Showing request which we send to the wpg service:
var httpClient = new HttpClient
{
    BaseAddress = new Uri("http://localhost:8000")
};
httpClient.DefaultRequestHeaders.Add("Authorization", hmacResult);
var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUrl)
{
    Content = new StringContent(userRequest.ToJson()!, Encoding.UTF8, "application/json"),
};

httpClient.GenerateCurlInConsole(httpRequestMessage);

