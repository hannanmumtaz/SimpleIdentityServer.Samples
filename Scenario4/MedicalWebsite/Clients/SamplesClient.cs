using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MedicalWebsite.Clients
{
    public class SampleResponse
    {
        public string Id { get; set; }
    }

    public class SamplesClient
    {
        public async Task<SampleResponse> GetSamples(string subject, string rptToken)
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(Constants.ApiUrl + $"/api/samples")
            };
            request.Headers.Add("Authorization", "Bearer " + rptToken);
            var result = await httpClient.SendAsync(request);
            result.EnsureSuccessStatusCode();
            var content = await result.Content.ReadAsStringAsync();
            var parsed = JObject.Parse(content);
            return new SampleResponse
            {
                Id = parsed["id"].ToString()
            };
        }
    }
}
