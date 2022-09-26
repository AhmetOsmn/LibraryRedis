using Business.Abstract;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class WebRequestManager : IWebRequestService
    {
        public async Task<string> CreateGetRequest(string url, string token = null)
        {
            using var client = new HttpClient();
            if (!String.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var result = await client.GetAsync(url);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
                return await result.Content.ReadAsStringAsync();
            return null;

        }

        public async Task<string> CreatePostRequest(string url, object data, string token = null)
        {
            using var client = new HttpClient();
            if (!String.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var serialiedData = JsonConvert.SerializeObject(data);
            var content = new StringContent(serialiedData, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return await response.Content.ReadAsStringAsync();
            return null;
        }
    }
}
