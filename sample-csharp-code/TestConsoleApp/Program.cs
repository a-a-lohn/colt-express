using System;
using RestSharp;
namespace TestConsoleApp {
  class TestConsoleApp {
    static void Main(string[] args) {
      var client = new RestClient("https://colt-express-api.azure-api.net/conference/sessions");
      client.Timeout = -1;
      var request = new RestRequest(Method.GET);
      request.AddHeader("Ocp-Apim-Subscription-Key", "3bfc2f7c3c2f46f985ae762363e3edfd");
      request.AddHeader("Ocp-Apim-Trace", "true");
      IRestResponse response = client.Execute(request);
      Console.WriteLine(response.Content);
    }
  }
}